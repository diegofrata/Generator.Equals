using FluentAssertions;

namespace Generator.Equals.Tests.Records.Diff;

/// <summary>
/// Diff tests for record inheritance scenarios.
/// </summary>
public partial class InheritanceDiffTests
{
    static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);

    static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);

    [Equatable]
    public partial record BasePerson(string? Name);

    [Equatable]
    public partial record DerivedManager(string? Name, string? Department) : BasePerson(Name);

    [Fact]
    public void Inheritance_BaseDifference_ReportsPath()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Bob", "Engineering");

        var diffs = DerivedManager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Name")) });
    }

    [Fact]
    public void Inheritance_DerivedDifference_ReportsPath()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Alice", "Sales");

        var diffs = DerivedManager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Engineering", "Sales", Prop("Department")) });
    }

    [Fact]
    public void Inheritance_MultipleDifferences_ReportsAll()
    {
        var a = new DerivedManager("Alice", "Engineering");
        var b = new DerivedManager("Bob", "Sales");

        var diffs = DerivedManager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Alice", "Bob", Prop("Name")),
            Ineq("Engineering", "Sales", Prop("Department"))
        });
    }

    [Fact]
    public void Inheritance_NullVsNonNull_ReportsEntireObject()
    {
        var a = new DerivedManager("Alice", "Engineering");

        var diffs = DerivedManager.EqualityComparer.Default.Inequalities(a, null).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(a, null) });
    }
}
