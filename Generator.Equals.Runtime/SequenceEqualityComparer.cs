using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals
{
    public class SequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        static readonly EqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;

        public static SequenceEqualityComparer<T> Default { get; } = new SequenceEqualityComparer<T>();

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            return x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<T>? obj)
        {
            if (obj == null)
                return 0;

            var hashCode = new HashCode();

            foreach (var item in obj)
                hashCode.Add(item, EqualityComparer);

            return hashCode.ToHashCode();
        }
    }
}