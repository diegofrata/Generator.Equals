using FluentAssertions;

namespace Generator.Equals.Tests.Runtime;

public class UnorderedEqualityComparerTests
{
    readonly UnorderedEqualityComparer<int> _sut;
    readonly int[] _a;
    readonly int[] _b;

    public UnorderedEqualityComparerTests()
    {
        _sut = new UnorderedEqualityComparer<int>(
            new NegativeEqualityComparer()
        );

        _a = [1, 2, 3, 4, 5];
        _b = [5, -4, 3, -2, 1];
    }

    [Fact]
    public void Equals_Should_use_ValueComparer()
    {
        _sut.Equals(_a, _b).Should().BeTrue();
    }

    [Fact]
    public void Equals_Should_count_null_elements()
    {
        var comparer = UnorderedEqualityComparer<string?>.Default;
        string?[] a = [null, "a", null];
        string?[] b = ["a", null, null];
        string?[] different = ["a", null, "a"];

        comparer.Equals(a, b).Should().BeTrue();
        comparer.GetHashCode(a).Should().Be(comparer.GetHashCode(b));
        comparer.Equals(a, different).Should().BeFalse();
        comparer.Equals(different, a).Should().BeFalse();
    }

    [Fact]
    public void Equals_Should_distinguish_null_multiplicity_from_non_collections()
    {
        // Non-ICollection inputs skip the up-front length check, so multiplicity
        // must be enforced purely by the counting pass.
        var comparer = UnorderedEqualityComparer<string?>.Default;
        IEnumerable<string?> a = new[] { null, "a", null }.Select(x => x);
        IEnumerable<string?> moreNulls = new[] { null, "a", null, null }.Select(x => x);
        IEnumerable<string?> fewerNulls = new[] { null, "a" }.Select(x => x);

        comparer.Equals(a, moreNulls).Should().BeFalse();
        comparer.Equals(a, fewerNulls).Should().BeFalse();
    }

    [Fact]
    public void Equals_Should_use_custom_comparer_with_null_elements()
    {
        var comparer = new UnorderedEqualityComparer<string?>(StringComparer.OrdinalIgnoreCase);
        string?[] a = [null, "A", null];
        string?[] b = ["a", null, null];

        comparer.Equals(a, b).Should().BeTrue();
        comparer.GetHashCode(a).Should().Be(comparer.GetHashCode(b));
    }

    [Fact]
    public void Equals_Should_count_nullable_value_elements()
    {
        var comparer = UnorderedEqualityComparer<int?>.Default;
        comparer.Equals([null, 1, null], [1, null, null]).Should().BeTrue();
        comparer.Equals([null, 1, null], [1, null, 1]).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_Should_use_ValueComparer()
    {
        _sut.GetHashCode(_a).Should().Be(_sut.GetHashCode(_b));
    }

    static readonly MemberPath Path = new([MemberPathSegment.Property("Values")]);

    [Fact]
    public void Inequalities_Should_report_multiplicity_in_source_order()
    {
        var diffs = UnorderedEqualityComparer<int>.Default.Inequalities([3, 1, 1, 2], [2, 1, 4, 4], Path).ToList();

        diffs.Select(d => d.Path.ToString()).Should().Equal("Values[-]", "Values[-]", "Values[+]", "Values[+]");
        diffs.Select(d => d.Left).Should().Equal(new object?[] { 3, 1, null, null });
        diffs.Select(d => d.Right).Should().Equal(new object?[] { null, null, 4, 4 });
    }

    [Fact]
    public void Inequalities_Should_use_ValueComparer_and_match_nulls()
    {
        var comparer = new UnorderedEqualityComparer<string?>(StringComparer.OrdinalIgnoreCase);

        var diffs = comparer.Inequalities([null, "A", null], ["a", "a", null], Path).ToList();

        diffs.Select(d => d.Path.ToString()).Should().Equal("Values[-]", "Values[+]");
        diffs[0].Left.Should().BeNull();
        diffs[1].Right.Should().Be("a");
    }

    [Fact]
    public void Inequalities_Should_report_null_versus_empty_on_the_member()
    {
        var comparer = UnorderedEqualityComparer<int>.Default;

        comparer.Inequalities(null, [], Path).Should().ContainSingle().Which.Path.Should().Be(Path);
        comparer.Inequalities([], null, Path).Should().ContainSingle().Which.Path.Should().Be(Path);
        comparer.Inequalities(null, null, Path).Should().BeEmpty();
        comparer.Inequalities(null, [1], Path).Should().ContainSingle().Which.Path.ToString().Should().Be("Values[+]");
    }

    [Fact]
    public void Inequalities_Should_handle_one_shot_sequences()
    {
        var comparer = UnorderedEqualityComparer<int>.Default;
        var enumerations = 0;
        IEnumerable<int> OneShot() { enumerations++; yield return 1; yield return 1; }

        var diffs = comparer.Inequalities(OneShot(), [1], Path).ToList();

        diffs.Should().ContainSingle().Which.Path.ToString().Should().Be("Values[-]");
        enumerations.Should().Be(1);
    }
}
