using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Classes;

/// <summary>
/// Tests for equality comparison with nullable properties.
/// </summary>
public partial class NullableEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        [OrderedEquality] public string[]? Addresses { get; set; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null should be equal
        { new Sample { Addresses = null }, new Sample { Addresses = null }, true },
        // Both empty should be equal
        { new Sample { Addresses = Array.Empty<string>() }, new Sample { Addresses = Array.Empty<string>() }, true },
        // Same content should be equal
        { new Sample { Addresses = new[] { "A" } }, new Sample { Addresses = new[] { "A" } }, true },
        // One null, one not - should not be equal
        { new Sample { Addresses = null }, new Sample { Addresses = new[] { "A" } }, false },
        // Different content should not be equal
        { new Sample { Addresses = new[] { "A" } }, new Sample { Addresses = new[] { "B" } }, false },
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
        public partial class NullableEqualitySample
        {
            [OrderedEquality] public string[]? Addresses { get; set; }
        }
        """;
}
