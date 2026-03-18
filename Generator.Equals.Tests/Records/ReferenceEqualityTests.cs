using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [ReferenceEquality] attribute on record property.
/// </summary>
public partial class ReferenceEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample([property: ReferenceEquality] string Name);

    static readonly string SharedName = "Dave";

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same reference
        { new Sample(SharedName), new Sample(SharedName), true },
        // Different references with same value (reference equality, so not equal)
        // Note: string literals are interned, so we need to create new strings
        { new Sample(new string("Dave".ToCharArray())), new Sample(new string("Dave".ToCharArray())), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void DifferentReferences_SameValue_ReportsInequality()
    {
        var nameA = new string("Dave".ToCharArray());
        var nameB = new string("Dave".ToCharArray());
        var a = new Sample(nameA);
        var b = new Sample(nameB);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(nameA, nameB, Prop("Name")) });
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record ReferenceEqualitySample([property: ReferenceEquality] string Name);
                                """;
}
