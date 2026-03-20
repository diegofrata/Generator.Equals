using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Generator.Equals.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Equals.Models;

sealed class EqualityTypeModelTransformer
{
    readonly GeneratorAttributeSyntaxContext _context;

    public EqualityTypeModelTransformer(GeneratorAttributeSyntaxContext context)
    {
        _context = context;
    }

    public EqualityTypeModel? Transform(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
        {
            return null;
        }

        var attributesMetadata = AttributesMetadata.Instance;

        var equatableAttributeData = _context.Attributes.SingleOrDefault();
        if (equatableAttributeData is null || !attributesMetadata.Equatable.Equals(equatableAttributeData.AttributeClass))
        {
            // Defensive: attribute mismatch during incremental compilation
            return null;
        }

        if (token.IsCancellationRequested)
        {
            return null;
        }

        var explicitMode = equatableAttributeData.GetNamedArgumentValue("Explicit") is true;
        var ignoreInheritedMembers = equatableAttributeData.GetNamedArgumentValue("IgnoreInheritedMembers") is true;

        if (_context.TargetSymbol is not ITypeSymbol symbol)
        {
            // Defensive: non-type symbol during incremental compilation
            return null;
        }

        if (token.IsCancellationRequested)
        {
            return null;
        }

        var baseTypeName = symbol.BaseType?.ToFQF();
        var baseTypeFullname = symbol.BaseType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
        var fullname = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        if (token.IsCancellationRequested)
        {
            return null;
        }

        var containingSymbols = GetContainingSymbols(symbol, includeSelf: symbol.IsRecord);
        if (token.IsCancellationRequested)
        {
            return null;
        }

        // When IgnoreInheritedMembers = false (default), skip overriding properties
        // ONLY IF the base type where the property is originally declared has [Equatable].
        // If the base type doesn't have [Equatable], we must include the property here
        // (with inherited attribute if any) since base.Equals() won't compare it.
        // When IgnoreInheritedMembers = true, include all members including overrides.
        Predicate<ISymbol>? filter = ignoreInheritedMembers
            ? null
            : s => s is not IPropertySymbol prop || !ShouldSkipOverridingProperty(prop, attributesMetadata);

        // For classes (not records), we need to determine if calling the base comparer will
        // reach a meaningful equality implementation. We walk up the inheritance chain
        // to find if any ancestor has a generated EqualityComparer. If so, we should call
        // that ancestor's comparer.
        var nearestComparerAncestor = FindNearestComparerAncestor(symbol.BaseType, attributesMetadata);
        var baseHasEquatable = nearestComparerAncestor != null;
        var nearestComparerAncestorFullname = nearestComparerAncestor?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var bems = EqualityMemberModelTransformer.BuildEqualityModels(symbol, attributesMetadata, explicitMode, filter);

        // When IgnoreInheritedMembers=false and no ancestor has [Equatable],
        // we need to collect all inherited properties to compare them explicitly.
        var inheritedModels = (!ignoreInheritedMembers && !baseHasEquatable)
            ? CollectInheritedProperties(symbol, symbol.BaseType, attributesMetadata, explicitMode)
            : new EquatableImmutableArray<EqualityMemberModel>();

        var model = new EqualityTypeModel
        {
            TypeName = typeName,
            ContainingSymbols = containingSymbols,
            AttributesMetadata = attributesMetadata,
            ExplicitMode = explicitMode,
            IgnoreInheritedMembers = ignoreInheritedMembers,
            BuildEqualityModels = bems,
            IsSealed = symbol.IsSealed,
            BaseTypeName = baseTypeName,
            BaseTypeFullname = baseTypeFullname,
            NearestComparerAncestorFullname = nearestComparerAncestorFullname,
            Fullname = fullname,
            SyntaxKind = _context.TargetNode.Kind(),
            BaseHasEquatable = baseHasEquatable,
            InheritedEqualityModels = inheritedModels
        };

        if (model.SyntaxKind is not (
            SyntaxKind.StructDeclaration
            or SyntaxKind.RecordStructDeclaration
            or SyntaxKind.RecordDeclaration
            or SyntaxKind.ClassDeclaration
            )
           )
        {
            // Todo: Report diagnostic
            // throw new Exception("Expected a struct, record struct, record, or class declaration syntax.");
            return null;
        }

        return model;
    }

    /// <summary>
    /// Determines if a type has the generated nested EqualityComparer class.
    /// This is used for cross-assembly detection since attributes may be erased.
    /// </summary>
    static bool HasGeneratedEqualityComparer(INamedTypeSymbol type)
    {
        var comparerType = type.GetTypeMembers("EqualityComparer").FirstOrDefault();
        if (comparerType is null || !comparerType.IsSealed)
            return false;

        // Check implements IEqualityComparer<T>
        var iEqualityComparer = comparerType.Interfaces
            .FirstOrDefault(i => i.Name == "IEqualityComparer"
                && i.TypeArguments.Length == 1
                && SymbolEqualityComparer.Default.Equals(i.TypeArguments[0], type));

        return iEqualityComparer != null;
    }

    /// <summary>
    /// Finds the nearest ancestor in the inheritance chain that has a generated EqualityComparer.
    /// Returns null if no ancestor has a comparer.
    /// </summary>
    static INamedTypeSymbol? FindNearestComparerAncestor(INamedTypeSymbol? baseType, AttributesMetadata attributesMetadata)
    {
        var current = baseType;
        while (current != null && current.SpecialType != SpecialType.System_Object)
        {
            // Check for [Equatable] attribute
            if (current.HasAttribute(attributesMetadata.Equatable))
            {
                return current;
            }

            // Check for generated EqualityComparer (cross-assembly support)
            if (HasGeneratedEqualityComparer(current))
            {
                return current;
            }

            current = current.BaseType;
        }
        return null;
    }

    /// <summary>
    /// Determines if any ancestor in the inheritance chain has a generated EqualityComparer.
    /// This includes types with [Equatable] attribute or types with a generated EqualityComparer (for cross-assembly).
    /// If so, calling the base comparer will reach that implementation.
    /// </summary>
    static bool AnyAncestorHasEquatable(INamedTypeSymbol? baseType, AttributesMetadata attributesMetadata)
    {
        return FindNearestComparerAncestor(baseType, attributesMetadata) != null;
    }

    /// <summary>
    /// Collects all properties from ancestor types that don't have [Equatable] or a generated EqualityComparer.
    /// Stops when reaching System.Object, a type with [Equatable], or a type with the generated EqualityComparer.
    /// Excludes properties that are overridden by the current type (they'll be handled by BuildEqualityModels).
    /// </summary>
    static EquatableImmutableArray<EqualityMemberModel> CollectInheritedProperties(
        ITypeSymbol currentType,
        INamedTypeSymbol? baseType,
        AttributesMetadata attributesMetadata,
        bool explicitMode)
    {
        // Collect names of properties that are overridden in the current type
        // These will be handled by BuildEqualityModels on the current type
        var overriddenPropertyNames = new HashSet<string>(
            currentType.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.IsOverride)
                .Select(p => p.Name));

        var builder = ImmutableArray.CreateBuilder<EqualityMemberModel>();
        var current = baseType;

        while (current != null && current.SpecialType != SpecialType.System_Object)
        {
            // Stop if this ancestor has [Equatable] - it will handle its own equality via its EqualityComparer
            if (current.HasAttribute(attributesMetadata.Equatable))
            {
                break;
            }

            // Stop if this ancestor has a generated EqualityComparer (cross-assembly detection)
            if (HasGeneratedEqualityComparer(current))
            {
                break;
            }

            // Collect properties from this ancestor, excluding those overridden by the current type
            // For inherited properties, we don't use explicit mode - we want all properties
            Predicate<ISymbol> filter = s => s is not IPropertySymbol prop || !overriddenPropertyNames.Contains(prop.Name);
            var ancestorModels = EqualityMemberModelTransformer.BuildEqualityModels(
                current, attributesMetadata, explicitMode: false, filter);

            // Add to the beginning so that ancestor properties come first (grandparent, then parent)
            builder.InsertRange(0, ancestorModels);

            current = current.BaseType;
        }

        return builder.ToImmutable();
    }

    /// <summary>
    /// Determines if an overriding property should be skipped because a parent type
    /// in the override chain has [Equatable] or a generated EqualityComparer.
    /// </summary>
    static bool ShouldSkipOverridingProperty(IPropertySymbol property, AttributesMetadata attributesMetadata)
    {
        if (!property.IsOverride)
            return false;

        // Walk up the override chain and check each type for [Equatable] or generated EqualityComparer
        var overridden = property.OverriddenProperty;
        while (overridden != null)
        {
            // If any parent type in the chain has [Equatable], skip this property
            if (overridden.ContainingType.HasAttribute(attributesMetadata.Equatable))
            {
                return true;
            }

            // If any parent type has a generated EqualityComparer (cross-assembly), skip this property
            if (overridden.ContainingType is INamedTypeSymbol namedType && HasGeneratedEqualityComparer(namedType))
            {
                return true;
            }

            overridden = overridden.OverriddenProperty;
        }

        // No parent type has [Equatable] or generated comparer, so we need to include this property
        return false;
    }

    public static ImmutableArray<ContainingSymbol> GetContainingSymbols(ISymbol symbol, bool includeSelf = false)
    {
        var parentSymbols = symbol
            .GetParentSymbols(includeSelf)
            .TakeUntilAfterNamespace();

        return parentSymbols.Select(x =>
            {
                if (x.IsNamespace)
                {
                    var format = SymbolDisplayFormat.FullyQualifiedFormat
                        .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted);

                    var namespaceName = x.ToDisplayString(format);

                    return (ContainingSymbol)new NamespaceContainingSymbol
                    {
                        Name = namespaceName
                    };
                }

                var typeDeclarationSyntax = x.DeclaringSyntaxReferences
                    .Select(x => x.GetSyntax())
                    .OfType<TypeDeclarationSyntax>()
                    .First();

                var typeName = x.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

                return (ContainingSymbol)new TypeContainingSymbol
                {
                    Name = typeName,
                    Kind = typeDeclarationSyntax.Kind()
                };
            })
            .ToImmutableArray();
    }
}