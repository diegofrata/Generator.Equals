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

        // One upward walk classifies how the base chain owns equality and yields the plain ancestors
        // (no equality of their own) below the equality-owning one. Those ancestors' members are collected
        // and compared here, because whatever base.Equals() reaches knows nothing about them. Records are
        // exempt when a comparer ancestor exists: a non-[Equatable] record intermediate has
        // compiler-synthesized equality over its own members that base.Equals() already honors.
        var baseChain = ClassifyBaseEquality(symbol.BaseType, attributesMetadata);
        var collectInherited = !ignoreInheritedMembers && (baseChain.Ownership == BaseEqualityOwnership.None || !symbol.IsRecord);

        var bems = EqualityMemberModelTransformer.BuildEqualityModels(symbol, attributesMetadata, explicitMode, filter);
        var inheritedModels = collectInherited
            ? CollectInheritedProperties(symbol, baseChain.PlainAncestors, attributesMetadata)
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
            BaseEquality = baseChain.Ownership,
            ImmediateBaseHasComparer = baseChain.ImmediateBaseHasComparer,
            BaseEqualsArgument = baseChain.BaseEqualsArgument,
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
    /// (preferred, reported through <paramref name="hasTypedEquals"/>) or an <c>Equals(object)</c>
    /// override (fallback). Not a record (whose equality is compiler-managed). Requiring GetHashCode keeps
    /// delegation mutually consistent; callers exclude comparer-owning ancestors before this check.
    /// </summary>
    static bool DeclaresManualEqualityPair(INamedTypeSymbol type, out bool hasTypedEquals)
    {
        hasTypedEquals = false;
        if (type.IsRecord || !type.GetMembers("GetHashCode").OfType<IMethodSymbol>().Any(SymbolExtensions.IsGetHashCodeOverride))
            return false;

        hasTypedEquals = DeclaresPublicTypedEquals(type);
        return hasTypedEquals || type.GetMembers("Equals").OfType<IMethodSymbol>().Any(SymbolExtensions.IsEqualsObjectOverride);
    }

    /// <summary>Result of <see cref="ClassifyBaseEquality"/>: everything the model needs to know about the base chain.</summary>
    readonly record struct BaseChain(
        BaseEqualityOwnership Ownership,
        bool ImmediateBaseHasComparer,
        string? BaseEqualsArgument,
        ImmutableArray<INamedTypeSymbol> PlainAncestors);

    /// <summary>
    /// Classifies how the base chain owns equality in a single upward walk (see
    /// <see cref="BaseEqualityOwnership"/>), stopping at well-known bases (object/ValueType/Enum) that
    /// never carry a user contract. Also yields the plain ancestors visited before the equality-owning one
    /// (nearest first) and the class <c>base.Equals</c> argument: cast to the immediate base on the comparer
    /// path; on the hand-written path, cast to the ancestor owning the contract when it exposes a public typed
    /// <c>Equals(TSelf)</c> (it may sit behind plain intermediates, so casting to the immediate base would
    /// miss it), otherwise to <c>object</c> so resolution reaches its <c>Equals(object)</c> rather than an
    /// <c>[Equatable]</c> ancestor's generated <c>Equals(TAncestor?)</c>.
    /// </summary>
    static BaseChain ClassifyBaseEquality(INamedTypeSymbol? baseType, AttributesMetadata attributesMetadata)
    {
        var immediateBaseHasComparer = baseType is { } b && OwnsEqualityViaComparer(b, attributesMetadata);
        var plainAncestors = ImmutableArray.CreateBuilder<INamedTypeSymbol>();
        string? manualEqualsArgument = null;

        for (var current = baseType; current != null && !IsWellKnownEqualityBase(current); current = current.BaseType)
        {
            if (OwnsEqualityViaComparer(current, attributesMetadata))
            {
                return manualEqualsArgument is null
                    ? new BaseChain(BaseEqualityOwnership.Comparer, immediateBaseHasComparer, $"other as {baseType!.ToFQF()}", plainAncestors.ToImmutable())
                    : new BaseChain(BaseEqualityOwnership.ComparerBehindManual, immediateBaseHasComparer, manualEqualsArgument, plainAncestors.ToImmutable());
            }

            if (manualEqualsArgument is not null)
                continue;

            if (DeclaresManualEqualityPair(current, out var hasTypedEquals))
                manualEqualsArgument = hasTypedEquals ? $"other as {current.ToFQF()}" : "(object?) other";
            else
                plainAncestors.Add(current);
        }

        return manualEqualsArgument is null
            ? new BaseChain(BaseEqualityOwnership.None, immediateBaseHasComparer, null, plainAncestors.ToImmutable())
            : new BaseChain(BaseEqualityOwnership.Manual, immediateBaseHasComparer, manualEqualsArgument, plainAncestors.ToImmutable());
    }

    /// <summary>
    /// Collects the properties of <paramref name="plainAncestors"/> (nearest first) in grandparent-to-parent
    /// order, excluding properties overridden by the current type (handled by BuildEqualityModels).
    /// </summary>
    static EquatableImmutableArray<EqualityMemberModel> CollectInheritedProperties(
        ITypeSymbol currentType,
        ImmutableArray<INamedTypeSymbol> plainAncestors,
        AttributesMetadata attributesMetadata)
    {
        var overriddenPropertyNames = new HashSet<string>(
            currentType.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(p => p.IsOverride)
                .Select(p => p.Name));

        // Inherited properties never use explicit mode - we want all of them.
        Predicate<ISymbol> filter = s => s is not IPropertySymbol prop || !overriddenPropertyNames.Contains(prop.Name);

        var builder = ImmutableArray.CreateBuilder<EqualityMemberModel>();
        for (var i = plainAncestors.Length - 1; i >= 0; i--)
        {
            builder.AddRange(EqualityMemberModelTransformer.BuildEqualityModels(
                plainAncestors[i], attributesMetadata, explicitMode: false, filter));
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