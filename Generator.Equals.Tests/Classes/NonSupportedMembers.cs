using NUnit.Framework;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class NonSupportedMembers 
    {
        [Equatable]
        public partial class Data
        {
            public Data(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public static int StaticProperty { get; }

            public int this[int index] => index;
        }

        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data("Dave");
            public override object Factory2() => new Data("Dave");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public  class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data("Dave");
            public override object Factory2() => new Data("John");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}