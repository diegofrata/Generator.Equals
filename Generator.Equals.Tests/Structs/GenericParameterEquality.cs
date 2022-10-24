namespace Generator.Equals.Tests.Structs
{
    public partial class GenericParameterEquality
    {
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Sample<string, int>("Dave", 35);

            public override bool EqualsOperator(object value1, object value2) =>
                (Sample<string, int>) value1 == (Sample<string, int>) value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (Sample<string, int>) value1 != (Sample<string, int>) value2;
        }
        
        
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Sample<string, int>("Dave", 35);
            public override object Factory2() => new Sample<string, int>("Dave", 37);

            public override bool EqualsOperator(object value1, object value2) =>
                (Sample<string, int>) value1 == (Sample<string, int>) value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (Sample<string, int>) value1 != (Sample<string, int>) value2;
        }
    }
}