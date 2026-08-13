using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality comparison using fields (not just properties).
/// </summary>
public partial class FieldEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string[] addresses)
        {
            _addresses = addresses;
        }

        [OrderedEquality] readonly string[] _addresses;
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same array content should be equal
        { new Sample(["10 Downing St"]), new Sample(["10 Downing St"]), true },
        // Different array content should not be equal
        { new Sample(["10 Downing St"]), new Sample(["Bricklane"]), false },
        // Multiple items, same order
        { new Sample(["A", "B"]), new Sample(["A", "B"]), true },
        // Multiple items, different order (ordered equality)
        { new Sample(["A", "B"]), new Sample(["B", "A"]), false },
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
    public void Inequality_DifferentContent()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample(["10 Downing St"]), new Sample(["Bricklane"])).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("10 Downing St", "Bricklane", Fld("_addresses"), Idx(0)) });
    }

    [Fact]
    public void Inequality_DifferentOrder()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample(["A", "B"]), new Sample(["B", "A"])).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("A", "B", Fld("_addresses"), Idx(0)),
            Ineq("B", "A", Fld("_addresses"), Idx(1))
        });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class FieldEqualitySample
                                {
                                    public FieldEqualitySample(string[] addresses)
                                    {
                                        _addresses = addresses;
                                    }

                                    [OrderedEquality] readonly string[] _addresses;
                                }
                                """;
}
