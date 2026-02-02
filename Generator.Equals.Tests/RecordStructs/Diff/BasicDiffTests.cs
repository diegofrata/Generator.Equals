using FluentAssertions;

namespace Generator.Equals.Tests.RecordStructs.Diff;

/// <summary>
/// Basic Diff tests for record structs: simple properties, path handling, consistency.
/// </summary>
public partial class BasicDiffTests
{
    static (string Path, object? Left, object? Right) Diff(string path, object? left, object? right)
        => (path, left, right);

    [Equatable]
    public partial record struct Sample(string? Name, int Age);

    [Fact]
    public void Diff_SameRecordStructs_ReturnsNoDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_DifferentName_ReturnsNameDifference()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Name", "Alice", "Bob") });
    }

    [Fact]
    public void Diff_DifferentAge_ReturnsAgeDifference()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 35);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Age", 30, 35) });
    }

    [Fact]
    public void Diff_MultipleDifferences_ReturnsAllDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 35);

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
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b, "Root").ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Root.Name", "Alice", "Bob") });
    }

    [Fact]
    public void Diff_ConsistentWithEquals_WhenEqual()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 30);

        var areEqual = Sample.EqualityComparer.Default.Equals(a, b);
        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        areEqual.Should().BeTrue();
        diffs.Should().BeEmpty("Diff should return no differences when Equals returns true");
    }

    [Fact]
    public void Diff_ConsistentWithEquals_WhenNotEqual()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 35);

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
