using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality comparison using fields (not just properties).
/// </summary>
public partial class FieldEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string[] addresses)
        {
            _addresses = addresses;
        }

        [OrderedEquality] readonly string[] _addresses;
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same array content should be equal
        { new Sample(new[] { "10 Downing St" }), new Sample(new[] { "10 Downing St" }), true },
        // Different array content should not be equal
        { new Sample(new[] { "10 Downing St" }), new Sample(new[] { "Bricklane" }), false },
        // Multiple items, same order
        { new Sample(new[] { "A", "B" }), new Sample(new[] { "A", "B" }), true },
        // Multiple items, different order (ordered equality)
        { new Sample(new[] { "A", "B" }), new Sample(new[] { "B", "A" }), false },
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

        namespace Generator.Equals.Tests.Classes;

        [Equatable]
        public partial class FieldEqualitySample
        {
            public FieldEqualitySample(string[] addresses)
            {
                _addresses = addresses;
            }

            [OrderedEquality] readonly string[] _addresses;
        }
        """;
}
