using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class CustomEquality
    {
        [Equatable]
        public partial class Sample
        {
            public Sample(string name1, string name2, string name3)
            {
                Name1 = name1;
                Name2 = name2;
                Name3 = name3;
            }

            [CustomEquality(typeof(Comparer1))] public string Name1 { get; }
            [CustomEquality(typeof(Comparer2), nameof(Comparer2.Instance))] public string Name2 { get; }

            [CustomEquality(typeof(LengthEqualityComparer))]
            public string Name3 { get; }
        }
    
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