using System.Collections.Generic;

namespace Generator.Equals
{
    public class SetEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        static readonly EqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        
        public static SetEqualityComparer<T> Default { get; } = new SetEqualityComparer<T>();

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;
            
            if (x is ISet<T> xSet)
                return xSet.SetEquals(y);
            
            if (y is ISet<T> ySet)
                return ySet.SetEquals(x);

            // This has an awful performance, needs improvement.
            // Please send me a PR! :)
            foreach (var yi in y)
            {
                var found = false;
                
                foreach (var xi in x)
                {
                    if (EqualityComparer.Equals(yi, xi))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }
            
            foreach (var xi in x)
            {
                var found = false;
                
                foreach (var yi in y)
                {
                    if (EqualityComparer.Equals(xi, yi))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }

        public int GetHashCode(IEnumerable<T>? obj) => 0;
    }
}