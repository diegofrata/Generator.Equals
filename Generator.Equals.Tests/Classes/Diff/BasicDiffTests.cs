using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Basic Diff tests for classes: null handling, simple properties, path handling, consistency.
/// </summary>
public partial class BasicDiffTests
{
    static (string Path, object? Left, object? Right) Diff(string path, object? left, object? right)
        => (path, left, right);

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

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_SameReference_ReturnsNoDifferences()
    {
        var a = new Sample { Name = "Alice", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Diff(a, a).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_OneNull_ReturnsEntireObject()
    {
        var a = new Sample { Name = "Alice", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Diff(a, null).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("", a, null) });
    }

    [Fact]
    public void Diff_BothNull_ReturnsNoDifferences()
    {
        var diffs = Sample.EqualityComparer.Default.Diff(null, null).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_DifferentName_ReturnsNameDifference()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Name", "Alice", "Bob") });
    }

    [Fact]
    public void Diff_DifferentAge_ReturnsAgeDifference()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Alice", Age = 35 };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Age", 30, 35) });
    }

    [Fact]
    public void Diff_MultipleDifferences_ReturnsAllDifferences()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 35 };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Name", "Alice", "Bob"),
            Diff("Age", 30, 35)
        });
    }

    [Fact]
    public void Diff_WithBasePath_PrependsPath()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 30 };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b, "Root").ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Root.Name", "Alice", "Bob") });
    }

    [Fact]
    public void Diff_ConsistentWithEquals_WhenEqual()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Alice", Age = 30 };

        var areEqual = Sample.EqualityComparer.Default.Equals(a, b);
        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        areEqual.Should().BeTrue();
        diffs.Should().BeEmpty("Diff should return no differences when Equals returns true");
    }

    [Fact]
    public void Diff_ConsistentWithEquals_WhenNotEqual()
    {
        var a = new Sample { Name = "Alice", Age = 30 };
        var b = new Sample { Name = "Bob", Age = 35 };

        var areEqual = Sample.EqualityComparer.Default.Equals(a, b);
        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        areEqual.Should().BeFalse();
        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Name", "Alice", "Bob"),
            Diff("Age", 30, 35)
        });
    }
}
