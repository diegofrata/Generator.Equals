using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality in an inheritance hierarchy where intermediate class manually overrides Equals.
/// </summary>
public partial class OverridingEqualsTests : SnapshotTestBase
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

    public class Manager : Person, IEquatable<Manager>
    {
        public Manager(int age, string department) : base(age)
        {
            Department = department;
        }

        public string Department { get; }

        public bool Equals(Manager? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Department == other.Department;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Manager)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Department);
        }
    }

    [Equatable]
    public partial class SeniorManager : Manager
    {
        public SeniorManager(int age, string department, int shares) : base(age, department)
        {
            Shares = shares;
        }

        public int Shares { get; }
    }

    public static TheoryData<SeniorManager, SeniorManager, bool> EqualityCases => new()
    {
        // Same values
        { new SeniorManager(25, "IT", 1000), new SeniorManager(25, "IT", 1000), true },
        // Different Shares
        { new SeniorManager(25, "IT", 1000), new SeniorManager(25, "IT", 2000), false },
        // Different Department - NOT equal because base.Equals() is called since Manager overrides Equals
        // Manager.Equals compares Department
        { new SeniorManager(25, "IT", 1000), new SeniorManager(25, "Sales", 1000), false },
        // Different Age - NOT equal because base.Equals() eventually reaches Person.Equals
        // which compares Age
        { new SeniorManager(25, "IT", 1000), new SeniorManager(30, "IT", 1000), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(SeniorManager a, SeniorManager b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class OverridingEqualsPerson
                                {
                                    public OverridingEqualsPerson(int age)
                                    {
                                        Age = age;
                                    }

                                    public int Age { get; }
                                }

                                public class OverridingEqualsManager : OverridingEqualsPerson, IEquatable<OverridingEqualsManager>
                                {
                                    public OverridingEqualsManager(int age, string department) : base(age)
                                    {
                                        Department = department;
                                    }

                                    public string Department { get; }

                                    public bool Equals(OverridingEqualsManager? other)
                                    {
                                        if (ReferenceEquals(null, other)) return false;
                                        if (ReferenceEquals(this, other)) return true;
                                        return base.Equals(other) && Department == other.Department;
                                    }

                                    public override bool Equals(object? obj)
                                    {
                                        if (ReferenceEquals(null, obj)) return false;
                                        if (ReferenceEquals(this, obj)) return true;
                                        if (obj.GetType() != this.GetType()) return false;
                                        return Equals((OverridingEqualsManager)obj);
                                    }

                                    public override int GetHashCode()
                                    {
                                        return HashCode.Combine(base.GetHashCode(), Department);
                                    }
                                }

                                [Equatable]
                                public partial class OverridingEqualsSeniorManager : OverridingEqualsManager
                                {
                                    public OverridingEqualsSeniorManager(int age, string department, int shares) : base(age, department)
                                    {
                                        Shares = shares;
                                    }

                                    public int Shares { get; }
                                }
                                """;
}
