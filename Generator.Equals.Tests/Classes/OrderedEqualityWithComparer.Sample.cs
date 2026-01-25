using System;
using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class OrderedEqualityWithComparer
    {
        [Equatable]
        public partial class SampleWithStringComparison
        {
            public SampleWithStringComparison(string[] tags)
            {
                Tags = tags;
            }

            [OrderedEquality(StringComparison.OrdinalIgnoreCase)]
            public string[] Tags { get; }
        }

        [Equatable]
        public partial class SampleWithCustomComparer
        {
            public SampleWithCustomComparer(string[] names)
            {
                Names = names;
            }

            [OrderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
            public string[] Names { get; }
        }

        [Equatable]
        public partial class SampleWithLengthComparer
        {
            public SampleWithLengthComparer(string[] values)
            {
                Values = values;
            }

            [OrderedEquality(typeof(LengthEqualityComparer))]
            public string[] Values { get; }
        }

        class LengthEqualityComparer : IEqualityComparer<string>
        {
            public static readonly LengthEqualityComparer Default = new();

            public bool Equals(string? x, string? y) => x?.Length == y?.Length;

            public int GetHashCode(string obj) => obj.Length.GetHashCode();
        }
    }
}
