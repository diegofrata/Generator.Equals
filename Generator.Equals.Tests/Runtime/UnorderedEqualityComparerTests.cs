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
}
