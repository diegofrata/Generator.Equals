using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class ObsoleteRecord
    {
        [Equatable]
        [Obsolete("Make sure the obsolete on the object model does not add warnings")]
        public partial record Data(string Name);

#pragma warning disable CS0618
        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data("Dave");
            public override object Factory2() => new Data("Dave");
            public override bool EqualsOperator(object value1, object value2) => (Data)value1 == (Data)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data)value1 != (Data)value2;
        }

        [TestFixture]
        public class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data("Dave");
            public override object Factory2() => new Data("John");
            public override bool EqualsOperator(object value1, object value2) => (Data)value1 == (Data)value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data)value1 != (Data)value2;
        }
#pragma warning restore CS0618
    }
}
