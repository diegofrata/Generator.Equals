using NUnit.Framework;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class MultiplePartialsEquality : EqualityTestCase
    {
        [Equatable]
        public partial class Data
        {
            [OrderedEquality] public string[]? Addresses { get; set; }
        }
        public partial class Data
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
        }
        public partial class Data
        {
            [IgnoreEquality] public int Age { get; set; }
        }

        public override object Factory1() => new Data() { FirstName = "Dave", Age = 35, Addresses = new[] { "10 Downing St", "Bricklane" } };
        public override object Factory2() => new Data() { FirstName = "Dave", Age = 42, Addresses = new[] { "10 Downing St", "Bricklane" } };
        public override bool EqualsOperator(object value1, object value2) => (Data)value1 == (Data)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Data)value1 != (Data)value2;
    }
}