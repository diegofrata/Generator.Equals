using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generator.Equals;

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
    public static string ToFQF(this ISymbol symbol) => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    public static INamedTypeSymbol? GetInterface(this ITypeSymbol symbol, string interfaceFqn)
    {
        var result = symbol.AllInterfaces
            .FirstOrDefault(x =>
                x.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == interfaceFqn);

        if (result == null && symbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ==
            interfaceFqn)
            result = (INamedTypeSymbol)symbol;

        return result;
    }

    public static ArgumentsResult? GetIEnumerableTypeArguments(this ITypeSymbol symbol)
    {
        var res = symbol.GetInterface("global::System.Collections.Generic.IEnumerable<T>")?.TypeArguments;
        return res == null 
            ? null 
            : new EnumerableArgumentsResult(res);
    }

    public static ArgumentsResult? GetIDictionaryTypeArguments(this ITypeSymbol symbol)
    {
        var res = symbol.GetInterface("global::System.Collections.Generic.IDictionary<TKey, TValue>")?.TypeArguments;
        return res == null 
            ? null 
            : new DictionaryArgumentsResult(res);
    }
}

public record DictionaryArgumentsResult(ImmutableArray<ITypeSymbol>? Arguments) : ArgumentsResult(Arguments)
{
    public static implicit operator DictionaryArgumentsResult?(ImmutableArray<ITypeSymbol>? arguments)
        => arguments.HasValue ? new DictionaryArgumentsResult(arguments) : null;
}

public record EnumerableArgumentsResult(ImmutableArray<ITypeSymbol>? Arguments) : ArgumentsResult(Arguments)
{
    public static implicit operator EnumerableArgumentsResult?(ImmutableArray<ITypeSymbol>? arguments) 
        => arguments.HasValue ? new EnumerableArgumentsResult(arguments) : null;

}

public abstract record ArgumentsResult(ImmutableArray<ITypeSymbol>? Arguments)
{
    public string Name => string.Join(", ", Arguments!.Value);
    public bool HasValue => Arguments.HasValue;
    
    public ImmutableArray<ITypeSymbol>? Value => Arguments;
}