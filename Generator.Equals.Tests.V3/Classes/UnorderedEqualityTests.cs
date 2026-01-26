using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Classes;

/// <summary>
/// Tests for [UnorderedEquality] on List properties.
/// Order should not matter when comparing collections.
/// </summary>
public partial class UnorderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        [UnorderedEquality] public List<int>? Properties { get; set; }
    }

    [Fact]
    public void SameContentsDifferentOrder_ShouldBeEqual()
    {
        var random = new Random(42);
        var items = Enumerable.Range(1, 1000).ToList();
        var shuffled = items.OrderBy(_ => random.NextDouble()).ToList();

        var a = new Sample { Properties = items };
        var b = new Sample { Properties = shuffled };

        EqualityAssert.Verify(a, b, true);
    }

    [Fact]
    public void DifferentContents_ShouldNotBeEqual()
    {
        var a = new Sample { Properties = Enumerable.Range(1, 1000).ToList() };
        var b = new Sample { Properties = Enumerable.Range(1, 1001).ToList() };

        EqualityAssert.Verify(a, b, false);
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same content
        { new Sample { Properties = new List<int> { 1, 2, 3 } }, new Sample { Properties = new List<int> { 1, 2, 3 } }, true },
        // Same content, different order (unordered equality)
        { new Sample { Properties = new List<int> { 1, 2, 3 } }, new Sample { Properties = new List<int> { 3, 2, 1 } }, true },
        // Different content
        { new Sample { Properties = new List<int> { 1, 2, 3 } }, new Sample { Properties = new List<int> { 1, 2, 4 } }, false },
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
        using System.Collections.Generic;
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Classes;

        [Equatable]
        public partial class UnorderedEqualitySample
        {
            [UnorderedEquality] public List<int>? Properties { get; set; }
        }
        """;
}
