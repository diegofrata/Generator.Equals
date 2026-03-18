using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [IgnoreEquality] attribute which excludes properties from equality comparison.
/// </summary>
public partial class IgnoreEqualityTests : SnapshotTestBase
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

        [IgnoreEquality]
        public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Age is ignored, so these should be equal (same Name)
        { new Sample("Dave", 35), new Sample("Dave", 85), true },
        // Different Name, so not equal
        { new Sample("Dave", 35), new Sample("John", 35), false },
        // Same values
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void DifferentName_ReportsNameInequality_IgnoresAge()
    {
        var a = new Sample("Dave", 35);
        var b = new Sample("John", 35);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Dave", "John", Prop("Name")) });
    }

    [Fact]
    public void DifferentAge_SameName_NoInequalities()
    {
        var a = new Sample("Dave", 35);
        var b = new Sample("Dave", 85);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class IgnoreEqualitySample
                                {
                                    public IgnoreEqualitySample(string name, int age)
                                    {
                                        Name = name;
                                        Age = age;
                                    }

                                    public string Name { get; }

                                    [IgnoreEquality]
                                    public int Age { get; }
                                }
                                """;
}
