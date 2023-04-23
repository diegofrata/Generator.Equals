#nullable enable
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    public class DictionaryEqualityComparer<TKey, TValue> : IEqualityComparer<IDictionary<TKey, TValue>>
    {
        [GeneratedCode("Generator.Equals", "1.0.0.0")]
        class KeyPairEqualityComparer : IEqualityComparer<KeyValuePair<TKey, TValue>>
        {
            readonly IEqualityComparer<TKey> _keyEqualityComparer;
            readonly IEqualityComparer<TValue> _valueEqualityComparer;

            public KeyPairEqualityComparer(
                IEqualityComparer<TKey> keyEqualityComparer,
                IEqualityComparer<TValue> valueEqualityComparer)
            {
                _keyEqualityComparer = keyEqualityComparer;
                _valueEqualityComparer = valueEqualityComparer;
            }

            public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return _keyEqualityComparer.Equals(x.Key!, y.Key!) &&
                       _valueEqualityComparer.Equals(x.Value!, y.Value!);
            }

            public int GetHashCode(KeyValuePair<TKey, TValue> obj)
            {
                return HashCode.Combine(
                    _keyEqualityComparer.GetHashCode(obj.Key!),
                    _valueEqualityComparer.GetHashCode(obj.Value!)
                );
            }
        }


        public static DictionaryEqualityComparer<TKey, TValue> Default { get; } =
            new DictionaryEqualityComparer<TKey, TValue>();

        readonly KeyPairEqualityComparer _keyPairEqualityComparer;
        public IEqualityComparer<TKey> KeyEqualityComparer { get; }
        public IEqualityComparer<TValue> ValueEqualityComparer { get; }

        public DictionaryEqualityComparer() : this(DefaultEqualityComparer<TKey>.Default,
            DefaultEqualityComparer<TValue>.Default)
        {
        }

        public DictionaryEqualityComparer(IEqualityComparer<TKey> keyEqualityComparer,
            IEqualityComparer<TValue> valueEqualityComparer)
        {
            _keyPairEqualityComparer = new KeyPairEqualityComparer(keyEqualityComparer, valueEqualityComparer);
            KeyEqualityComparer = keyEqualityComparer;
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

            // Here we intentionally do not use the KeyComparer. Dictionaries already 
            // may have different key comparers associated and there is no way for us to know.
            foreach (var pair in x)
            {
                if (!y.TryGetValue(pair.Key, out var yValue))
                    return false;

                if (!ValueEqualityComparer.Equals(pair.Value!, yValue!))
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