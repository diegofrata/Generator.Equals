using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals.Tests.Runtime;

class ReverseEqualityComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        return string.Equals(x, y) || string.Equals(x,  new string(y!.Reverse().ToArray()));
    }

    public int GetHashCode(string obj)
    {
        var str = new string(obj.OrderBy(x => x).ToArray());
        
        return str.GetHashCode();
    }
}

class NegativeEqualityComparer : IEqualityComparer<int>
{
    public bool Equals(int x, int y) => x == y || x == -y;

    public int GetHashCode(int obj) => Math.Abs(obj).GetHashCode();
}