using System.Collections.Generic;
using FluentAssertions;
using Xunit;

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

        _a = new[] { 1, 2, 3, 4, 5 };
        _b = new[] { 5, -4, 3, -2, 1 };
    }
    
    [Fact]
    public void Equals_Should_use_ValueComparer()
    {
        _sut.Equals(_a, _b).Should().BeTrue();
    }
    
    [Fact]
    public void GetHashCode_Should_use_ValueComparer()
    {
        _sut.GetHashCode(_a).Should().Be(_sut.GetHashCode(_b));
    }
}