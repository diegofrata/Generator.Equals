using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class SetEquality
    {
        [Equatable]
        public partial record Data
        {
            [SetEquality] public List<int>? Properties { get; set; }
        }

        [TestFixture]
        public class EqualsTests : EqualityTestCase
        {
            public override object Factory1()
            {
                return new Data
                {
                    Properties = new List<int> { 1, 2, 3, 4, 5, 5, 4, 3, 2, 1}
                };
            }
            
            public override object Factory2()
            {
                return new Data
                {
                    Properties = new List<int> { 1, 5, 2, 4, 3 }
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
                Properties = new List<int> { 1, 2, 3, 4, 5 }
            };

            public override object Factory2() => new Data
            {
                Properties = new List<int> { 1, 2, 3, 4 }
            };

            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;

            public override void HashCode()
            {
                var value1 = Factory1();
                var value2 = Factory2();

                Assert.That(value1.GetHashCode() == value2.GetHashCode(), Is.True);
            }
        }
    }
}