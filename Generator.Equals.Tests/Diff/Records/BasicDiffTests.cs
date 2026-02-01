using FluentAssertions;

namespace Generator.Equals.Tests.Diff.Records;

/// <summary>
/// Basic Diff tests for records: null handling, simple properties, path handling, consistency.
/// </summary>
public partial class BasicDiffTests
{
    [Equatable]
    public partial record Sample(string? Name, int Age);

    [Fact]
    public void Diff_SameRecords_ReturnsNoDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_SameReference_ReturnsNoDifferences()
    {
        var a = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, a).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Diff_OneNull_ReturnsEntireObject()
    {
        var a = new Sample("Alice", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, null).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("");
        diffs[0].Left.Should().Be(a);
        diffs[0].Right.Should().BeNull();
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
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Name");
        diffs[0].Left.Should().Be("Alice");
        diffs[0].Right.Should().Be("Bob");
    }

    [Fact]
    public void Diff_DifferentAge_ReturnsAgeDifference()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Alice", 35);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Age");
        diffs[0].Left.Should().Be(30);
        diffs[0].Right.Should().Be(35);
    }

    [Fact]
    public void Diff_MultipleDifferences_ReturnsAllDifferences()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 35);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "Name");
        diffs.Should().Contain(d => d.Path == "Age");
    }

    [Fact]
    public void Diff_WithBasePath_PrependsPath()
    {
        var a = new Sample("Alice", 30);
        var b = new Sample("Bob", 30);

        var diffs = Sample.EqualityComparer.Default.Diff(a, b, "Root").ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Root.Name");
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
        diffs.Should().NotBeEmpty("Diff should return differences when Equals returns false");
    }
}
