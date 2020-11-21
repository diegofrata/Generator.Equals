using NUnit.Framework;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class OrderedEquality
    {
        [Equatable]
        public partial class Data
        {
            public Data(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }

        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data(new[] {"10 Downing St"});
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data(new[] {"10 Downing St"});
            public override object Factory2() => new Data(new[] {"Bricklane"});
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}