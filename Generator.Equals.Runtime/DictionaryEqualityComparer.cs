using System;
using System.Collections.Generic;

namespace Generator.Equals
{
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        class KeyPairEqualityComparer : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            readonly IEqualityComparer<TValue> _valueEqualityComparer;

            public KeyPairEqualityComparer(IEqualityComparer<TValue> valueEqualityComparer)
            {
                _valueEqualityComparer = valueEqualityComparer;
            }
            
            public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return Equals(x.Key, y.Key) && _valueEqualityComparer.Equals(x.Value, y.Value);
            }

            public int GetHashCode(KeyValuePair<TKey, TValue> obj)
            {
                return HashCode.Combine(obj.Key, _valueEqualityComparer.GetHashCode(obj.Value));
            }
        }
        

        public static DictionaryEqualityComparer<TKey, TValue> Default { get; } =
            new DictionaryEqualityComparer<TKey, TValue>();

        readonly KeyPairEqualityComparer _keyPairEqualityComparer;
        
        public IEqualityComparer<TValue> ValueEqualityComparer { get; }

        public DictionaryEqualityComparer() : this(DefaultEqualityComparer<TValue>.Default)
        {
        }
        
        public DictionaryEqualityComparer(IEqualityComparer<TValue> valueEqualityComparer)
        {
            _keyPairEqualityComparer = new KeyPairEqualityComparer(valueEqualityComparer);
            ValueEqualityComparer = valueEqualityComparer;
        }

        public bool Equals(IDictionary<TKey, TValue>? x, IDictionary<TKey, TValue>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            foreach (var pair in x)
            {
                if (!y.TryGetValue(pair.Key, out var yValue))
                    return false;

                if (!ValueEqualityComparer.Equals(pair.Value, yValue))
                    return false;
            }

            return true;
        }

        public int GetHashCode(IDictionary<TKey, TValue>? obj)
        {
            var hashCode = 0;

            if (obj == null)
                return hashCode;

            foreach (var t in obj)
                hashCode ^= _keyPairEqualityComparer.GetHashCode(t) & 0x7FFFFFFF;

            return hashCode;
        }
    }
}