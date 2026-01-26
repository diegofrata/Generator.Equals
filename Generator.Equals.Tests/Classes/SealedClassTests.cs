using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality on sealed classes.
/// </summary>
public partial class SealedClassTests : SnapshotTestBase
{
    [Equatable]
    public sealed partial class Sample
    {
        public Sample(string[] addresses)
        {
            Addresses = addresses;
        }

        [OrderedEquality] public string[] Addresses { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content should be equal
        { new Sample(new[] { "10 Downing St" }), new Sample(new[] { "10 Downing St" }), true },
        // Different content should not be equal
        { new Sample(new[] { "10 Downing St" }), new Sample(new[] { "Bricklane" }), false },
        // Multiple items, same order
        { new Sample(new[] { "A", "B" }), new Sample(new[] { "A", "B" }), true },
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
        public sealed partial class SealedClassSample
        {
            public SealedClassSample(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }
        """;
}
