using FluentAssertions;

namespace Generator.Equals.Tests.Diff.Classes;

/// <summary>
/// Diff tests for inheritance scenarios: base classes with/without [Equatable].
/// </summary>
public partial class InheritanceDiffTests
{
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

        var diffs = Manager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Name");
    }

    [Fact]
    public void Inheritance_DerivedDifference_ReportsPath()
    {
        var a = new Manager { Name = "Alice", Department = "Engineering" };
        var b = new Manager { Name = "Alice", Department = "Sales" };

        var diffs = Manager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Department");
    }

    [Fact]
    public void Inheritance_MultipleDifferences_ReportsAll()
    {
        var a = new Manager { Name = "Alice", Department = "Engineering" };
        var b = new Manager { Name = "Bob", Department = "Sales" };

        var diffs = Manager.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "Name");
        diffs.Should().Contain(d => d.Path == "Department");
    }

    #endregion

    #region Non-Equatable Base Tests

    [Fact]
    public void InheritedFromNonEquatable_ReportsBaseProperty()
    {
        var a = new DerivedFromNonEquatable { BaseProp = "Base1", DerivedProp = "Derived" };
        var b = new DerivedFromNonEquatable { BaseProp = "Base2", DerivedProp = "Derived" };

        var diffs = DerivedFromNonEquatable.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("BaseProp");
    }

    [Fact]
    public void InheritedFromNonEquatable_ReportsDerivedProperty()
    {
        var a = new DerivedFromNonEquatable { BaseProp = "Base", DerivedProp = "Derived1" };
        var b = new DerivedFromNonEquatable { BaseProp = "Base", DerivedProp = "Derived2" };

        var diffs = DerivedFromNonEquatable.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("DerivedProp");
    }

    [Fact]
    public void InheritedFromNonEquatable_ReportsBothProperties()
    {
        var a = new DerivedFromNonEquatable { BaseProp = "Base1", DerivedProp = "Derived1" };
        var b = new DerivedFromNonEquatable { BaseProp = "Base2", DerivedProp = "Derived2" };

        var diffs = DerivedFromNonEquatable.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "BaseProp");
        diffs.Should().Contain(d => d.Path == "DerivedProp");
    }

    #endregion
}
