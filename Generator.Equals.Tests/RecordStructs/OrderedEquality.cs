namespace Generator.Equals.Tests.RecordStructs
{
    public partial class OrderedEquality
    {
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Sample(new[] {"10 Downing St"});
            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;
        }
        
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Sample(new[] {"10 Downing St"});
            public override object Factory2() => new Sample(new[] {"Bricklane"});
            public override bool EqualsOperator(object value1, object value2) => (Sample) value1 == (Sample) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample) value1 != (Sample) value2;
        }
    }
}