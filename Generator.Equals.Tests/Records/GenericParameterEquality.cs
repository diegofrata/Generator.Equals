using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class GenericParameterEquality
    {
        [Equatable]
        public partial record Data<TName, TAge>(TName Name, TAge Age);
        
        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data<string, int>("Dave", 35);

            public override bool EqualsOperator(object value1, object value2) =>
                (Data<string, int>) value1 == (Data<string, int>) value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (Data<string, int>) value1 != (Data<string, int>) value2;
        }
        
        
        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data<string, int>("Dave", 35);
            public override object Factory2() => new Data<string, int>("Dave", 37);

            public override bool EqualsOperator(object value1, object value2) =>
                (Data<string, int>) value1 == (Data<string, int>) value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (Data<string, int>) value1 != (Data<string, int>) value2;
        }
    }
}