using System;
using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class UnorderedEqualityWithComparer
    {
        [Equatable]
        public partial class SampleWithStringComparison
        {
            public SampleWithStringComparison(List<string> tags)
            {
                Tags = tags;
            }

            [UnorderedEquality(StringComparison.OrdinalIgnoreCase)]
            public List<string> Tags { get; }
        }

        [Equatable]
        public partial class SampleWithCustomComparer
        {
            public SampleWithCustomComparer(List<string> names)
            {
                Names = names;
            }

            [UnorderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
            public List<string> Names { get; }
        }

        [Equatable]
        public partial class SampleWithLengthComparer
        {
            public SampleWithLengthComparer(List<string> values)
            {
                Values = values;
            }

            [UnorderedEquality(typeof(LengthEqualityComparer))]
            public List<string> Values { get; }
        }

        class LengthEqualityComparer : IEqualityComparer<string>
        {
            public static readonly LengthEqualityComparer Default = new();

            public bool Equals(string? x, string? y) => x?.Length == y?.Length;

            public int GetHashCode(string obj) => obj.Length.GetHashCode();
        }
    }
}
