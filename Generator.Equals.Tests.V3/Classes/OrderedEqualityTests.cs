using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Classes;

/// <summary>
/// Tests for [OrderedEquality] on array properties.
/// Order matters when comparing collections.
/// </summary>
public partial class OrderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string[] addresses)
        {
            Addresses = addresses;
        }

        [OrderedEquality] public string[] Addresses { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content, same order
        { new Sample(new[] { "10 Downing St" }), new Sample(new[] { "10 Downing St" }), true },
        // Different content
        { new Sample(new[] { "10 Downing St" }), new Sample(new[] { "Bricklane" }), false },
        // Same content, different order (ordered equality - order matters!)
        { new Sample(new[] { "A", "B" }), new Sample(new[] { "B", "A" }), false },
        // Multiple items, same order
        { new Sample(new[] { "A", "B", "C" }), new Sample(new[] { "A", "B", "C" }), true },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Classes;

        [Equatable]
        public partial class OrderedEqualitySample
        {
            public OrderedEqualitySample(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }
        """;
}
