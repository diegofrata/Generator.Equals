using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [SetEquality] attribute on List property.
/// Set equality treats the collection as a set (ignores duplicates and order).
/// </summary>
public partial class SetEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        [SetEquality] public List<int>? Properties { get; set; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same content
        { new Sample { Properties = new() { 1, 2, 3 } }, new Sample { Properties = new() { 1, 2, 3 } }, true },
        // Same content, different order
        { new Sample { Properties = new() { 1, 2, 3 } }, new Sample { Properties = new() { 3, 2, 1 } }, true },
        // Same unique values with duplicates
        { new Sample { Properties = new() { 1, 2, 2 } }, new Sample { Properties = new() { 1, 1, 2 } }, true },
        // Different content
        { new Sample { Properties = new() { 1, 2, 3 } }, new Sample { Properties = new() { 1, 2, 4 } }, false },
        // One null, one not
        { new Sample { Properties = null }, new Sample { Properties = new() { 1 } }, false },
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
                                public partial struct SetEqualitySample
                                {
                                    [SetEquality] public List<int>? Properties { get; set; }
                                }
                                """;
}
