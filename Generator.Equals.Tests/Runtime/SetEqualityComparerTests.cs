using System.Collections.Generic;
using FluentAssertions;
using Xunit;

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
    public void Sets_Equals_Should_not_use_ValueComparer()
    {
        var a = new HashSet<int> { 1, 2, 3, 4, 5 };
        var b = new[] { 1, -2, 3, -4, 5 };

        _sut.Equals(a, b).Should().BeFalse();
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
}