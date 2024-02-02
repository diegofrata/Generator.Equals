namespace Generator.Equals.Tests.Structs
{
    public partial class StringEquality
    {
        public class EqualsTests : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseInsensitive { Name = "BAR" };
            public override object Factory2() => new SampleCaseInsensitive { Name = "bar" };

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 == (SampleCaseInsensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 != (SampleCaseInsensitive)value2;
        }

        public class NotEqualsTest : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseInsensitive { Name = "BAR" };
            public override object Factory2() => new SampleCaseInsensitive { Name = "foo" };
            public override bool Expected => false;

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 == (SampleCaseInsensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 != (SampleCaseInsensitive)value2;
        }
    }
}
