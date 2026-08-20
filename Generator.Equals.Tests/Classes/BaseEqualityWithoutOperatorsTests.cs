using System.Reflection;
using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for inheritance with [Equatable] attribute when equality operators are disabled.
/// Verifies that derived classes properly include base class properties in equality
/// without generating == and != operators.
/// </summary>
public partial class BaseEqualityWithoutOperatorsTests : SnapshotTestBase
{
    [Equatable(GenerateClassEqualityOperators = false)]
    public partial class Person
    {
        public Person(int age)
        {
            Age = age;
        }

        public int Age { get; }
    }

    [Equatable(GenerateClassEqualityOperators = false)]
    public partial class Manager : Person
    {
        public Manager(int age, string department) : base(age)
        {
            Department = department;
        }

        public string Department { get; }
    }

    public static TheoryData<Manager, Manager, bool> ManagerEqualityCases => new()
    {
        // Same Age and Department
        { new Manager(25, "IT"), new Manager(25, "IT"), true },
        // Same Age, different Department
        { new Manager(25, "IT"), new Manager(25, "Sales"), false },
        // Different Age, same Department
        { new Manager(25, "IT"), new Manager(30, "IT"), false },
        // Different Age and Department
        { new Manager(25, "IT"), new Manager(30, "Sales"), false },
    };

    [Theory]
    [MemberData(nameof(ManagerEqualityCases))]
    public void ManagerEquality(Manager a, Manager b, bool expected) =>
        EqualityAssert.VerifyWithoutOperators(a, b, expected);

    [Fact]
    public void ManagerOperatorsUseReferenceEquality()
    {
        var a = new Manager(25, "IT");
        var b = new Manager(25, "IT");

        // Structurally equal, but == is object's reference comparison because no operator was generated.
        a.Equals(b).Should().BeTrue();
        (a == b).Should().BeFalse();
        (a != b).Should().BeTrue();

        // Same reference still compares equal, and so do two nulls.
        var alias = a;
        (alias == a).Should().BeTrue();
        (alias != a).Should().BeFalse();
        ((Manager?)null == null).Should().BeTrue();
        (a == null).Should().BeFalse();
        (null == a).Should().BeFalse();
    }

    public static TheoryData<Person, Person, bool> PersonEqualityCases => new()
    {
        // Same Age
        { new Person(25), new Person(25), true },
        // Different Age
        { new Person(25), new Person(30), false },
    };

    [Theory]
    [MemberData(nameof(PersonEqualityCases))]
    public void PersonEquality(Person a, Person b, bool expected) =>
        EqualityAssert.VerifyWithoutOperators(a, b, expected);

    [Fact]
    public void PersonInequality_DifferentAge()
    {
        var a = new Person(25);
        var b = new Person(30);

        var diffs = Person.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(25, 30, Prop("Age")) });
    }

    [Fact]
    public void ManagerInequality_DifferentDepartment()
    {
        var a = new Manager(25, "IT");
        var b = new Manager(25, "Sales");

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("IT", "Sales", Prop("Department")) });
    }

    [Fact]
    public void ManagerInequality_DifferentAge()
    {
        var a = new Manager(25, "IT");
        var b = new Manager(30, "IT");

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(25, 30, Prop("Age")) });
    }

    [Fact]
    public void ManagerInequality_DifferentAgeAndDepartment()
    {
        var a = new Manager(25, "IT");
        var b = new Manager(30, "Sales");

        var diffs = Manager.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(25, 30, Prop("Age")),
            Ineq("IT", "Sales", Prop("Department"))
        });
    }

    [Equatable]
    public partial class OperatorBase
    {
        public OperatorBase(int age)
        {
            Age = age;
        }

        public int Age { get; }
    }

    // The opt-out is ineffective here: C# overload resolution also considers OperatorBase's operators
    // for OperatorDerived operands. GE012 warns about exactly this, so the diagnostic is expected.
#pragma warning disable GE012
    [Equatable(GenerateClassEqualityOperators = false)]
    public partial class OperatorDerived : OperatorBase
    {
        public OperatorDerived(int age, string department) : base(age)
        {
            Department = department;
        }

        public string Department { get; }
    }
#pragma warning restore GE012

    [Fact]
    public void OptingOutOnDerivedDoesNotHideBaseOperators()
    {
        var a = new OperatorDerived(25, "IT");
        var b = new OperatorDerived(25, "IT");

        typeof(OperatorDerived)
            .GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Should().BeNull("the derived class opted out of generated operators");

        // ...but OperatorBase.op_Equality still binds, and it dispatches virtually into the derived
        // Equals, so == remains structural. This is what GE012 exists to warn about.
        (a == b).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable(GenerateClassEqualityOperators = false)]
                                public partial class BaseEqualityPerson
                                {
                                    public BaseEqualityPerson(int age)
                                    {
                                        Age = age;
                                    }

                                    public int Age { get; }
                                }

                                [Equatable(GenerateClassEqualityOperators = false)]
                                public partial class BaseEqualityManager : BaseEqualityPerson
                                {
                                    public BaseEqualityManager(int age, string department) : base(age)
                                    {
                                        Department = department;
                                    }

                                    public string Department { get; }
                                }
                                """;
}