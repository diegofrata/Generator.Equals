using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.Extensions;

public static class SymbolExtensions
{
    public static IEnumerable<INamespaceOrTypeSymbol> TakeUntilNamespace(this IEnumerable<INamespaceOrTypeSymbol> symbols)
    {
        foreach (var symbol in symbols)
        {
            yield return symbol;

            if (symbol is INamespaceSymbol)
                yield break;
        }
    }
    
    public static IEnumerable<INamespaceOrTypeSymbol> GetParentSymbols(this ISymbol symbol, bool includeSelf)
    {
        if (includeSelf && symbol is INamespaceOrTypeSymbol self)
            yield return self;

        while (true)
        {
            symbol = symbol.ContainingSymbol;

            if (!(symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol))
                yield break;

            yield return namespaceOrTypeSymbol;
        }
    }

    public static bool IsAttribute(this INamedTypeSymbol symbol, Compilation compilation)
    {
        var attributeType = compilation.GetTypeByMetadataName("System.Attribute");
        return symbol.InheritsFrom(attributeType);
    }

    private static bool InheritsFrom(this INamedTypeSymbol symbol, INamedTypeSymbol baseType)
    {
        while (symbol != null)
        {
            if (SymbolEqualityComparer.Default.Equals(symbol, baseType))
            {
                return true;
            }

            symbol = symbol.BaseType;
        }

        return false;
    }
}