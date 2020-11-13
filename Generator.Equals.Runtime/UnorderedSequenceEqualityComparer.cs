using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals
{
    public class UnorderedSequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public static IEqualityComparer<IEnumerable<T>> Default { get; } = new UnorderedSequenceEqualityComparer<T>();

        readonly IEqualityComparer<T> _equalityComparer;

        public UnorderedSequenceEqualityComparer() : this(EqualityComparer<T>.Default)
        {
        }

        public UnorderedSequenceEqualityComparer(IEqualityComparer<T> equalityComparer)
        {
            _equalityComparer = equalityComparer;
        }

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            var cnt = new Dictionary<T, int>(_equalityComparer);

            foreach (var s in x)
            {
                if (cnt.TryGetValue(s, out var v))
                {
                    cnt[s] = v + 1;
                }
                else
                {
                    cnt[s] = 1;
                }
            }

            foreach (var s in y)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }

            return cnt.Values.All(c => c == 0);
        }

        public int GetHashCode(IEnumerable<T>? obj)
        {
            var hashCode = 0;

            if (obj == null)
                return hashCode;

            foreach (var t in obj)
                hashCode ^= _equalityComparer.GetHashCode(t) & 0x7FFFFFFF;

            return hashCode;
        }
    }
}
