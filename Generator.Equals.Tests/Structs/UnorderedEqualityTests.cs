using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [UnorderedEquality] attribute on List property.
/// </summary>
public partial class UnorderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        [UnorderedEquality] public List<int>? Properties { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same content
        { new Sample { Properties = [1, 2, 3] }, new Sample { Properties = [1, 2, 3] }, true },
        // Same content, different order (unordered equality - order doesn't matter!)
        { new Sample { Properties = [1, 2, 3] }, new Sample { Properties = [3, 2, 1] }, true },
        // Different content
        { new Sample { Properties = [1, 2, 3] }, new Sample { Properties = [1, 2, 4] }, false },
        // One null, one not
        { new Sample { Properties = null }, new Sample { Properties = [1] }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct UnorderedEqualitySample
                                {
                                    [UnorderedEquality] public List<int>? Properties { get; init; }
                                }
                                """;
}
