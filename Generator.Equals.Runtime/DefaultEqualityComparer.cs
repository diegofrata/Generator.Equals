using System.Collections.Generic;

namespace Generator.Equals
{
    public class DefaultEqualityComparer<T> : IEqualityComparer<T>
    {
        public static DefaultEqualityComparer<T> Default { get; } = new DefaultEqualityComparer<T>();

        static readonly IEqualityComparer<T> _underlying;

        static DefaultEqualityComparer()
        {
            if (typeof(T).IsSealed)
                _underlying = EqualityComparer<T>.Default;
            else
                _underlying = new ObjectEqualityComparer();
        }

        public bool Equals(T x, T y) => _underlying.Equals(x, y);

        public int GetHashCode(T obj) => _underlying.GetHashCode(obj);

        class ObjectEqualityComparer : IEqualityComparer<T>
        {
            public bool Equals(T x, T y) => object.Equals(x, y);

            public int GetHashCode(T obj) => obj!.GetHashCode();
        }
    }
}