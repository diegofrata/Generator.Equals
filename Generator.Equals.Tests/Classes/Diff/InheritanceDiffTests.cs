using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Diff tests for inheritance scenarios: base classes with/without [Equatable].
/// </summary>
public partial class InheritanceDiffTests
{
    static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);

    static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);

    [Equatable]
    public partial class Person
    {
        public string? Name { get; set; }
    }

    [Equatable]
    public partial class Manager : Person
    {
        public string? Department { get; set; }
    }

    // For testing inheritance from non-equatable base
    public class NonEquatableBase
    {
        public string? BaseProp { get; set; }
    }

    [Equatable]
    public partial class DerivedFromNonEquatable : NonEquatableBase
    {
        public string? DerivedProp { get; set; }
    }

    #region Base with [Equatable] Tests

    [Fact]
    public void Inheritance_BaseDifference_ReportsBasePath()
    {
        var a = new Manager { Name = "Alice", Department = "Engineering" };
        var b = new Manager { Name = "Bob", Department = "Engineering" };

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Name")) });
    }

    [Fact]
    public void Inheritance_DerivedDifference_ReportsPath()
    {
        var a = new Manager { Name = "Alice", Department = "Engineering" };
        var b = new Manager { Name = "Alice", Department = "Sales" };

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Engineering", "Sales", Prop("Department")) });
    }

    [Fact]
    public void Inheritance_MultipleDifferences_ReportsAll()
    {
        var a = new Manager { Name = "Alice", Department = "Engineering" };
        var b = new Manager { Name = "Bob", Department = "Sales" };

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Alice", "Bob", Prop("Name")),
            Ineq("Engineering", "Sales", Prop("Department"))
        });
    }

    [Fact]
    public void Inheritance_NullVsNonNull_ReportsEntireObject()
    {
        var a = new Manager { Name = "Alice", Department = "Engineering" };

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, null).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(a, null) });
    }

    #endregion

    #region Non-Equatable Base Tests

    [Fact]
    public void InheritedFromNonEquatable_ReportsBaseProperty()
    {
        var a = new DerivedFromNonEquatable { BaseProp = "Base1", DerivedProp = "Derived" };
        var b = new DerivedFromNonEquatable { BaseProp = "Base2", DerivedProp = "Derived" };

        var diffs = DerivedFromNonEquatable.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Base1", "Base2", Prop("BaseProp")) });
    }

    [Fact]
    public void InheritedFromNonEquatable_ReportsDerivedProperty()
    {
        var a = new DerivedFromNonEquatable { BaseProp = "Base", DerivedProp = "Derived1" };
        var b = new DerivedFromNonEquatable { BaseProp = "Base", DerivedProp = "Derived2" };

        var diffs = DerivedFromNonEquatable.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Derived1", "Derived2", Prop("DerivedProp")) });
    }

    [Fact]
    public void InheritedFromNonEquatable_ReportsBothProperties()
    {
        var a = new DerivedFromNonEquatable { BaseProp = "Base1", DerivedProp = "Derived1" };
        var b = new DerivedFromNonEquatable { BaseProp = "Base2", DerivedProp = "Derived2" };

        var diffs = DerivedFromNonEquatable.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Base1", "Base2", Prop("BaseProp")),
            Ineq("Derived1", "Derived2", Prop("DerivedProp"))
        });
    }

    #endregion
}
