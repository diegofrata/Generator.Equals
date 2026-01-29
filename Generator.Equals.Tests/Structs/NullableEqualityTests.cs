using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [Equatable] struct with nullable array property.
/// </summary>
public partial class NullableEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        [OrderedEquality] public string[]? Addresses { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Addresses = null }, new Sample { Addresses = null }, true },
        // Same content
        { new Sample { Addresses = new[] { "10 Some Street" } }, new Sample { Addresses = new[] { "10 Some Street" } }, true },
        // One null, one not
        { new Sample { Addresses = null }, new Sample { Addresses = new[] { "10 Some Street" } }, false },
        // Different content
        { new Sample { Addresses = new[] { "10 Some Street" } }, new Sample { Addresses = new[] { "11 Some Street" } }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct NullableEqualitySample
                                {
                                    [OrderedEquality] public string[]? Addresses { get; init; }
                                }
                                """;
}
