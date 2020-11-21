using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public class DefaultBehavior 
    {
        [TestFixture]
        public class RecordsWithArray : EqualityTestCase
        {
            public record Data(string Name, int Age, string[] Addresses);

            public override bool Expected => false;

            public override object Factory1() => new Data("Dave", 35, new[] {"10 Downing St"});
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public class RecordsWithPrimitives : EqualityTestCase
        {
            public record Data(string Name, int Age);

            public override object Factory1() => new Data("Dave", 35);
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public class DistinctRecords : EqualityTestCase
        {
            public record Data(string Name, int Age);

            public override bool Expected => false;
            public override object Factory1() => new Data("Dave", 35);
            public override object Factory2() => new Data("John", 25);
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}