using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable(Explicit = true)] record mode where only marked properties are compared.
/// </summary>
public partial class ExplicitModeTests : SnapshotTestBase
{
    [Equatable(Explicit = true)]
    public partial record Sample(string Name, [property: DefaultEquality] int Age);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Age (Name is ignored in explicit mode)
        { new Sample("Dave", 35), new Sample("John", 35), true },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
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
    public void Inequality_DifferentAge()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave", 35), new Sample("Dave", 40)).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(35, 40, Prop("Age")) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable(Explicit = true)]
                                public partial record ExplicitModeSample(string Name, [property: DefaultEquality] int Age);
                                """;
}
