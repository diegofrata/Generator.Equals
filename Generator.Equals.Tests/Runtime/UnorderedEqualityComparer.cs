using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Generator.Equals.Tests.Runtime;

public class UnorderedEqualityComparer
{
    private readonly UnorderedEqualityComparer<int> _sut;
    private readonly int[] _a;
    private readonly int[] _b;

    public UnorderedEqualityComparer()
    {
        _sut = new UnorderedEqualityComparer<int>(
            new NegativeEqualityComparer()
        );

        _a = new[] { 1, 2, 3, 4, 5 };
        _b = new[] { 5, -4, 3, -2, 1 };
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