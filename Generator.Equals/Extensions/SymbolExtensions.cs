using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.Extensions;

static class SymbolExtensions
{
    public static IEnumerable<INamespaceOrTypeSymbol> TakeUntilAfterNamespace(this IEnumerable<INamespaceOrTypeSymbol> symbols)
    {
        _ = symbols ?? throw new System.ArgumentNullException(nameof(symbols));

        foreach (var symbol in symbols)
        {
            yield return symbol;

            if (symbol is INamespaceSymbol)
                yield break;
        }
    }

    public static IEnumerable<INamespaceOrTypeSymbol> GetParentSymbols(this ISymbol symbol, bool includeSelf)
    {
        if (symbol is null)
        {
            throw new System.ArgumentNullException(nameof(symbol));
        }

        if (includeSelf && symbol is INamespaceOrTypeSymbol self)
            yield return self;

        while (true)
        {
            symbol = symbol.ContainingSymbol;

            if (symbol is not INamespaceOrTypeSymbol namespaceOrTypeSymbol)
                yield break;

            yield return namespaceOrTypeSymbol;
        }
    }
    
    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator", Justification = "Performance")]
    public static bool HasAttribute(this ISymbol symbol, AttributeMetadata metadata)
    {
        return symbol.GetAttributeIncludingInherited(metadata) != null;
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, AttributeMetadata metadata)
    {
        return symbol.GetAttributeIncludingInherited(metadata);
    }

    /// <summary>
    /// True if the method is a hand-written <c>public override bool Equals(object)</c>.
    /// </summary>
    public static bool IsEqualsObjectOverride(this IMethodSymbol method) =>
        method is { Name: "Equals", IsOverride: true, MethodKind: MethodKind.Ordinary, DeclaredAccessibility: Accessibility.Public, Parameters.Length: 1 }
        && method.Parameters[0].Type.SpecialType == SpecialType.System_Object
        && method.ReturnType.SpecialType == SpecialType.System_Boolean;

    /// <summary>
    /// True if the method is a hand-written <c>public override int GetHashCode()</c>.
    /// </summary>
    public static bool IsGetHashCodeOverride(this IMethodSymbol method) =>
        method is { Name: "GetHashCode", IsOverride: true, DeclaredAccessibility: Accessibility.Public, Parameters.Length: 0 };

    static AttributeData? GetAttributeIncludingInherited(this ISymbol symbol, AttributeMetadata metadata)
    {
        var current = symbol;

        while (current != null)
        {
            foreach (var attribute in current.GetAttributes())
            {
                if (metadata.Equals(attribute.AttributeClass))
                {
                    return attribute;
                }
            }

            // If this is an overriding property, continue up the override chain
            current = (current as IPropertySymbol)?.OverriddenProperty;
        }

        return null;
    }
}