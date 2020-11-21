using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class ReferenceEquality
    {
        [Equatable]
        public partial record Data([property: ReferenceEquality] string Name);

        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data(string.Intern($"Dave{35}"));
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
        
        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data($"Dave{35}");
            public override object Factory2() => new Data($"Dave{35}");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}