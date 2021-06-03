using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class CustomEquality
    {
        [Equatable]
        public partial class Data
        {
            public Data(string name1, string name2, string name3)
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


        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data("My String", "My String", "My String");
            public override object Factory2() => new Data("My ____ng", "My ____ng", "My ____ng");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data("My String", "My String", "My String");
            public override object Factory2() => new Data("My String ", "My String ", "My String ");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}
