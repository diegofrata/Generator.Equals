using System.Collections.Generic;
using FluentAssertions;

namespace Generator.Equals.Tests.Structs
{
    public partial class SetEquality
    {
        public class EqualsTests : EqualityTestCase
        {
            public override object Factory1()
            {
                return new Sample
                {
                    Properties = new List<int> { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1}
                };
            }
            
            public override object Factory2()
            {
                return new Sample
                {
                    Properties = new List<int> { 1, 5, 2, 4, 3 }
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
                Properties = new List<int> { 1, 2, 3, 4, 5 }
            };

            public override object Factory2() => new Sample
            {
                Properties = new List<int> { 1, 2, 3, 4 }
            };

            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;

            public override void TestHashCode()
            {
                var value1 = Factory1();
                var value2 = Factory2();
                var result = value1.GetHashCode() == value2.GetHashCode();
                result.Should().BeTrue();
            }
        }
    }
}