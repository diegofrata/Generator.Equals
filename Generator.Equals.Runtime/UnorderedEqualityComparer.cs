using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    public class UnorderedEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public static IEqualityComparer<IEnumerable<T>> Default { get; } = new UnorderedEqualityComparer<T>();

        public IEqualityComparer<T> EqualityComparer { get; }

        readonly ElementKeyComparer _keyComparer;

        public UnorderedEqualityComparer() : this(DefaultEqualityComparer<T>.Default)
        {
        }

        public UnorderedEqualityComparer(IEqualityComparer<T> equalityComparer)
        {
            EqualityComparer = equalityComparer;
            _keyComparer = new ElementKeyComparer(equalityComparer);
        }

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (x is ICollection<T> xCollection &&
                y is ICollection<T> yCollection &&
                xCollection.Count != yCollection.Count) return false;

            // Wrap elements in a value-type key so Dictionary accepts null elements too.
            var cnt = new Dictionary<ValueTuple<T>, int>(_keyComparer);

            foreach (var s in x)
            {
                var key = ValueTuple.Create(s);
                cnt[key] = (cnt.TryGetValue(key, out var v) ? v : 0) + 1;
            }

            foreach (var s in y)
            {
                var key = ValueTuple.Create(s);
                if (!cnt.TryGetValue(key, out var count))
                    return false;

                cnt[key] = count - 1;
            }

            return cnt.Values.All(c => c == 0);
        }

        [GeneratedCode("Generator.Equals", "1.0.0.0")]
        sealed class ElementKeyComparer : IEqualityComparer<ValueTuple<T>>
        {
            readonly IEqualityComparer<T> _comparer;

            public ElementKeyComparer(IEqualityComparer<T> comparer) => _comparer = comparer;

            public bool Equals(ValueTuple<T> x, ValueTuple<T> y) => _comparer.Equals(x.Item1, y.Item1);

            public int GetHashCode(ValueTuple<T> obj) => obj.Item1 is null ? 0 : _comparer.GetHashCode(obj.Item1);
        }

        public int GetHashCode(IEnumerable<T>? obj)
        {
            var hashCode = 0;

            if (obj == null)
                return hashCode;

            foreach (var t in obj)
                if (t != null)
                    hashCode ^= EqualityComparer.GetHashCode(t) & 0x7FFFFFFF;

            return hashCode;
        }
    }
}