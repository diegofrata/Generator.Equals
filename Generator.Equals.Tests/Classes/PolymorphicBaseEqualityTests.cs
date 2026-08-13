using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Regression tests for https://github.com/diegofrata/Generator.Equals/issues/77
///
/// When two instances of a derived class are compared through a reference typed as their
/// [Equatable] base class, the comparison must consider the members declared on the derived
/// type (polymorphic equality), matching the v3.x behavior. The base class's generated
/// <c>==</c> operator delegates to the nested <c>EqualityComparer</c>, which must dispatch
/// virtually to the runtime type's <c>Equals</c> rather than binding to the non-virtual
/// <c>Equals(TBase?)</c> on the base type (which would only compare <c>GetType()</c>).
/// </summary>
public partial class PolymorphicBaseEqualityTests : SnapshotTestBase
{
    [Equatable]
    public abstract partial class Animal
    {
    }

    [Equatable]
    public partial class Dog : Animal
    {
        public string Name { get; set; } = "";
    }

    [Equatable]
    public partial class Cat : Animal
    {
        public string Name { get; set; } = "";
    }

    // Three-level hierarchy: Shape <- Polygon <- Rectangle
    [Equatable]
    public abstract partial class Shape
    {
    }

    [Equatable]
    public partial class Polygon : Shape
    {
        public int Sides { get; set; }
    }

    [Equatable]
    public partial class Rectangle : Polygon
    {
        public int Width { get; set; }
    }

    [Fact]
    public void DerivedMembers_AreCompared_ThroughBaseReference()
    {
        Animal a = new Dog { Name = "Rex" };
        Animal b = new Dog { Name = "Fido" };

        (a == b).Should().BeFalse("Dog.Name differs when compared through an Animal reference");
        (a != b).Should().BeTrue();
        a.Equals(b).Should().BeFalse();
    }

    [Fact]
    public void EqualDerivedInstances_AreEqual_ThroughBaseReference()
    {
        Animal a = new Dog { Name = "Rex" };
        Animal b = new Dog { Name = "Rex" };

        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void DifferentDerivedTypes_AreNotEqual_AndSymmetric()
    {
        Animal dog = new Dog { Name = "Rex" };
        Animal cat = new Cat { Name = "Rex" };

        (dog == cat).Should().BeFalse();
        (cat == dog).Should().BeFalse("equality must be symmetric");
    }

    [Fact]
    public void HashCode_IsConsistentWithEquals_ThroughBaseReference()
    {
        // Before the fix, the nested EqualityComparer compared via the non-virtual base
        // Equals (ignoring derived members) while GetHashCode dispatched virtually, so two
        // unequal Dogs could report Equals == true with differing hash codes. Confirm the
        // comparer now honours the Equals/GetHashCode contract through a base reference.
        var cmp = Animal.EqualityComparer.Default;
        Animal a = new Dog { Name = "Rex" };
        Animal b = new Dog { Name = "Rex" };
        Animal c = new Dog { Name = "Fido" };

        cmp.Equals(a, b).Should().BeTrue();
        cmp.GetHashCode(a).Should().Be(cmp.GetHashCode(b));
        cmp.Equals(a, c).Should().BeFalse();
    }

    [Fact]
    public void DeepHierarchy_ComparesAllLevels_ThroughRootReference()
    {
        Shape a = new Rectangle { Sides = 4, Width = 10 };
        Shape differByWidth = new Rectangle { Sides = 4, Width = 20 };
        Shape differBySides = new Rectangle { Sides = 5, Width = 10 };
        Shape equal = new Rectangle { Sides = 4, Width = 10 };

        (a == differByWidth).Should().BeFalse("Rectangle.Width differs");
        (a == differBySides).Should().BeFalse("Polygon.Sides differs");
        (a == equal).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public abstract partial class PolymorphicAnimal
                                {
                                }

                                [Equatable]
                                public partial class PolymorphicDog : PolymorphicAnimal
                                {
                                    public string Name { get; set; } = "";
                                }
                                """;
}
