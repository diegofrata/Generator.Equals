using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Generator.Equals;

using Microsoft.CodeAnalysis;

namespace Generator.Equals;

public record AttributesMetadata(
    AttributeMetadata Equatable,
    AttributeMetadata DefaultEquality,
    AttributeMetadata OrderedEquality,
    AttributeMetadata IgnoreEquality,
    AttributeMetadata UnorderedEquality,
    AttributeMetadata ReferenceEquality,
    AttributeMetadata SetEquality,
    AttributeMetadata StringEquality,
    AttributeMetadata CustomEquality,
    ImmutableDictionary<long, string> StringComparisonLookup)
{
    public static AttributesMetadata Instance { get; } = CreateDefault();

    static AttributesMetadata CreateDefault()
    {
        var lookup = new Dictionary<long, string>
        {
            [0] = "CurrentCulture",
            [1] = "CurrentCultureIgnoreCase",
            [2] = "InvariantCulture",
            [3] = "InvariantCultureIgnoreCase",
            [4] = "Ordinal",
            [5] = "OrdinalIgnoreCase"
        }.ToImmutableDictionary();

        var attributesMetadata = new AttributesMetadata(
               AttributeMetadata.FromFullName("Generator.Equals.EquatableAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.DefaultEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.OrderedEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.IgnoreEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.UnorderedEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.ReferenceEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.SetEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.StringEqualityAttribute")!,
               AttributeMetadata.FromFullName("Generator.Equals.CustomEqualityAttribute")!,
               lookup
           );

        return attributesMetadata;
    }
}

public record AttributeMetadata(string FullName, string Namespace, string LongName, string ShortName)
{
    // Returns an AttributeMetadata instance for a full name
    public static AttributeMetadata FromFullName(string fullName)
    {
        var parts = fullName.Split('.');

        var name = parts.Last();

        var shortName = name.EndsWith("Attribute") ? name.Substring(0, name.Length - "Attribute".Length) : name;
        var longName = name.EndsWith("Attribute") ? name : name + "Attribute";

        var metadata = new AttributeMetadata
        (
            FullName: fullName,
            Namespace: parts.Length == 1 ? string.Empty : string.Join(".", parts.Take(parts.Length - 1)),
            LongName: longName,
            ShortName: shortName
        );

        return metadata;
    }

    public bool Equals(INamedTypeSymbol? symbol)
    {
        if (symbol == null)
        {
            return false;
        }

        var nameEquals = string.Equals(symbol.Name, LongName, StringComparison.Ordinal)
                         || string.Equals(symbol.Name, ShortName, StringComparison.Ordinal);

        var containingNamespace = symbol.ContainingNamespace.ToDisplayString();
        var namespaceEquals = string.Equals(containingNamespace, Namespace, StringComparison.Ordinal);

        return nameEquals && namespaceEquals;
    }
}

public static class AttributeMetadataExtensions
{
    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator", Justification = "Performance")]
    public static bool HasAttribute(this ISymbol symbol, AttributeMetadata metadata)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (metadata.Equals(attribute.AttributeClass))
            {
                return true;
            }
        }

        return false;
    }

    //GetAttribute
    public static AttributeData? GetAttribute(this ISymbol symbol, AttributeMetadata metadata)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (metadata.Equals(attribute.AttributeClass))
            {
                return attribute;
            }
        }

        return null;
    }
}