namespace Generator.Equals.Tests.Classes
{
    public partial class CustomEquality
    {
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Sample("My String", "My String", "My String");
            public override object Factory2() => new Sample("My ____ng", "My ____ng", "My ____ng");
            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;
        }

        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Sample("My String", "My String", "My String");
            public override object Factory2() => new Sample("My String ", "My String ", "My String ");
            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;
        }
    }
}
