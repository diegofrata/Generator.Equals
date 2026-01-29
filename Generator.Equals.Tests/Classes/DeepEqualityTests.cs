using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality with nested equatable objects.
/// Verifies that equality properly traverses object hierarchies.
/// </summary>
public partial class DeepEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(Person person)
        {
            Person = person;
        }

        public Person Person { get; }
    }

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

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same nested object values
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(25, "IT")), true },
        // Different nested Department
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(25, "Sales")), false },
        // Different nested Age
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(30, "IT")), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class DeepEqualitySample
                                {
                                    public DeepEqualitySample(DeepEqualityPerson person)
                                    {
                                        Person = person;
                                    }

                                    public DeepEqualityPerson Person { get; }
                                }

                                [Equatable]
                                public partial class DeepEqualityPerson
                                {
                                    public DeepEqualityPerson(int age)
                                    {
                                        Age = age;
                                    }

                                    public int Age { get; }
                                }

                                [Equatable]
                                public partial class DeepEqualityManager : DeepEqualityPerson
                                {
                                    public DeepEqualityManager(int age, string department) : base(age)
                                    {
                                        Department = department;
                                    }

                                    public string Department { get; }
                                }
                                """;
}
