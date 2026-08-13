using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for nested [Equatable] records (deep equality).
/// </summary>
public partial class DeepEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample(Person Person);

    [Equatable]
    public partial record Person(int Age);

    [Equatable]
    public partial record Manager(int Age, string Department) : Person(Age);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same nested Person
        { new Sample(new Person(25)), new Sample(new Person(25)), true },
        // Different Age
        { new Sample(new Person(25)), new Sample(new Person(30)), false },
        // Same Manager
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(25, "IT")), true },
        // Different Manager Department
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(25, "Sales")), false },
        // Person vs Manager with same Age (different types)
        { new Sample(new Person(25)), new Sample(new Manager(25, "IT")), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void SampleInequality_DifferentAge()
    {
        var a = new Sample(new Person(25));
        var b = new Sample(new Person(30));

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(a.Person, b.Person, Prop("Person"))
        });
    }

    [Fact]
    public void SampleInequality_DifferentManagerDepartment()
    {
        var a = new Sample(new Manager(25, "IT"));
        var b = new Sample(new Manager(25, "Sales"));

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(a.Person, b.Person, Prop("Person"))
        });
    }

    [Fact]
    public void SampleInequality_PersonVsManager()
    {
        var a = new Sample(new Person(25));
        var b = new Sample(new Manager(25, "IT"));

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(a.Person, b.Person, Prop("Person"))
        });
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record DeepEqualitySample(DeepEqualityPerson Person);

                                [Equatable]
                                public partial record DeepEqualityPerson(int Age);

                                [Equatable]
                                public partial record DeepEqualityManager(int Age, string Department) : DeepEqualityPerson(Age);
                                """;
}
