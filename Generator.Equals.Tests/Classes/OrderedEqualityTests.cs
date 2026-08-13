using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [OrderedEquality] on array properties.
/// Order matters when comparing collections.
/// </summary>
public partial class OrderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string[] addresses)
        {
            Addresses = addresses;
        }

        [OrderedEquality] public string[] Addresses { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content, same order
        { new Sample(["10 Downing St"]), new Sample(["10 Downing St"]), true },
        // Different content
        { new Sample(["10 Downing St"]), new Sample(["Bricklane"]), false },
        // Same content, different order (ordered equality - order matters!)
        { new Sample(["A", "B"]), new Sample(["B", "A"]), false },
        // Multiple items, same order
        { new Sample(["A", "B", "C"]), new Sample(["A", "B", "C"]), true },
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
    public void Inequality_DifferentContent_ReportsDiffAtIndex()
    {
        var a = new Sample(["10 Downing St"]);
        var b = new Sample(["Bricklane"]);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("10 Downing St", "Bricklane", Prop("Addresses"), Idx(0)) });
    }

    [Fact]
    public void Inequality_DifferentOrder_ReportsDiffsAtEachSwappedIndex()
    {
        var a = new Sample(["A", "B"]);
        var b = new Sample(["B", "A"]);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("A", "B", Prop("Addresses"), Idx(0)),
            Ineq("B", "A", Prop("Addresses"), Idx(1))
        });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class OrderedEqualitySample
                                {
                                    public OrderedEqualitySample(string[] addresses)
                                    {
                                        Addresses = addresses;
                                    }

                                    [OrderedEquality] public string[] Addresses { get; }
                                }
                                """;
}
