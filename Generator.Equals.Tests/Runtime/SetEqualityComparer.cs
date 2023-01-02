using FluentAssertions;
using Xunit;

namespace Generator.Equals.Tests.Runtime;

public class SetEqualityComparer
{
    readonly SetEqualityComparer<int> _sut;
    readonly int[] _a;
    readonly int[] _b;

    public SetEqualityComparer()
    {
        _sut = new SetEqualityComparer<int>(
            new NegativeEqualityComparer()
        );

        _a = new[] { 1, 2, 3, 4, 5 };
        _b = new[] { 1, -2, 3, -4, 5 };
    }
    
    [Fact]
    public void Equals_Uses_specified_comparers()
    {
        _sut.Equals(_a, _b).Should().BeTrue();
    }
    
    [Fact]
    public void GetHashCode_Uses_specified_comparers()
    {
        _sut.GetHashCode(_a).Should().Be(_sut.GetHashCode(_b));
    }
}