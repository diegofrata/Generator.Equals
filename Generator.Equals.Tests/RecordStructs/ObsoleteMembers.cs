namespace Generator.Equals.Tests.RecordStructs
{
    public partial class ObsoleteMembers
    {
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Sample("Dave", "Smith");
            public override object Factory2() => new Sample("Dave", "Smith");
            public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
        }
        
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Sample("Dave", "West");
            public override object Factory2() => new Sample("John", "Smith");
            public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
        }
    }
}
