using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Generator.Equals.Extensions;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Generator.Equals.Analyzers;

/// <summary>
/// Roslyn analyzer that catches common mistakes when using Generator.Equals attributes.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class EquatableAnalyzer : DiagnosticAnalyzer
{
    private static readonly AttributesMetadata Metadata = AttributesMetadata.Instance;

    /// <summary>
    /// All property-level equality attributes.
    /// </summary>
    private static readonly ImmutableArray<AttributeMetadata> PropertyEqualityAttributes = ImmutableArray.Create(
        Metadata.DefaultEquality,
        Metadata.OrderedEquality,
        Metadata.UnorderedEquality,
        Metadata.SetEquality,
        Metadata.StringEquality,
        Metadata.CustomEquality,
        Metadata.ReferenceEquality,
        Metadata.IgnoreEquality);

    /// <summary>
    /// Collection equality attributes.
    /// </summary>
    private static readonly ImmutableArray<AttributeMetadata> CollectionEqualityAttributes = ImmutableArray.Create(
        Metadata.OrderedEquality,
        Metadata.UnorderedEquality,
        Metadata.SetEquality);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(
        DiagnosticDescriptors.CollectionMissingAttribute,          // GE001
        DiagnosticDescriptors.ComplexTypeMissingEquatable,         // GE002
        DiagnosticDescriptors.CollectionElementMissingEquatable,   // GE003
        DiagnosticDescriptors.OrphanedEqualityAttribute,           // GE004
        DiagnosticDescriptors.ManualEqualsImplementation,          // GE005
        DiagnosticDescriptors.NonPartialType,                      // GE006
        DiagnosticDescriptors.ConflictingAttributes,               // GE007
        DiagnosticDescriptors.StringEqualityOnNonString,           // GE008
        DiagnosticDescriptors.CollectionAttributeOnNonCollection); // GE009

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        // Register for type declarations to check [Equatable] types
        context.RegisterSymbolAction(AnalyzeTypeSymbol, SymbolKind.NamedType);

        // Register for property/field symbols to check orphaned attributes
        context.RegisterSymbolAction(AnalyzePropertyOrFieldSymbol, SymbolKind.Property, SymbolKind.Field);
    }

    private static void AnalyzeTypeSymbol(SymbolAnalysisContext context)
    {
        var typeSymbol = (INamedTypeSymbol)context.Symbol;

        // Only analyze types with [Equatable] attribute
        if (!typeSymbol.HasAttribute(Metadata.Equatable))
            return;

        // GE006: Check if type is partial
        CheckNonPartialType(context, typeSymbol);

        // GE005: Check for manual Equals/GetHashCode implementation
        CheckManualEqualsImplementation(context, typeSymbol);

        // Get the Equatable attribute to check Explicit mode
        var equatableAttr = typeSymbol.GetAttribute(Metadata.Equatable);
        var explicitMode = GetExplicitMode(equatableAttr);

        // Analyze each property/field in the type
        foreach (var member in typeSymbol.GetPropertiesAndFields())
        {
            var memberType = GetMemberType(member);
            if (memberType == null)
                continue;

            AnalyzeMemberInEquatableType(context, member, memberType, explicitMode);
        }
    }

    private static void AnalyzePropertyOrFieldSymbol(SymbolAnalysisContext context)
    {
        var memberSymbol = context.Symbol;
        var containingType = memberSymbol.ContainingType;

        if (containingType == null)
            return;

        // Get all equality attributes on this member
        var memberAttributes = GetEqualityAttributesOnMember(memberSymbol);

        if (memberAttributes.Count == 0)
            return;

        // GE004: Check if containing type has [Equatable]
        if (!containingType.HasAttribute(Metadata.Equatable))
        {
            foreach (var (attr, attrData) in memberAttributes)
            {
                var location = attrData.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                               ?? memberSymbol.Locations.FirstOrDefault()
                               ?? Location.None;

                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.OrphanedEqualityAttribute,
                    location,
                    memberSymbol.Name,
                    attr.ShortName,
                    containingType.Name));
            }
            return;
        }

        // GE007: Check for conflicting attributes (only directly declared, not inherited)
        var directlyDeclaredAttributes = GetDirectlyDeclaredEqualityAttributes(memberSymbol);
        CheckConflictingAttributes(context, memberSymbol, directlyDeclaredAttributes);

        // GE008: Check [StringEquality] on non-string
        CheckStringEqualityOnNonString(context, memberSymbol, memberAttributes);

        // GE009: Check collection attributes on non-collection
        CheckCollectionAttributeOnNonCollection(context, memberSymbol, memberAttributes);
    }

    private static void AnalyzeMemberInEquatableType(
        SymbolAnalysisContext context,
        ISymbol member,
        ITypeSymbol memberType,
        bool explicitMode)
    {
        // Get equality attributes on this member
        var memberAttributes = GetEqualityAttributesOnMember(member);

        // Skip all checks if member has [IgnoreEquality] or [ReferenceEquality]
        if (memberAttributes.Any(x => x.Metadata.Equals(Metadata.IgnoreEquality) ||
                                       x.Metadata.Equals(Metadata.ReferenceEquality)))
            return;

        // In explicit mode, only check members with [DefaultEquality]
        if (explicitMode && !memberAttributes.Any(x => x.Metadata.Equals(Metadata.DefaultEquality)))
            return;

        // Check if member has [DefaultEquality] - suppresses GE002/GE003 but not GE001
        // DefaultEquality explicitly indicates the user wants default equality behavior (e.g., for types
        // like protobuf-generated classes that implement their own Equals/GetHashCode)
        var hasDefaultEquality = memberAttributes.Any(x => x.Metadata.Equals(Metadata.DefaultEquality));

        // Check if member has a collection equality attribute
        var hasCollectionAttribute = memberAttributes.Any(x =>
            CollectionEqualityAttributes.Contains(x.Metadata));

        // GE001: Collection without collection equality attribute
        if (memberType.IsCollection() && !hasCollectionAttribute)
        {
            var location = GetMemberTypeLocation(member) ?? member.Locations.FirstOrDefault() ?? Location.None;
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.CollectionMissingAttribute,
                location,
                member.Name));
        }

        // Skip GE002/GE003 if [DefaultEquality] is present - user explicitly chose default behavior
        if (hasDefaultEquality)
            return;

        // GE002: Complex type without [Equatable]
        if (memberType.IsComplexType() && !HasEquatableAttribute(memberType))
        {
            var location = GetMemberTypeLocation(member) ?? member.Locations.FirstOrDefault() ?? Location.None;
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.ComplexTypeMissingEquatable,
                location,
                member.Name,
                memberType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
        }

        // GE003: Collection element type is complex without [Equatable]
        if (memberType.IsCollection())
        {
            var elementType = memberType.GetCollectionElementType();
            if (elementType != null && elementType.IsComplexType() && !HasEquatableAttribute(elementType))
            {
                var location = GetMemberTypeLocation(member) ?? member.Locations.FirstOrDefault() ?? Location.None;
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.CollectionElementMissingEquatable,
                    location,
                    member.Name,
                    elementType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
            }
        }
    }

    private static void CheckNonPartialType(SymbolAnalysisContext context, INamedTypeSymbol typeSymbol)
    {
        // Check if ALL declarations have the partial modifier
        var allDeclarations = typeSymbol.DeclaringSyntaxReferences;
        var hasPartialDeclaration = false;

        foreach (var syntaxRef in allDeclarations)
        {
            var syntax = syntaxRef.GetSyntax();
            if (syntax is TypeDeclarationSyntax typeDecl &&
                typeDecl.Modifiers.Any(SyntaxKind.PartialKeyword))
            {
                hasPartialDeclaration = true;
                break;
            }
        }

        if (!hasPartialDeclaration)
        {
            var location = typeSymbol.Locations.FirstOrDefault() ?? Location.None;
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.NonPartialType,
                location,
                typeSymbol.Name));
        }
    }

    private static void CheckManualEqualsImplementation(SymbolAnalysisContext context, INamedTypeSymbol typeSymbol)
    {
        // Skip records - they auto-generate Equals/GetHashCode and that's expected behavior
        if (typeSymbol.IsRecord)
            return;

        foreach (var member in typeSymbol.GetMembers())
        {
            if (member is IMethodSymbol method)
            {
                // Check for Equals(object) override
                if (method is
                    {
                        Name: "Equals",
                        IsOverride: true,
                        Parameters.Length: 1,
                        DeclaredAccessibility: Accessibility.Public
                    } && method.Parameters[0].Type.SpecialType == SpecialType.System_Object)
                {
                    var location = method.Locations.FirstOrDefault() ?? Location.None;
                    context.ReportDiagnostic(Diagnostic.Create(
                        DiagnosticDescriptors.ManualEqualsImplementation,
                        location,
                        typeSymbol.Name,
                        "Equals(object)"));
                }

                // Check for GetHashCode override
                if (method is
                    {
                        Name: "GetHashCode",
                        IsOverride: true,
                        Parameters.Length: 0,
                        DeclaredAccessibility: Accessibility.Public
                    })
                {
                    var location = method.Locations.FirstOrDefault() ?? Location.None;
                    context.ReportDiagnostic(Diagnostic.Create(
                        DiagnosticDescriptors.ManualEqualsImplementation,
                        location,
                        typeSymbol.Name,
                        "GetHashCode()"));
                }
            }
        }
    }

    private static void CheckConflictingAttributes(
        SymbolAnalysisContext context,
        ISymbol memberSymbol,
        List<(AttributeMetadata Metadata, AttributeData Data)> memberAttributes)
    {
        if (memberAttributes.Count <= 1)
            return;

        // IgnoreEquality conflicts with all other equality attributes
        var ignoreAttr = memberAttributes.FirstOrDefault(x => x.Metadata.Equals(Metadata.IgnoreEquality));
        var otherAttrs = memberAttributes.Where(x => !x.Metadata.Equals(Metadata.IgnoreEquality)).ToList();

        if (ignoreAttr.Metadata != null && otherAttrs.Count > 0)
        {
            var location = otherAttrs[0].Data.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                           ?? memberSymbol.Locations.FirstOrDefault()
                           ?? Location.None;
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.ConflictingAttributes,
                location,
                memberSymbol.Name,
                ignoreAttr.Metadata.ShortName,
                otherAttrs[0].Metadata.ShortName));
            return;
        }

        // Multiple non-IgnoreEquality attributes conflict
        // DefaultEquality doesn't conflict with collection attributes
        var nonDefaultAttrs = memberAttributes
            .Where(x => !x.Metadata.Equals(Metadata.DefaultEquality) &&
                       !x.Metadata.Equals(Metadata.IgnoreEquality))
            .ToList();

        if (nonDefaultAttrs.Count > 1)
        {
            var location = nonDefaultAttrs[1].Data.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                           ?? memberSymbol.Locations.FirstOrDefault()
                           ?? Location.None;
            context.ReportDiagnostic(Diagnostic.Create(
                DiagnosticDescriptors.ConflictingAttributes,
                location,
                memberSymbol.Name,
                nonDefaultAttrs[0].Metadata.ShortName,
                nonDefaultAttrs[1].Metadata.ShortName));
        }
    }

    private static void CheckStringEqualityOnNonString(
        SymbolAnalysisContext context,
        ISymbol memberSymbol,
        List<(AttributeMetadata Metadata, AttributeData Data)> memberAttributes)
    {
        var stringEqualityAttr = memberAttributes.FirstOrDefault(x => x.Metadata.Equals(Metadata.StringEquality));
        if (stringEqualityAttr.Metadata == null)
            return;

        var memberType = GetMemberType(memberSymbol);
        if (memberType?.SpecialType == SpecialType.System_String)
            return;

        var location = stringEqualityAttr.Data.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                       ?? memberSymbol.Locations.FirstOrDefault()
                       ?? Location.None;
        context.ReportDiagnostic(Diagnostic.Create(
            DiagnosticDescriptors.StringEqualityOnNonString,
            location,
            memberSymbol.Name,
            memberType?.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat) ?? "unknown"));
    }

    private static void CheckCollectionAttributeOnNonCollection(
        SymbolAnalysisContext context,
        ISymbol memberSymbol,
        List<(AttributeMetadata Metadata, AttributeData Data)> memberAttributes)
    {
        var memberType = GetMemberType(memberSymbol);
        if (memberType == null)
            return;

        // Skip if it's actually a collection
        if (memberType.IsCollection())
            return;

        // Check for collection attributes
        foreach (var (attrMetadata, attrData) in memberAttributes)
        {
            if (CollectionEqualityAttributes.Contains(attrMetadata))
            {
                var location = attrData.ApplicationSyntaxReference?.GetSyntax().GetLocation()
                               ?? memberSymbol.Locations.FirstOrDefault()
                               ?? Location.None;
                context.ReportDiagnostic(Diagnostic.Create(
                    DiagnosticDescriptors.CollectionAttributeOnNonCollection,
                    location,
                    memberSymbol.Name,
                    attrMetadata.ShortName,
                    memberType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)));
            }
        }
    }

    private static List<(AttributeMetadata Metadata, AttributeData Data)> GetEqualityAttributesOnMember(ISymbol member)
    {
        var result = new List<(AttributeMetadata, AttributeData)>();

        foreach (var attrMetadata in PropertyEqualityAttributes)
        {
            var attrData = member.GetAttribute(attrMetadata);
            if (attrData != null)
            {
                result.Add((attrMetadata, attrData));
            }
        }

        return result;
    }

    /// <summary>
    /// Gets equality attributes declared directly on the member (not inherited from overridden properties).
    /// Used for conflict detection to avoid false positives when child overrides parent's attribute.
    /// </summary>
    private static List<(AttributeMetadata Metadata, AttributeData Data)> GetDirectlyDeclaredEqualityAttributes(ISymbol member)
    {
        var result = new List<(AttributeMetadata, AttributeData)>();

        foreach (var attribute in member.GetAttributes())
        {
            foreach (var attrMetadata in PropertyEqualityAttributes)
            {
                if (attrMetadata.Equals(attribute.AttributeClass))
                {
                    result.Add((attrMetadata, attribute));
                    break;
                }
            }
        }

        return result;
    }

    private static ITypeSymbol? GetMemberType(ISymbol member)
    {
        return member switch
        {
            IPropertySymbol prop => prop.Type,
            IFieldSymbol field => field.Type,
            _ => null
        };
    }

    private static Location? GetMemberTypeLocation(ISymbol member)
    {
        var syntaxRef = member.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef == null)
            return null;

        var syntax = syntaxRef.GetSyntax();
        return syntax switch
        {
            PropertyDeclarationSyntax propSyntax => propSyntax.Type.GetLocation(),
            FieldDeclarationSyntax fieldSyntax => fieldSyntax.Declaration.Type.GetLocation(),
            VariableDeclaratorSyntax varSyntax when varSyntax.Parent is VariableDeclarationSyntax varDecl
                => varDecl.Type.GetLocation(),
            _ => null
        };
    }

    private static bool GetExplicitMode(AttributeData? equatableAttr)
    {
        if (equatableAttr == null)
            return false;

        foreach (var namedArg in equatableAttr.NamedArguments)
        {
            if (namedArg.Key == "Explicit" && namedArg.Value.Value is bool explicitValue)
                return explicitValue;
        }

        return false;
    }

    private static bool HasEquatableAttribute(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        return namedType.HasAttribute(Metadata.Equatable);
    }
}
