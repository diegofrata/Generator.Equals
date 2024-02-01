namespace Generator.Equals.Tests.Classes
{
    public partial class StringEquality
    {
        public class EqualsTestsNotCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseInsensitive("BAR");
            public override object Factory2() => new SampleCaseInsensitive("bar");

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 == (SampleCaseInsensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 != (SampleCaseInsensitive)value2;
        }

        public class NotEqualsTestsNotCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseInsensitive("BAR");
            public override object Factory2() => new SampleCaseInsensitive("foo");
            public override bool Expected => false;

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 == (SampleCaseInsensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 != (SampleCaseInsensitive)value2;
        }

        public class EqualsTestsCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseSensitive("Foo");
            public override object Factory2() => new SampleCaseSensitive("Foo");

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 == (SampleCaseSensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 != (SampleCaseSensitive)value2;
        }

        public class NotEqualsTestsCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseSensitive("Foo");
            public override object Factory2() => new SampleCaseSensitive("foo");
            public override bool Expected => false;

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 == (SampleCaseSensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 != (SampleCaseSensitive)value2;
        }
    }
}
