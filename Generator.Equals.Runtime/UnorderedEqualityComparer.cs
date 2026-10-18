using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    public class UnorderedEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public static UnorderedEqualityComparer<T> Default { get; } = new UnorderedEqualityComparer<T>();

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

        /// <summary>
        /// Reports the element-level differences between two sequences compared as bags.
        /// Every occurrence in <paramref name="x"/> with no counterpart in <paramref name="y"/>
        /// (under the configured element comparer) is reported as removed, and every unmatched
        /// occurrence in <paramref name="y"/> as added, so multiplicity is preserved. Null versus
        /// empty has no element to point at and is reported as a single difference on
        /// <paramref name="path"/> itself; null versus a non-empty sequence is treated as empty.
        /// </summary>
        public IEnumerable<Inequality> Inequalities(IEnumerable<T>? x, IEnumerable<T>? y, MemberPath path)
        {
            if (x is null && y is null)
                yield break;

            if (x is null || y is null)
            {
                if (!(x ?? y)!.Any())
                {
                    yield return new Inequality(path, x, y);
                    yield break;
                }

                x ??= Array.Empty<T>();
                y ??= Array.Empty<T>();
            }

            // x is walked twice below, so pin down one-shot sequences first.
            var xs = x as ICollection<T> ?? x.ToList();

            var cnt = new Dictionary<ValueTuple<T>, int>(_keyComparer);

            foreach (var s in xs)
            {
                var key = ValueTuple.Create(s);
                cnt[key] = (cnt.TryGetValue(key, out var v) ? v : 0) + 1;
            }

            var added = new List<T>();

            foreach (var s in y)
            {
                var key = ValueTuple.Create(s);
                if (cnt.TryGetValue(key, out var v) && v > 0)
                    cnt[key] = v - 1;
                else
                    added.Add(s);
            }

            // Whatever count is left over was removed; walk x again so removals keep source order.
            foreach (var s in xs)
            {
                var key = ValueTuple.Create(s);
                var v = cnt[key];
                if (v > 0)
                {
                    cnt[key] = v - 1;
                    yield return new Inequality(path.Append(MemberPathSegment.Removed()), s, null);
                }
            }

            foreach (var s in added)
                yield return new Inequality(path.Append(MemberPathSegment.Added()), null, s);
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
