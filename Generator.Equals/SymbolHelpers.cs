using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public static class SymbolHelpers
    {
        public static IEnumerable<IPropertySymbol> GetProperties(this ITypeSymbol symbol)
        {
            foreach (var property in symbol.GetMembers().OfType<IPropertySymbol>())
                yield return property;
        }

        // ReSharper disable once InconsistentNaming
        public static string ToNullableFQF(this ISymbol symbol) =>
            symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
                    SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
                )
            );

        // ReSharper disable once InconsistentNaming
        public static string ToFQF(this ISymbol symbol) =>
            symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        public static AttributeData? GetAttribute(this ISymbol symbol, INamedTypeSymbol attribute)
        {
            return symbol
                .GetAttributes()
                .FirstOrDefault(x => x.AttributeClass?.Equals(attribute, SymbolEqualityComparer.Default) == true);
        }

        public static bool HasAttribute(this ISymbol symbol, INamedTypeSymbol attribute) =>
            GetAttribute(symbol, attribute) != null;

        public static INamedTypeSymbol? GetInterface(this IPropertySymbol property, string interfaceFqn)
        {
            return property.Type.AllInterfaces
                .FirstOrDefault(x => x.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == interfaceFqn);
        }

        public static ImmutableArray<ITypeSymbol>? GetIEnumerableTypeArguments(this IPropertySymbol property)
        {
            return property.GetInterface("global::System.Collections.Generic.IEnumerable<T>")?.TypeArguments;
        }
        
        public static ImmutableArray<ITypeSymbol>? GetIDictionaryTypeArguments(this IPropertySymbol property)
        {
            return property.GetInterface("global::System.Collections.Generic.IDictionary<TKey, TValue>")?.TypeArguments;
        }
    }
}