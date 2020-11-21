using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class UnorderedEquality
    {
        [Equatable]
        public partial class Data
        {
            [UnorderedEquality] public List<int>? Properties { get; set; }
        }

        [TestFixture]
        public class EqualsTests : EqualityTestCase
        {
            public override object Factory1()
            {
                var randomSort = new Random();

                // This should generate objects with the same contents, but different orders, thus proving
                // that dictionary equality is being used instead of the normal sequence equality.
                return new Data
                {
                    Properties = Enumerable
                        .Range(1, 1000)
                        .OrderBy(_ => randomSort.NextDouble())
                        .ToList()
                };
            }

            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;

            public override object Factory1() => new Data
            {
                Properties = Enumerable.Range(1, 1000).ToList()
            };

            public override object Factory2() => new Data
            {
                Properties = Enumerable.Range(1, 1001).ToList()
            };

            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}