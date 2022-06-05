using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class MultiplePartialsEquality : EqualityTestCase
    {
        [Equatable]
        public partial record Data
        {
            [OrderedEquality] public string[]? Addresses { get; init; }
        }
        public partial record Data
        {
            public string FirstName { get; init; } = string.Empty;
            public string LastName { get; init; } = string.Empty;
        }
        public partial record Data
        {
            [IgnoreEquality] public int Age { get; init; }
        }

        public override object Factory1() => new Data() { FirstName = "Dave", Age = 35, Addresses = new[] { "10 Downing St", "Bricklane" } };
        public override object Factory2() => new Data() { FirstName = "Dave", Age = 42, Addresses = new[] { "10 Downing St", "Bricklane" } };
        public override bool EqualsOperator(object value1, object value2) => (Data)value1 == (Data)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Data)value1 != (Data)value2;
    }
}