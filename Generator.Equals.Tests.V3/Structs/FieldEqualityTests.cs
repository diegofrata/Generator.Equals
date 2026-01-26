using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Structs;

/// <summary>
/// Tests for [Equatable] struct with field equality.
/// </summary>
public partial class FieldEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        public Sample(string[] addresses)
        {
            _addresses = addresses;
        }

        [OrderedEquality] readonly string[] _addresses;
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content
        { new Sample(new[] { "10 Some Street" }), new Sample(new[] { "10 Some Street" }), true },
        // Different content
        { new Sample(new[] { "10 Some Street" }), new Sample(new[] { "11 Some Street" }), false },
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
        public partial struct FieldEqualitySample
        {
            public FieldEqualitySample(string[] addresses)
            {
                _addresses = addresses;
            }

            [OrderedEquality] readonly string[] _addresses;
        }
        """;
}
