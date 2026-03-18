using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [OrderedEquality] attribute on array property.
/// </summary>
public partial class OrderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample([property: OrderedEquality] string[] Addresses);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content
        { new Sample(["10 Some Street"]), new Sample(["10 Some Street"]), true },
        // Different content
        { new Sample(["10 Some Street"]), new Sample(["11 Some Street"]), false },
        // Same content, different order (ordered equality - order matters!)
        { new Sample(["A", "B"]), new Sample(["B", "A"]), false },
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
    public void Inequality_DifferentContent_ReportsDiffAtIndex()
    {
        var a = new Sample(["10 Some Street"]);
        var b = new Sample(["11 Some Street"]);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("10 Some Street", "11 Some Street", Prop("Addresses"), Idx(0)) });
    }

    [Fact]
    public void Inequality_DifferentOrder_ReportsDiffsAtEachSwappedIndex()
    {
        var a = new Sample(["A", "B"]);
        var b = new Sample(["B", "A"]);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("A", "B", Prop("Addresses"), Idx(0)),
            Ineq("B", "A", Prop("Addresses"), Idx(1))
        });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record OrderedEqualitySample([property: OrderedEquality] string[] Addresses);
                                """;
}
