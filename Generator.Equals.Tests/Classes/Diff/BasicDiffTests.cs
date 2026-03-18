using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Basic Diff tests for classes: null handling, simple properties, path handling, consistency.
/// </summary>
public partial class BasicDiffTests
{
    static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);
    static MemberPathSegment Idx(int i) => MemberPathSegment.Index(i);
    static MemberPathSegment DKey(object k) => MemberPathSegment.Key(k);

    static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);

    [Equatable]
    public partial class Sample
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    [Fact]
    public void Diff_SameObjects_ReturnsNoDifferences()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Alice", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_SameReference_ReturnsNoDifferences()
    {
        var a = new Sample { Name = "Alice", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, a).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_OneNull_ReturnsEntireObject()
    {
        var a = new Sample { Name = "Alice", Age = 30 };

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

    [Fact]
    public void Diff_ConsistentWithEquals_WhenEqual()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Alice", Age = 30 };

        var areEqual = Sample.EqualityComparer.Default.Equals(a, b);
        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        areEqual.Should().BeTrue();
        diffs.Should().BeEmpty("Diff should return no differences when Equals returns true");
    }

    [Fact]
    public void Diff_ConsistentWithEquals_WhenNotEqual()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 35 };

        var areEqual = Sample.EqualityComparer.Default.Equals(a, b);
        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        areEqual.Should().BeFalse();
        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Alice", "Bob", Prop("Name")),
            Ineq(30, 35, Prop("Age"))
        });
    }
}
