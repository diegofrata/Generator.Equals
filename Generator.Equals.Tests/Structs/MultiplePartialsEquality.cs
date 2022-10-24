namespace Generator.Equals.Tests.Structs
{
    public partial class MultiplePartialsEquality : EqualityTestCase
    {
        public override object Factory1() => new Sample() { FirstName = "Dave", Age = 35, Addresses = new[] { "10 Downing St", "Bricklane" } };
        public override object Factory2() => new Sample() { FirstName = "Dave", Age = 42, Addresses = new[] { "10 Downing St", "Bricklane" } };
        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }
}