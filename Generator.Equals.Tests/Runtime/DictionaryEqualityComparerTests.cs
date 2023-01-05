using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Generator.Equals.Tests.Runtime;

public class DictionaryEqualityComparerTests
{
    readonly DictionaryEqualityComparer<string, int> _sut;

    public DictionaryEqualityComparerTests()
    {
        _sut = new DictionaryEqualityComparer<string, int>(
            new ReverseEqualityComparer(),
            new NegativeEqualityComparer()
        );
    }
    
    [Fact]
    public void Equals_Should_use_ValueComparer()
    {
        var a = new Dictionary<string, int>
        {
            ["abc"] = 10,
            ["bde"] = 5
        };
        
        var b = new Dictionary<string, int>
        {
            ["abc"] = -10,
            ["bde"] = 5
        };
        
        _sut.Equals(a, b).Should().BeTrue();
    }
    
    
    [Fact]
    public void Equals_Should_not_use_KeyComparer()
    {
        var a = new Dictionary<string, int>
        {
            ["abc"] = 10,
            ["bde"] = 5
        };
        
        var b = new Dictionary<string, int>
        {
            ["cba"] = 10,
            ["bde"] = 5
        };
        
        _sut.Equals(a, b).Should().BeFalse();
    }
    
    [Fact]
    public void GetHashCode_Should_use_both_KeyComparer_and_ValueComparer()
    {
        var a = new Dictionary<string, int>
        {
            ["abc"] = 10,
            ["bde"] = 5
        };
        
        var b = new Dictionary<string, int>
        {
            ["cba"] = -10,
            ["bde"] = 5
        };
        
        _sut.GetHashCode(a).Should().Be(_sut.GetHashCode(b));
    }
}