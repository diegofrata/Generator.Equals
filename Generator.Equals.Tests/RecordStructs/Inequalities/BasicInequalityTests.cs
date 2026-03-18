using FluentAssertions;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.RecordStructs.Inequalities;

/// <summary>
/// Basic inequality tests for record structs: simple properties, path handling.
/// </summary>
public partial class BasicInequalityTests
{

    [Equatable]
    public partial record struct Sample(string? Name, int Age);

    [Fact]
    public void Diff_SameRecordStructs_ReturnsNoDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

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
