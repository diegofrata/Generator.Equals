using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [IgnoreEquality] attribute on record struct properties.
/// </summary>
public partial class IgnoreEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct Sample(string Name, [property: IgnoreEquality] int Age);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Name, different Age (Age is ignored)
        { new Sample("Dave", 35), new Sample("Dave", 85), true },
        // Different Name (not ignored)
        { new Sample("Dave", 35), new Sample("John", 35), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

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

                                namespace Generator.Equals.Tests.RecordStructs;

                                [Equatable]
                                public partial record struct IgnoreEqualitySample(string Name, [property: IgnoreEquality] int Age);
                                """;
}
