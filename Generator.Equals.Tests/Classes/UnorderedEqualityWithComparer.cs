using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class UnorderedEqualityWithComparer
    {
        public class EqualsWithStringComparison : EqualityTestCase
        {
            public override object Factory1() => new SampleWithStringComparison(new List<string> { "FOO", "BAR" });
            public override object Factory2() => new SampleWithStringComparison(new List<string> { "bar", "foo" });

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleWithStringComparison)value1 == (SampleWithStringComparison)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleWithStringComparison)value1 != (SampleWithStringComparison)value2;
        }

        public class NotEqualsWithStringComparison : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new SampleWithStringComparison(new List<string> { "FOO", "BAR" });
            public override object Factory2() => new SampleWithStringComparison(new List<string> { "foo", "baz" });

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleWithStringComparison)value1 == (SampleWithStringComparison)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleWithStringComparison)value1 != (SampleWithStringComparison)value2;
        }

        public class EqualsWithCustomComparer : EqualityTestCase
        {
            public override object Factory1() => new SampleWithCustomComparer(new List<string> { "FOO", "BAR" });
            public override object Factory2() => new SampleWithCustomComparer(new List<string> { "bar", "foo" });

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleWithCustomComparer)value1 == (SampleWithCustomComparer)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleWithCustomComparer)value1 != (SampleWithCustomComparer)value2;
        }

        public class NotEqualsWithCustomComparer : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new SampleWithCustomComparer(new List<string> { "FOO", "BAR" });
            public override object Factory2() => new SampleWithCustomComparer(new List<string> { "foo", "baz" });

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleWithCustomComparer)value1 == (SampleWithCustomComparer)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleWithCustomComparer)value1 != (SampleWithCustomComparer)value2;
        }

        public class EqualsWithLengthComparer : EqualityTestCase
        {
            public override object Factory1() => new SampleWithLengthComparer(new List<string> { "aaa", "bb" });
            public override object Factory2() => new SampleWithLengthComparer(new List<string> { "cc", "bbb" });

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleWithLengthComparer)value1 == (SampleWithLengthComparer)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleWithLengthComparer)value1 != (SampleWithLengthComparer)value2;
        }

        public class NotEqualsWithLengthComparer : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new SampleWithLengthComparer(new List<string> { "aaa", "bb" });
            public override object Factory2() => new SampleWithLengthComparer(new List<string> { "bb", "aaaa" });

            public override bool EqualsOperator(object value1, object value2) =>
                (SampleWithLengthComparer)value1 == (SampleWithLengthComparer)value2;

            public override bool NotEqualsOperator(object value1, object value2) =>
                (SampleWithLengthComparer)value1 != (SampleWithLengthComparer)value2;
        }
    }
}
