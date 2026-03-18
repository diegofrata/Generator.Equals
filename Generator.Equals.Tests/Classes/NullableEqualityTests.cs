using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

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
        { new Sample { Addresses = [] }, new Sample { Addresses = [] }, true },
        // Same content should be equal
        { new Sample { Addresses = ["A"] }, new Sample { Addresses = ["A"] }, true },
        // One null, one not - should not be equal
        { new Sample { Addresses = null }, new Sample { Addresses = ["A"] }, false },
        // Different content should not be equal
        { new Sample { Addresses = ["A"] }, new Sample { Addresses = ["B"] }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    [Fact]
    public void Inequality_NullVsValue()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample { Addresses = null }, new Sample { Addresses = ["A"] }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, "A", Prop("Addresses"), Idx(0)) });
    }

    [Fact]
    public void Inequality_DifferentContent()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample { Addresses = ["A"] }, new Sample { Addresses = ["B"] }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("A", "B", Prop("Addresses"), Idx(0)) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class NullableEqualitySample
                                {
                                    [OrderedEquality] public string[]? Addresses { get; set; }
                                }
                                """;
}
