using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] record with nullable array property.
/// </summary>
public partial class NullableEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample
    {
        [OrderedEquality] public string[]? Addresses { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Addresses = null }, new Sample { Addresses = null }, true },
        // Same content
        { new Sample { Addresses = ["10 Some Street"] }, new Sample { Addresses = ["10 Some Street"] }, true },
        // One null, one not
        { new Sample { Addresses = null }, new Sample { Addresses = ["10 Some Street"] }, false },
        // Different content
        { new Sample { Addresses = ["10 Some Street"] }, new Sample { Addresses = ["11 Some Street"] }, false },
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
            new Sample { Addresses = null }, new Sample { Addresses = ["10 Some Street"] }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, "10 Some Street", Prop("Addresses"), Idx(0)) });
    }

    [Fact]
    public void Inequality_DifferentContent()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample { Addresses = ["10 Some Street"] }, new Sample { Addresses = ["11 Some Street"] }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("10 Some Street", "11 Some Street", Prop("Addresses"), Idx(0)) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record NullableEqualitySample
                                {
                                    [OrderedEquality] public string[]? Addresses { get; init; }
                                }
                                """;
}
