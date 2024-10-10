using System.Collections.Generic;
using System.Collections.Immutable;

namespace Generator.Equals.Models;

internal sealed record AttributesMetadata(
    AttributeMetadata Equatable,
    AttributeMetadata DefaultEquality,
    AttributeMetadata OrderedEquality,
    AttributeMetadata IgnoreEquality,
    AttributeMetadata UnorderedEquality,
    AttributeMetadata ReferenceEquality,
    AttributeMetadata SetEquality,
    AttributeMetadata StringEquality,
    AttributeMetadata CustomEquality,
    EquatableImmutableDictionary<long, string> StringComparisonLookup)
{
    public static AttributesMetadata Instance { get; } = CreateDefault();

    internal static AttributesMetadata CreateDefault()
    {
        var attributesMetadata = new AttributesMetadata(
            Equatable: AttributeMetadata.FromFullName("Generator.Equals.EquatableAttribute")!,
            DefaultEquality: AttributeMetadata.FromFullName("Generator.Equals.DefaultEqualityAttribute")!,
            OrderedEquality: AttributeMetadata.FromFullName("Generator.Equals.OrderedEqualityAttribute")!,
            IgnoreEquality: AttributeMetadata.FromFullName("Generator.Equals.IgnoreEqualityAttribute")!,
            UnorderedEquality: AttributeMetadata.FromFullName("Generator.Equals.UnorderedEqualityAttribute")!,
            ReferenceEquality: AttributeMetadata.FromFullName("Generator.Equals.ReferenceEqualityAttribute")!,
            SetEquality: AttributeMetadata.FromFullName("Generator.Equals.SetEqualityAttribute")!,
            StringEquality: AttributeMetadata.FromFullName("Generator.Equals.StringEqualityAttribute")!,
            CustomEquality: AttributeMetadata.FromFullName("Generator.Equals.CustomEqualityAttribute")!,
            StringComparisonLookup: new Dictionary<long, string>
            {
                [0] = "CurrentCulture",
                [1] = "CurrentCultureIgnoreCase",
                [2] = "InvariantCulture",
                [3] = "InvariantCultureIgnoreCase",
                [4] = "Ordinal",
                [5] = "OrdinalIgnoreCase"
            }.ToImmutableDictionary()
        );

        return attributesMetadata;
    }
}