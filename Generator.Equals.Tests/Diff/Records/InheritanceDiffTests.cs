using FluentAssertions;

namespace Generator.Equals.Tests.Diff.Records;

/// <summary>
/// Diff tests for record inheritance scenarios.
/// </summary>
public partial class InheritanceDiffTests
{
    [Equatable]
    public partial record BasePerson(string? Name);

    [Equatable]
    public partial record DerivedManager(string? Name, string? Department) : BasePerson(Name);

    [Fact]
    public void Inheritance_BaseDifference_ReportsPath()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Bob", "Engineering");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Name");
    }

    [Fact]
    public void Inheritance_DerivedDifference_ReportsPath()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Alice", "Sales");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Department");
    }

    [Fact]
    public void Inheritance_MultipleDifferences_ReportsAll()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Bob", "Sales");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "Name");
        diffs.Should().Contain(d => d.Path == "Department");
    }

    [Fact]
    public void Inheritance_NullVsNonNull_ReportsEntireObject()
    {
        var a = new DerivedManager("Alice", "Engineering");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, null).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("");
        diffs[0].Left.Should().Be(a);
        diffs[0].Right.Should().BeNull();
    }
}
