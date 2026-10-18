#nullable enable
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Reports the element-level differences between two sequences compared as sets:
        /// elements of <paramref name="x"/> absent from <paramref name="y"/> are removed and
        /// elements of <paramref name="y"/> absent from <paramref name="x"/> are added, using the
        /// same comparer resolution as <see cref="Equals(IEnumerable{T}, IEnumerable{T})"/>.
        /// Null versus empty has no element to point at and is reported as a single difference
        /// on <paramref name="path"/> itself; null versus a non-empty sequence is treated as empty.
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

            var comparer = ResolveComparer(x, y);
            var xSet = new HashSet<T>(x, comparer);
            var ySet = new HashSet<T>(y, comparer);

            foreach (var item in xSet)
                if (!ySet.Contains(item))
                    yield return new Inequality(path.Append(MemberPathSegment.Removed()), item, null);

            foreach (var item in ySet)
                if (!xSet.Contains(item))
                    yield return new Inequality(path.Append(MemberPathSegment.Added()), null, item);
        }

        // Mirrors Equals: without an attribute-specified comparer, a HashSet input's own comparer wins.
        IEqualityComparer<T> ResolveComparer(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (!ReferenceEquals(EqualityComparer, DefaultEqualityComparer<T>.Default))
                return EqualityComparer;

            if (x is HashSet<T> xSet)
                return xSet.Comparer;

            if (y is HashSet<T> ySet)
                return ySet.Comparer;

            return EqualityComparer;
        }

        public int GetHashCode(IEnumerable<T>? obj) => 0;
    }
}
