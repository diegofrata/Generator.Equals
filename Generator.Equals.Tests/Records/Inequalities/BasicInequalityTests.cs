using FluentAssertions;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records.Inequalities;

/// <summary>
/// Basic inequality tests for records: null handling, simple properties, path handling.
/// </summary>
public partial class BasicInequalityTests
{

    [Equatable]
    public partial record Sample(string? Name, int Age);

    [Fact]
    public void Diff_SameRecords_ReturnsNoDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_SameReference_ReturnsNoDifferences()
    {
        var a = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, a).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_OneNull_ReturnsEntireObject()
    {
        var a = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, null).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(a, null) });
    }

    [Fact]
    public void Diff_BothNull_ReturnsNoDifferences()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(null, null).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_DifferentName_ReturnsNameDifference()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 30);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Name")) });
    }

    [Fact]
    public void Diff_DifferentAge_ReturnsAgeDifference()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 35);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(30, 35, Prop("Age")) });
    }

    [Fact]
    public void Diff_MultipleDifferences_ReturnsAllDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 35);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Alice", "Bob", Prop("Name")),
            Ineq(30, 35, Prop("Age"))
        });
    }

    [Fact]
    public void Diff_WithBasePath_PrependsPath()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 30);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b, new MemberPath(new[] { Prop("Root") })).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Root"), Prop("Name")) });
    }

}
