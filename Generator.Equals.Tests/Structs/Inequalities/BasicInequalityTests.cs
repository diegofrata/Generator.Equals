using FluentAssertions;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Structs.Inequalities;

/// <summary>
/// Basic inequality tests for structs: simple properties, path handling.
/// </summary>
public partial class BasicInequalityTests
{

    [Equatable]
    public partial struct Sample
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    [Fact]
    public void Diff_SameStructs_ReturnsNoDifferences()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Alice", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_DifferentName_ReturnsNameDifference()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Name")) });
    }

    [Fact]
    public void Diff_DifferentAge_ReturnsAgeDifference()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Alice", Age = 35 };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(30, 35, Prop("Age")) });
    }

    [Fact]
    public void Diff_MultipleDifferences_ReturnsAllDifferences()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 35 };

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
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b, new MemberPath(new[] { Prop("Root") })).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Root"), Prop("Name")) });
    }

}
