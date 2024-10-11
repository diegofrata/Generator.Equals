﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.Extensions;

public static class SymbolExtensions
{
    public static IEnumerable<INamespaceOrTypeSymbol> TakeUntilAfterNamespace(this IEnumerable<INamespaceOrTypeSymbol> symbols)
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

            if (symbol is not INamespaceOrTypeSymbol namespaceOrTypeSymbol)
                yield break;

            yield return namespaceOrTypeSymbol;
        }
    }
}