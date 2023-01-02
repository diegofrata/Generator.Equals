using System.Collections.Generic;

namespace Generator.Equals
{
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

            var xSet = new HashSet<T>(x, EqualityComparer);
            return xSet.SetEquals(y);
        }

        public int GetHashCode(IEnumerable<T>? obj) => 0;
    }
}