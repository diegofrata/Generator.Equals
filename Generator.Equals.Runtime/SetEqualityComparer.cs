#nullable enable
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    public class SetEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public static SetEqualityComparer<T> Default { get; } = new SetEqualityComparer<T>();

        public IEqualityComparer<T> EqualityComparer { get; }

        public SetEqualityComparer() : this(DefaultEqualityComparer<T>.Default)
        {
        }

        public SetEqualityComparer(IEqualityComparer<T> equalityComparer)
        {
            EqualityComparer = equalityComparer;
        }

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            // If either of the underlying collections is a set, then we want to respect whatever 
            // is the equality comparer tha was specified.
            if (x is ISet<T> xSet)
                return xSet.SetEquals(y);

            if (y is ISet<T> ySet)
                return ySet.SetEquals(x);

            // Otherwise we go with our own.
            xSet = new HashSet<T>(x, EqualityComparer);
            return xSet.SetEquals(y);
        }

        public int GetHashCode(IEnumerable<T>? obj) => 0;
    }
}