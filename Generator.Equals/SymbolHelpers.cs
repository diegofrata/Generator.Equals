using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace Generator.Equals;

public static class SymbolHelpers
{
    public static IEnumerable<ISymbol> GetPropertiesAndFields(this ITypeSymbol symbol)
    {
        _ = symbol ?? throw new System.ArgumentNullException(nameof(symbol));

        var members = symbol
            .GetMembers()
            .Where(x => !x.IsStatic && x switch
            {
                IPropertySymbol { IsIndexer: false } => true,
                IFieldSymbol { CanBeReferencedByName: true } => true,
                _ => false
            });

        foreach (var member in members)
            yield return member;
    }

    // ReSharper disable once InconsistentNaming
    public static string ToNullableFQF(this ISymbol symbol)
    {
        _ = symbol ?? throw new System.ArgumentNullException(nameof(symbol));

        var format = SymbolDisplayFormat.FullyQualifiedFormat
            .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

        return symbol.ToDisplayString(format);
    }

    // ReSharper disable once InconsistentNaming
    public static string ToFQF(this ISymbol symbol)
    {
        _ = symbol ?? throw new System.ArgumentNullException(nameof(symbol));

        return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }

    public static INamedTypeSymbol? GetInterface(this ITypeSymbol symbol, string interfaceFqn)
    {
        _ = symbol ?? throw new System.ArgumentNullException(nameof(symbol));

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

public record DictionaryArgumentsResult(ImmutableArray<ITypeSymbol>? Arguments) : ArgumentsResult(Arguments);

public record EnumerableArgumentsResult(ImmutableArray<ITypeSymbol>? Arguments) : ArgumentsResult(Arguments);

public abstract record ArgumentsResult(ImmutableArray<ITypeSymbol>? Arguments)
{
    public string Name => string.Join(", ", Arguments!.Value.Select(v => v.ToNullableFQF()));
    public bool HasValue => Arguments.HasValue;

    public ImmutableArray<ITypeSymbol>? Value => Arguments;
}