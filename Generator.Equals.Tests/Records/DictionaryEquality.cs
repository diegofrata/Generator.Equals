using NUnit.Framework;

namespace Generator.Equals.Tests.Records
{
    [TestFixture]
    public partial class DictionaryEquality
    {
        [Equatable]
        public partial record Data
        {
            [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
        }

        [TestFixture]
        public class EqualsTests : EqualityTestCase
        {
            public override object Factory1()
            {
                var randomSort = new Random();

                // This should generate objects with the same contents, but different orders, thus proving
                // that dictionary equality is being used instead of the normal sequence equality.
                return new Data
                {
                    Properties = Enumerable
                        .Range(1, 1000)
                        .OrderBy(x => randomSort.NextDouble())
                        .ToDictionary(x => x.ToString(), x => x)
                };
            }

            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public partial class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;

            public override object Factory1() => new Data
            {
                Properties = Enumerable.Range(1, 1000).ToDictionary(x => x.ToString(), x => x)
            };

            public override object Factory2() => new Data
            {
                Properties = Enumerable.Range(2, 999).ToDictionary(x => x.ToString(), x => x)
            };

            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
    }
}