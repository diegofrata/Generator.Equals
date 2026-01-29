using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [UnorderedEquality] attribute on Dictionary property.
/// </summary>
public partial class DictionaryEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample
    {
        [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same content
        { new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } }, new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } }, true },
        // Same content, different insertion order
        { new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } }, new Sample { Properties = new() { ["b"] = 2, ["a"] = 1 } }, true },
        // Different content
        { new Sample { Properties = new() { ["a"] = 1 } }, new Sample { Properties = new() { ["a"] = 2 } }, false },
        // One null, one not
        { new Sample { Properties = null }, new Sample { Properties = new() { ["a"] = 1 } }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record DictionaryEqualitySample
                                {
                                    [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
                                }
                                """;
}
