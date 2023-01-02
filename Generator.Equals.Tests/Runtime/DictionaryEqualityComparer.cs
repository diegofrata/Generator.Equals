using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Generator.Equals.Tests.Runtime;

public class DictionaryEqualityComparer
{
    readonly DictionaryEqualityComparer<string, int> _sut;
    readonly Dictionary<string, int> _a;
    readonly Dictionary<string, int> _b;

    public DictionaryEqualityComparer()
    {
        _sut = new DictionaryEqualityComparer<string, int>(
            new ReverseEqualityComparer(),
            new NegativeEqualityComparer()
        );

        _a = new Dictionary<string, int>
        {
            ["abc"] = 10,
            ["bde"] = 5
        };
        
        _b = new Dictionary<string, int>
        {
            ["cba"] = -10,
            ["bde"] = 5
        };
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