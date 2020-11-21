using NUnit.Framework;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class PrimitiveEquality 
    {
        [Equatable]
        public partial class Data
        {
            public Data(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
            public int Age { get; }
        }
        
        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data("Dave", 35);
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {

            public override bool Expected => false;
            public override object Factory1() => new Data("Dave", 35);
            public override object Factory2() => new Data("Joe", 77);
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}