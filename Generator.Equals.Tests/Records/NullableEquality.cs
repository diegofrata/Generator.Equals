using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class NullableEquality : EqualityTestCase
    {
        [Equatable]
        public partial record Data
        {
            [OrderedEquality] public string[]? Addresses { get; init; }
        }

        public override object Factory1() => new Data();
        public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
    }
}