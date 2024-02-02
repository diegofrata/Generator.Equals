namespace Generator.Equals.Tests.Records
{
    public partial class StringEquality
    {
        public class EqualsTestsNotCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseInsensitive { Name = "BAR" };
            public override object Factory2() => new SampleCaseInsensitive { Name = "bar" };

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 == (SampleCaseInsensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 != (SampleCaseInsensitive)value2;
        }

        public class NotEqualsTestsNotCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseInsensitive { Name = "BAR" };
            public override object Factory2() => new SampleCaseInsensitive { Name = "foo" };
            public override bool Expected => false;

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 == (SampleCaseInsensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseInsensitive)value1 != (SampleCaseInsensitive)value2;
        }

        public class EqualsTestsCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseSensitive { Name = "Foo" };
            public override object Factory2() => new SampleCaseSensitive { Name = "Foo" };

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 == (SampleCaseSensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 != (SampleCaseSensitive)value2;
        }

        public class NotEqualsTestsCaseSensitive : EqualityTestCase
        {
            public override object Factory1() => new SampleCaseSensitive { Name = "Foo" };
            public override object Factory2() => new SampleCaseSensitive { Name = "foo" };
            public override bool Expected => false;

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 == (SampleCaseSensitive)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleCaseSensitive)value1 != (SampleCaseSensitive)value2;
        }
    }
}
