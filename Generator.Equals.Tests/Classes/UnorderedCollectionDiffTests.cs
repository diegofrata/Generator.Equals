using System.Collections.Immutable;
using FluentAssertions;

namespace Generator.Equals.Tests.Classes;

public partial class UnorderedCollectionDiffTests
{
    [Equatable]
    public partial class Bag
    {
        [UnorderedEquality] public int[]? Values { get; set; }
    }

    [Equatable]
    public partial record RecordBag([property: UnorderedEquality] int[] Values);

    [Equatable]
    public partial struct StructBag
    {
        [UnorderedEquality] public int[] Values { get; set; }
    }

    [Equatable]
    public partial record struct RecordStructBag([property: UnorderedEquality] int[] Values);

    [Equatable]
    public partial class CustomBag
    {
        [UnorderedEquality(StringComparison.OrdinalIgnoreCase)] public string[] Values { get; set; } = [];
    }

    [Equatable]
    public partial class ImmutableBag
    {
        [UnorderedEquality] public ImmutableArray<int> Values { get; set; }
    }

    static void VerifyMultiplicity(IEnumerable<Inequality> differences)
    {
        var diffs = differences.ToList();
        diffs.Select(x => x.Path.ToString()).Should().Equal("Values[-]", "Values[+]");
        diffs[0].Left.Should().Be(1);
        diffs[1].Right.Should().Be(2);
    }

    [Fact]
    public void MultiplicityChanges_ShouldReportRemovedAndAddedOccurrences()
    {
        VerifyMultiplicity(Bag.EqualityComparer.Default.Inequalities(
            new Bag { Values = [1, 1, 2] }, new Bag { Values = [1, 2, 2] }));
        VerifyMultiplicity(RecordBag.EqualityComparer.Default.Inequalities(
            new RecordBag([1, 1, 2]), new RecordBag([1, 2, 2])));
        VerifyMultiplicity(StructBag.EqualityComparer.Default.Inequalities(
            new StructBag { Values = [1, 1, 2] }, new StructBag { Values = [1, 2, 2] }));
        VerifyMultiplicity(RecordStructBag.EqualityComparer.Default.Inequalities(
            new RecordStructBag([1, 1, 2]), new RecordStructBag([1, 2, 2])));
        VerifyMultiplicity(ImmutableBag.EqualityComparer.Default.Inequalities(
            new ImmutableBag { Values = ImmutableArray.Create(1, 1, 2) },
            new ImmutableBag { Values = ImmutableArray.Create(1, 2, 2) }));
    }

    [Fact]
    public void CustomComparer_ShouldMatchUnchangedOccurrences()
    {
        var a = new CustomBag { Values = ["A", "b"] };
        var b = new CustomBag { Values = ["a", "c"] };
        var diffs = CustomBag.EqualityComparer.Default.Inequalities(a, b).ToList();
        diffs.Should().HaveCount(2);
        diffs[0].Left.Should().Be("b");
        diffs[1].Right.Should().Be("c");
    }

    [Fact]
    public void NullVersusEmpty_ShouldReportMemberDifference()
    {
        var a = new Bag { Values = null };
        var b = new Bag { Values = [] };
        a.Equals(b).Should().BeFalse();
        Bag.EqualityComparer.Default.Inequalities(a, b).Should().ContainSingle()
            .Which.Path.ToString().Should().Be("Values");
        Bag.EqualityComparer.Default.Inequalities(b, a).Should().ContainSingle();
    }
}
