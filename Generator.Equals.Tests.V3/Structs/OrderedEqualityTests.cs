using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Structs;

/// <summary>
/// Tests for [OrderedEquality] attribute on array property.
/// </summary>
public partial class OrderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        public Sample(string[] addresses)
        {
            Addresses = addresses;
        }

        [OrderedEquality] public string[] Addresses { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content
        { new Sample(new[] { "10 Some Street" }), new Sample(new[] { "10 Some Street" }), true },
        // Different content
        { new Sample(new[] { "10 Some Street" }), new Sample(new[] { "11 Some Street" }), false },
        // Same content, different order (ordered equality - order matters!)
        { new Sample(new[] { "A", "B" }), new Sample(new[] { "B", "A" }), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Structs;

        [Equatable]
        public partial struct OrderedEqualitySample
        {
            public OrderedEqualitySample(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }
        """;
}
