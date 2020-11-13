using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals
{
    public class SequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public static SequenceEqualityComparer<T> Default { get; } = new SequenceEqualityComparer<T>();
        
        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<T>? obj)
        {
            if (obj is null) 
                return 0;
            
            var hashCode = new HashCode();
            
            foreach (var item in obj) 
                hashCode.Add(item, EqualityComparer<T>.Default);

            return hashCode.ToHashCode();
        }
    }
}
