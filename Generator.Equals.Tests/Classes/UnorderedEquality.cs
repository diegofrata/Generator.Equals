using System;
using System.Linq;

namespace Generator.Equals.Tests.Classes
{
    public partial class UnorderedEquality
    {
        public class EqualsTests : EqualityTestCase
        {
            public override object Factory1()
            {
                var randomSort = new Random();

                // This should generate objects with the same contents, but different orders, thus proving
                // that dictionary equality is being used instead of the normal sequence equality.
                return new Sample
                {
                    Properties = Enumerable
                        .Range(1, 1000)
                        .OrderBy(_ => randomSort.NextDouble())
                        .ToList()
                };
            }

            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;
        }
        
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;

            public override object Factory1() => new Sample
            {
                Properties = Enumerable.Range(1, 1000).ToList()
            };

            public override object Factory2() => new Sample
            {
                Properties = Enumerable.Range(1, 1001).ToList()
            };

            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;
        }
    }
}