using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public static class SymbolHelpers
    {
        public static IEnumerable<ISymbol> GetPropertiesAndFields(this ITypeSymbol symbol)
        {
            var members = symbol
                .GetMembers()
                .Where(x =>
                {
                    return !x.IsStatic && x switch
                    {
                        IPropertySymbol { IsIndexer: false } => true,
                        IFieldSymbol { CanBeReferencedByName: true } => true,
                        _ => false
                    };
                });

            foreach (var member in members)
                yield return member;
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

        public static INamedTypeSymbol? GetInterface(this ITypeSymbol symbol, string interfaceFqn)
        {
            var result = symbol.AllInterfaces
                .FirstOrDefault(x => x.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == interfaceFqn);

            if (result == null && symbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == interfaceFqn)
                result = (INamedTypeSymbol)symbol;

            return result;
        }

        public static ImmutableArray<ITypeSymbol>? GetIEnumerableTypeArguments(this ITypeSymbol symbol)
        {
            return symbol.GetInterface("global::System.Collections.Generic.IEnumerable<T>")?.TypeArguments;
        }

        public static ImmutableArray<ITypeSymbol>? GetIDictionaryTypeArguments(this ITypeSymbol symbol)
        {
            return symbol.GetInterface("global::System.Collections.Generic.IDictionary<TKey, TValue>")?.TypeArguments;
        }
    }
}