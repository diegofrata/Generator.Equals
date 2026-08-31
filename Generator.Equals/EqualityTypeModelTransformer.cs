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

        var explicitMode = equatableAttributeData.GetNamedArgumentValue(nameof(EquatableAttribute.Explicit)) is true;
        var ignoreInheritedMembers = equatableAttributeData.GetNamedArgumentValue(nameof(EquatableAttribute.IgnoreInheritedMembers)) is true;
        var generateClassEqualityOperators = equatableAttributeData.GeneratesClassEqualityOperators();

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

        // Classify how the base chain owns equality, in a single upward walk. A generated comparer
        // anywhere wins (we delegate to it); otherwise a hand-written complete contract (both
        // Equals(object) and GetHashCode on one type) makes the base "manual" and we delegate to
        // base.Equals()/base.GetHashCode() rather than re-deriving from the base's public properties.
        // BaseHasManualEquality also covers ComparerBehindManual: base.Equals honors the hand-written
        // intermediate, but the inherited comparer can't see it, so Inequalities must use the bridge.
        // The walk also reports whether the IMMEDIATE base owns its comparer: when a comparer ancestor is
        // reached only through a non-comparer intermediate, `{immediateBase}.EqualityComparer` resolves to
        // the inherited ancestor comparer and skips the intermediate's members, so member-level
        // Inequalities delegation is only valid when the immediate base owns its comparer directly.
        var baseEqualityOwnership = ClassifyBaseEquality(symbol.BaseType, attributesMetadata, out var immediateBaseHasComparer);
        var baseHasEquatable = baseEqualityOwnership
            is BaseEqualityOwnership.Comparer or BaseEqualityOwnership.ComparerBehindManual;
        var baseHasManualEquality = baseEqualityOwnership
            is BaseEqualityOwnership.Manual or BaseEqualityOwnership.ComparerBehindManual;

        // Whether the immediate base exposes a public typed Equals(TSelf) (implicit IEquatable<TSelf>).
        // When it does, base delegation casts to the base type and binds to that typed Equals; otherwise
        // it falls back to base.Equals((object?) other). Only consumed on the hand-written-base path.
        var immediateBaseHasTypedEquals = symbol.BaseType is { } typedBase
            && DeclaresPublicTypedEquals(typedBase);

        var bems = EqualityMemberModelTransformer.BuildEqualityModels(symbol, attributesMetadata, explicitMode, filter);

        // When IgnoreInheritedMembers=false and no ancestor has [Equatable], we collect inherited
        // properties to compare them explicitly. CollectInheritedProperties stops at an ancestor that
        // owns equality (via [Equatable] or a complete manual contract), so any properties it covers
        // are delegated through base.Equals() instead of being collected (and compared twice).
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
            Fullname = fullname,
            SyntaxKind = _context.TargetNode.Kind(),
            BaseHasEquatable = baseHasEquatable,
            BaseHasManualEquality = baseHasManualEquality,
            ImmediateBaseHasComparer = immediateBaseHasComparer,
            ImmediateBaseHasTypedEquals = immediateBaseHasTypedEquals,
            InheritedEqualityModels = inheritedModels,
            GenerateClassEqualityOperators = generateClassEqualityOperators,
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
    /// How the base inheritance chain owns equality, if at all.
    /// </summary>
    enum BaseEqualityOwnership
    {
        /// <summary>No ancestor owns equality; inherited members are compared explicitly.</summary>
        None,

        /// <summary>The nearest equality-owning ancestor exposes a generated EqualityComparer ([Equatable] or cross-assembly), with no hand-written contract between it and this type; delegate to that comparer.</summary>
        Comparer,

        /// <summary>A comparer-owning ancestor exists, but a hand-written complete contract sits between it and this type. base.Equals() honors that intermediate, but its members are invisible to the (inherited) comparer, so Inequalities must go through the base-equality bridge instead of delegating member-level.</summary>
        ComparerBehindManual,

        /// <summary>An ancestor hand-rolls a complete Equals/GetHashCode contract and no comparer exists anywhere; delegate via base.Equals()/base.GetHashCode() and the bridge.</summary>
        Manual,
    }

    /// <summary>
    /// Base types whose <c>Equals</c>/<c>GetHashCode</c> are provided by the runtime and must never
    /// be treated as a hand-written value-equality contract (they end the inheritance walk).
    /// </summary>
    static bool IsWellKnownEqualityBase(ITypeSymbol type) =>
        type.SpecialType is SpecialType.System_Object or SpecialType.System_ValueType or SpecialType.System_Enum;

    /// <summary>
    /// True if the type owns equality through a generated comparer — it has <c>[Equatable]</c> or a
    /// generated <c>EqualityComparer</c> (the cross-assembly signal). Calling its comparer reaches it.
    /// </summary>
    static bool OwnsEqualityViaComparer(INamedTypeSymbol type, AttributesMetadata attributesMetadata) =>
        type.HasAttribute(attributesMetadata.Equatable) || type.HasGeneratedEqualityComparer();

    /// <summary>
    /// True if the type declares its own <c>public override bool Equals(object)</c>.
    /// </summary>
    static bool DeclaresEqualsObjectOverride(INamedTypeSymbol type) =>
        type.GetMembers("Equals").OfType<IMethodSymbol>().Any(SymbolExtensions.IsEqualsObjectOverride);

    /// <summary>
    /// True if the type declares its own <c>public override int GetHashCode()</c>.
    /// </summary>
    static bool DeclaresGetHashCodeOverride(INamedTypeSymbol type) =>
        type.GetMembers("GetHashCode").OfType<IMethodSymbol>().Any(SymbolExtensions.IsGetHashCodeOverride);

    /// <summary>
    /// True if the type exposes a public, normal-lookup <c>bool Equals(TSelf)</c> — i.e. an implicit
    /// <c>IEquatable&lt;TSelf&gt;</c> implementation. <c>base.Equals(other as TSelf)</c> binds to this exact
    /// overload, so it is preferred over routing through <c>Equals(object)</c>. (Explicit interface
    /// implementations are not normal-lookup candidates and so are not matched here.)
    /// </summary>
    static bool DeclaresPublicTypedEquals(INamedTypeSymbol type) =>
        type.GetMembers("Equals").OfType<IMethodSymbol>().Any(m =>
            m is { MethodKind: MethodKind.Ordinary, DeclaredAccessibility: Accessibility.Public, IsStatic: false, Parameters.Length: 1 }
            && m.ReturnType.SpecialType == SpecialType.System_Boolean
            && SymbolEqualityComparer.Default.Equals(m.Parameters[0].Type, type));

    /// <summary>
    /// True if the type hand-rolls a complete equality contract — a <c>GetHashCode</c> override plus a
    /// value <c>Equals</c> reachable via a base call: either a public typed <c>Equals(TSelf)</c>
    /// (preferred) or an <c>Equals(object)</c> override (fallback). Not a record (whose equality is
    /// compiler-managed). Requiring GetHashCode keeps delegation mutually consistent, and callers exclude
    /// <c>[Equatable]</c>/comparer ancestors before reaching this check.
    /// </summary>
    static bool DeclaresManualEqualityPair(INamedTypeSymbol type) =>
        !type.IsRecord
        && DeclaresGetHashCodeOverride(type)
        && (DeclaresPublicTypedEquals(type) || DeclaresEqualsObjectOverride(type));

    /// <summary>
    /// Classifies how the base chain owns equality, in a single upward walk. A generated comparer
    /// <em>anywhere</em> in the chain wins (a derived type delegates to the nearest inherited comparer);
    /// whether a hand-written contract sits below it decides <see cref="BaseEqualityOwnership.Comparer"/>
    /// vs <see cref="BaseEqualityOwnership.ComparerBehindManual"/>. With no comparer at all, a hand-written
    /// complete contract makes the base <see cref="BaseEqualityOwnership.Manual"/>. Stops at well-known
    /// bases (object/ValueType/Enum) that never carry a user contract. <paramref name="immediateBaseHasComparer"/>
    /// reports whether the immediate base (the first ancestor visited) owns its own comparer.
    /// </summary>
    static BaseEqualityOwnership ClassifyBaseEquality(INamedTypeSymbol? baseType, AttributesMetadata attributesMetadata, out bool immediateBaseHasComparer)
    {
        immediateBaseHasComparer = false;
        var hasManual = false;
        var isImmediate = true;
        for (var current = baseType; current != null && !IsWellKnownEqualityBase(current); current = current.BaseType, isImmediate = false)
        {
            var ownsComparer = OwnsEqualityViaComparer(current, attributesMetadata);
            if (isImmediate)
                immediateBaseHasComparer = ownsComparer;

            if (ownsComparer)
                return hasManual ? BaseEqualityOwnership.ComparerBehindManual : BaseEqualityOwnership.Comparer;

            if (!hasManual && DeclaresManualEqualityPair(current))
                hasManual = true;
        }

        return hasManual ? BaseEqualityOwnership.Manual : BaseEqualityOwnership.None;
    }

    /// <summary>
    /// Collects all properties from ancestor types that don't own their equality.
    /// Stops when reaching System.Object, a type with [Equatable], a type with the generated
    /// EqualityComparer, or a type that hand-rolls a complete equality contract (Equals + GetHashCode) —
    /// the latter's members are delegated through base.Equals()/base.GetHashCode() instead.
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

        while (current != null && !IsWellKnownEqualityBase(current))
        {
            // Stop at an ancestor that owns equality: it either exposes a generated comparer, or
            // hand-rolls a complete contract that base.Equals()/base.GetHashCode() delegates to.
            // Collecting its (and its ancestors') members here would compare them twice.
            if (OwnsEqualityViaComparer(current, attributesMetadata) || DeclaresManualEqualityPair(current))
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
            if (overridden.ContainingType is INamedTypeSymbol namedType && namedType.HasGeneratedEqualityComparer())
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