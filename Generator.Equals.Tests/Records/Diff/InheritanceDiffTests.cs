using FluentAssertions;

namespace Generator.Equals.Tests.Records.Diff;

/// <summary>
/// Diff tests for record inheritance scenarios.
/// </summary>
public partial class InheritanceDiffTests
{
    static (string Path, object? Left, object? Right) Diff(string path, object? left, object? right)
        => (path, left, right);

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

        diffs.Should().BeEquivalentTo(new[] { Diff("Name", "Alice", "Bob") });
    }

    [Fact]
    public void Inheritance_DerivedDifference_ReportsPath()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Alice", "Sales");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Department", "Engineering", "Sales") });
    }

    [Fact]
    public void Inheritance_MultipleDifferences_ReportsAll()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Bob", "Sales");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Name", "Alice", "Bob"),
            Diff("Department", "Engineering", "Sales")
        });
    }

    [Fact]
    public void Inheritance_NullVsNonNull_ReportsEntireObject()
    {
        var a = new DerivedManager("Alice", "Engineering");

        var diffs = DerivedManager.EqualityComparer.Default.Diff(a, null).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("", a, null) });
    }
}
