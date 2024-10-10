using System;

namespace Generator.Equals.Tests.Classes;

public partial class StringArrayEquality
{
    [Equatable]
    public partial class Sample
    {
        [UnorderedEquality, StringEquality(StringComparison.OrdinalIgnoreCase)]
        public string[] Tags { get; set; }
    }
}

public partial class StringArrayEquality
{
    public class EqualsTests : EqualityTestCase
    {
        public override object Factory1()
        {
            return new Sample
            {
                Tags = new[] { "a", "b", "c" }
            };
        }

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }
    
    // Order doesnt matter
    public class OrderDoesntMatterEqualsTest : EqualityTestCase
    {
        public override bool Expected => true;

        public override object Factory1() => new Sample
        {
            Tags = new[] { "a", "b", "c" }
        };

        public override object Factory2() => new Sample
        {
            Tags = new[] { "c", "b", "a" }
        };

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }
}