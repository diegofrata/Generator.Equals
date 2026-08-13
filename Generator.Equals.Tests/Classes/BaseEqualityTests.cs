using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for inheritance with [Equatable] attribute.
/// Verifies that derived classes properly include base class properties in equality.
/// </summary>
public partial class BaseEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Person
    {
        public Person(int age)
        {
            Age = age;
        }

        public int Age { get; }
    }

    [Equatable]
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
        EqualityAssert.Verify(a, b, expected);

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
        EqualityAssert.Verify(a, b, expected);

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

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class BaseEqualityPerson
                                {
                                    public BaseEqualityPerson(int age)
                                    {
                                        Age = age;
                                    }

                                    public int Age { get; }
                                }

                                [Equatable]
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
