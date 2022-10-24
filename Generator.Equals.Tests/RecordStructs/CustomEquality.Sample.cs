using System.Collections.Generic;

namespace Generator.Equals.Tests.RecordStructs
{
    public partial class CustomEquality
    {
        [Equatable]
        public partial record struct Sample
        (
            [property: CustomEquality(typeof(Comparer1))] string Name1,
            [property: CustomEquality(typeof(Comparer2), nameof(Comparer2.Instance))] string Name2,
            [property: CustomEquality(typeof(LengthEqualityComparer))] string Name3
        );

        class Comparer1
        {
            public static readonly LengthEqualityComparer Default = new();
        }

        class Comparer2
        {
            public static readonly LengthEqualityComparer Instance = new();
        }

        class LengthEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y) => x?.Length == y?.Length;

            public int GetHashCode(string obj) => obj.Length.GetHashCode();
        }
    }
}