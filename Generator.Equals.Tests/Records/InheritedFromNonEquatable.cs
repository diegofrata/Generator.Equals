namespace Generator.Equals.Tests.Records
{
    public partial class InheritedFromNonEquatable
    {
        // Tests that Child includes Ints with inherited [OrderedEquality]
        // because Parent doesn't have [Equatable]
        public class ChildEqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Child { Ints = new[] { 1, 2, 3 } };
            public override bool EqualsOperator(object value1, object value2) => (Child)value1 == (Child)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Child)value1 != (Child)value2;
        }

        // Tests that order matters (because of inherited OrderedEquality)
        public class ChildOrderMattersTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Child { Ints = new[] { 1, 2, 3 } };
            public override object Factory2() => new Child { Ints = new[] { 3, 2, 1 } };
            public override bool EqualsOperator(object value1, object value2) => (Child)value1 == (Child)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Child)value1 != (Child)value2;
        }

        // Tests that different arrays are not equal
        public class ChildNotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Child { Ints = new[] { 1, 2, 3 } };
            public override object Factory2() => new Child { Ints = new[] { 1, 2, 4 } };
            public override bool EqualsOperator(object value1, object value2) => (Child)value1 == (Child)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Child)value1 != (Child)value2;
        }
    }
}
