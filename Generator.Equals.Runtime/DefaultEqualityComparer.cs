#nullable enable
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
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

        public bool Equals(T? x, T? y)
        {
            return _underlying.Equals(x!, y!);
        }

        public int GetHashCode(T obj)
        {
            return _underlying.GetHashCode(obj!);
        }

        [GeneratedCode("Generator.Equals", "1.0.0.0")]
        class ObjectEqualityComparer : IEqualityComparer<T>
        {
            public bool Equals(T? x, T? y)
            {
                return object.Equals(x!, y!);
            }

            public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
        }
    }
}