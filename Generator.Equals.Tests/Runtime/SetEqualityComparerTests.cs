using FluentAssertions;

namespace Generator.Equals.Tests.Runtime;

public class SetEqualityComparerTests
{
    readonly SetEqualityComparer<int> _sut;

    public SetEqualityComparerTests()
    {
        _sut = new SetEqualityComparer<int>(
            new NegativeEqualityComparer()
        );
    }

    [Fact]
    public void NonSets_Equals_Should_use_ValueComparer()
    {
        var a = new[] { 1, 2, 3, 4, 5 };
        var b = new[] { 1, -2, 3, -4, 5 };

        _sut.Equals(a, b).Should().BeTrue();
    }

    [Fact]
    public void Sets_Equals_Should_use_ValueComparer_over_collection_comparer()
    {
        var a = new HashSet<int> { 1, 2, 3, 4, 5 };
        var b = new[] { 1, -2, 3, -4, 5 };

        // The custom comparer passed to SetEqualityComparer is authoritative,
        // even when the input is a set with a different (default) comparer.
        _sut.Equals(a, b).Should().BeTrue();
    }

    [Fact]
    public void Sets_Equals_Should_use_A_collection_comparer()
    {
        var a = new HashSet<int>(new NegativeEqualityComparer()) { 1, 2, 3, 4, 5 };
        var b = new[] { 1, -2, 3, -4, 5 };

        _sut.Equals(a, b).Should().BeTrue();
    }

    [Fact]
    public void Sets_Equals_Should_use_B_collection_comparer()
    {
        var a = new[] { 1, 2, 3, 4, 5 };
        var b = new HashSet<int>(new NegativeEqualityComparer()) { 1, -2, 3, -4, 5 };

        _sut.Equals(a, b).Should().BeTrue();
    }

    static readonly MemberPath Path = new([MemberPathSegment.Property("Values")]);

    [Fact]
    public void Inequalities_Should_use_ValueComparer()
    {
        var diffs = _sut.Inequalities([1, 2, 3], [-1, 2, 5], Path).ToList();

        diffs.Select(d => d.Path.ToString()).Should().Equal("Values[-]", "Values[+]");
        diffs[0].Left.Should().Be(3);
        diffs[1].Right.Should().Be(5);
    }

    [Fact]
    public void Inequalities_Should_use_set_comparer_when_no_ValueComparer()
    {
        var a = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "A", "b" };

        var diffs = SetEqualityComparer<string>.Default.Inequalities(a, ["a", "c"], Path).ToList();

        diffs.Select(d => d.Path.ToString()).Should().Equal("Values[-]", "Values[+]");
        diffs[0].Left.Should().Be("b");
        diffs[1].Right.Should().Be("c");
    }

    [Fact]
    public void Inequalities_Should_report_null_versus_empty_on_the_member()
    {
        var comparer = SetEqualityComparer<int>.Default;

        comparer.Inequalities(null, [], Path).Should().ContainSingle().Which.Path.Should().Be(Path);
        comparer.Inequalities([], null, Path).Should().ContainSingle().Which.Path.Should().Be(Path);
        comparer.Inequalities(null, null, Path).Should().BeEmpty();
        comparer.Inequalities(null, [1], Path).Should().ContainSingle().Which.Path.ToString().Should().Be("Values[+]");
    }
}
