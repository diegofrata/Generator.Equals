using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for basic primitive type equality comparison.
/// </summary>
public partial class PrimitiveEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values should be equal
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
        // Different values should not be equal
        { new Sample("Dave", 35), new Sample("Joe", 77), false },
        // Same name, different age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
        // Different name, same age
        { new Sample("Dave", 35), new Sample("Joe", 35), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    [Fact]
    public void Inequality_DifferentNameAndAge()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave", 35), new Sample("Joe", 77)).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Dave", "Joe", Prop("Name")),
            Ineq(35, 77, Prop("Age"))
        });
    }

    [Fact]
    public void Inequality_DifferentAge()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave", 35), new Sample("Dave", 40)).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(35, 40, Prop("Age")) });
    }

    [Fact]
    public void Inequality_DifferentName()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave", 35), new Sample("Joe", 35)).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Dave", "Joe", Prop("Name")) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class PrimitiveEqualitySample
                                {
                                    public PrimitiveEqualitySample(string name, int age)
                                    {
                                        Name = name;
                                        Age = age;
                                    }

                                    public string Name { get; }
                                    public int Age { get; }
                                }
                                """;
}
