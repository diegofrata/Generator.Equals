using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals
{
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        public static DictionaryEqualityComparer<TKey, TValue> Default { get; } =
            new DictionaryEqualityComparer<TKey, TValue>();

        public bool Equals(IDictionary<TKey, TValue>? x, IDictionary<TKey, TValue>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            if (x.Count != y.Count)
                return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (var (key, xValue) in x)
            {
                if (!y.TryGetValue(key, out var yValue))
                    return false;

                if (!comparer.Equals(xValue, yValue))
                    return false;
            }

            return true;
        }

        public int GetHashCode(IDictionary<TKey, TValue>? obj)
        {
            // There's no easy way to hash a dictionary.
            return obj?.Count ?? 0;
        }
    }
}
