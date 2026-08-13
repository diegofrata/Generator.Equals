using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for C# 12 primary constructor support.
/// Primary constructor parameters that are used in the class become captured fields.
/// </summary>
public partial class PrimaryConstructorTests : SnapshotTestBase
{
    // Primary constructor with parameters used as fields
    [Equatable]
    public partial class Sample(string name, int age)
    {
        // These are captured as compiler-generated fields
        public string GetName() => name;
        public int GetAge() => age;

        // This is an explicit property that SHOULD be compared
        public string Title { get; set; } = "";
    }

    // Primary constructor with explicit property declarations
    [Equatable]
    public partial class SampleWithProperties(string name, int age)
    {
        // Explicitly declared properties from primary constructor
        public string Name { get; } = name;
        public int Age { get; } = age;
    }

    public static TheoryData<Sample, Sample, bool> SampleCases => new()
    {
        // Same Title (captured fields name/age are NOT compared - they're compiler-generated)
        { new Sample("Dave", 35) { Title = "Mr" }, new Sample("John", 40) { Title = "Mr" }, true },
        // Different Title
        { new Sample("Dave", 35) { Title = "Mr" }, new Sample("Dave", 35) { Title = "Dr" }, false },
    };

    [Theory]
    [MemberData(nameof(SampleCases))]
    public void SampleEquality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithProperties, SampleWithProperties, bool> SampleWithPropertiesCases => new()
    {
        // Same Name and Age
        { new SampleWithProperties("Dave", 35), new SampleWithProperties("Dave", 35), true },
        // Different Name
        { new SampleWithProperties("Dave", 35), new SampleWithProperties("John", 35), false },
        // Different Age
        { new SampleWithProperties("Dave", 35), new SampleWithProperties("Dave", 40), false },
    };

    [Theory]
    [MemberData(nameof(SampleWithPropertiesCases))]
    public void SampleWithPropertiesEquality(SampleWithProperties a, SampleWithProperties b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    [Fact]
    public void Inequality_Sample_DifferentTitle()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave", 35) { Title = "Mr" },
            new Sample("Dave", 35) { Title = "Dr" }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Mr", "Dr", Prop("Title")) });
    }

    [Fact]
    public void Inequality_SampleWithProperties_DifferentName()
    {
        var diffs = SampleWithProperties.EqualityComparer.Default.Inequalities(
            new SampleWithProperties("Dave", 35),
            new SampleWithProperties("John", 35)).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Dave", "John", Prop("Name")) });
    }

    [Fact]
    public void Inequality_SampleWithProperties_DifferentAge()
    {
        var diffs = SampleWithProperties.EqualityComparer.Default.Inequalities(
            new SampleWithProperties("Dave", 35),
            new SampleWithProperties("Dave", 40)).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(35, 40, Prop("Age")) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class PrimaryCtorSample(string name, int age)
                                {
                                    public string GetName() => name;
                                    public int GetAge() => age;
                                    public string Title { get; set; } = "";
                                }

                                [Equatable]
                                public partial class PrimaryCtorSampleWithProperties(string name, int age)
                                {
                                    public string Name { get; } = name;
                                    public int Age { get; } = age;
                                }
                                """;
}
