using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public static class SymbolExtensions
    {
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
}