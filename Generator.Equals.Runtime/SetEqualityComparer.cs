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

            // When no custom comparer was specified, delegate to the set's own
            // comparer so we respect whatever the collection was built with.
            if (ReferenceEquals(EqualityComparer, DefaultEqualityComparer<T>.Default))
            {
                if (x is ISet<T> xSet)
                    return xSet.SetEquals(y);

                if (y is ISet<T> ySet)
                    return ySet.SetEquals(x);
            }

            // When a custom comparer was specified (or neither input is a set),
            // build a fresh set with the attribute-specified comparer.
            var set = new HashSet<T>(x, EqualityComparer);
            return set.SetEquals(y);
        }

        public int GetHashCode(IEnumerable<T>? obj) => 0;
    }
}