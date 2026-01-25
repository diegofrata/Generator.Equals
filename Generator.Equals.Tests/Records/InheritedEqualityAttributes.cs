namespace Generator.Equals.Tests.Records
{
    public partial class InheritedEqualityAttributes
    {
        // Tests that Child inherits [OrderedEquality] from Parent
        // and arrays are compared element-by-element (same elements = equal)
        public class ChildEqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Child { Ints = new[] { 1, 2, 3 } };
            public override bool EqualsOperator(object value1, object value2) => (Child)value1 == (Child)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Child)value1 != (Child)value2;
        }

        // Tests that different arrays are not equal (ordered comparison)
        public class ChildNotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Child { Ints = new[] { 1, 2, 3 } };
            public override object Factory2() => new Child { Ints = new[] { 1, 2, 4 } };
            public override bool EqualsOperator(object value1, object value2) => (Child)value1 == (Child)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Child)value1 != (Child)value2;
        }

        // Tests that order matters for ordered equality (inherited from parent)
        public class ChildOrderMattersTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Child { Ints = new[] { 1, 2, 3 } };
            public override object Factory2() => new Child { Ints = new[] { 3, 2, 1 } };
            public override bool EqualsOperator(object value1, object value2) => (Child)value1 == (Child)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Child)value1 != (Child)value2;
        }

        // Tests that ChildWithOwnAttribute uses [UnorderedEquality] (order doesn't matter)
        public class ChildWithOwnAttributeEqualsTest : EqualityTestCase
        {
            // Same elements, different order - should be equal because of UnorderedEquality
            public override object Factory1() => new ChildWithOwnAttribute { Ints = new[] { 1, 2, 3 } };
            public override object Factory2() => new ChildWithOwnAttribute { Ints = new[] { 3, 2, 1 } };
            public override bool EqualsOperator(object value1, object value2) => (ChildWithOwnAttribute)value1 == (ChildWithOwnAttribute)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (ChildWithOwnAttribute)value1 != (ChildWithOwnAttribute)value2;
        }

        // Tests multi-level inheritance (GrandChild inherits from Child which inherits from Parent)
        public class GrandChildEqualsTest : EqualityTestCase
        {
            public override object Factory1() => new GrandChild { Ints = new[] { 1, 2, 3 } };
            public override bool EqualsOperator(object value1, object value2) => (GrandChild)value1 == (GrandChild)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (GrandChild)value1 != (GrandChild)value2;
        }

        // Tests that order matters for GrandChild (inherits OrderedEquality)
        public class GrandChildOrderMattersTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new GrandChild { Ints = new[] { 1, 2, 3 } };
            public override object Factory2() => new GrandChild { Ints = new[] { 3, 2, 1 } };
            public override bool EqualsOperator(object value1, object value2) => (GrandChild)value1 == (GrandChild)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (GrandChild)value1 != (GrandChild)value2;
        }
    }
}
