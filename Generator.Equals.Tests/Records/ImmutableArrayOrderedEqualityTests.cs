using System.Collections.Immutable;
using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [OrderedEquality] on ImmutableArray properties in records.
/// </summary>
public partial class ImmutableArrayOrderedEqualityTests
{
    [Equatable]
    public partial record Sample(
        [property: OrderedEquality] ImmutableArray<string> Items
    );

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        { new Sample(["A", "B"]), new Sample(["A", "B"]), true },
        { new Sample(["A"]), new Sample(["B"]), false },
        { new Sample(["A", "B"]), new Sample(["B", "A"]), false },
        { new Sample(["A", "B"]), new Sample(["A"]), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void Inequality_DifferentContent()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample(["A"]), new Sample(["B"])).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("A", "B", Prop("Items"), Idx(0)) });
    }

    [Fact]
    public void Inequality_DifferentOrder()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample(["A", "B"]), new Sample(["B", "A"])).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("A", "B", Prop("Items"), Idx(0)),
            Ineq("B", "A", Prop("Items"), Idx(1))
        });
    }

    [Fact]
    public void Inequality_DifferentLength()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample(["A", "B"]), new Sample(["A"])).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("B", null, Prop("Items"), Idx(1)) });
    }
}
