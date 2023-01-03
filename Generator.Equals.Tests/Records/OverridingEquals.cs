namespace Generator.Equals.Tests.Records
{
    public partial class OverridingEquals
    {
        public class EqualsTest : EqualityTestCase<Person>
        {
            public override object Factory1() => new SeniorManager(25, "IT", 1000);
            public override bool EqualsOperator(object value1, object value2) => (SeniorManager) value1 == (SeniorManager) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (SeniorManager) value1 != (SeniorManager) value2;
        }
        
        public class NotEqualsTest : EqualityTestCase<Person>
        {
            public override bool Expected => false;
            public override object Factory1() => new SeniorManager(25, "IT", 1000);
            public override object Factory2() => new SeniorManager(25, "IT", 2000);
            public override bool EqualsOperator(object value1, object value2) => (SeniorManager) value1 == (Person) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (SeniorManager) value1 != (Person) value2;
        }
    }
}