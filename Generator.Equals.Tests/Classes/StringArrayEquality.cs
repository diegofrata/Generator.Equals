using System;

namespace Generator.Equals.Tests.Classes;

public partial class StringArrayEquality
{
    [Equatable]
    public partial class Sample
    {
        // Order doesnt matter, case doesn't matter
        [UnorderedEquality, StringEquality(StringComparison.OrdinalIgnoreCase)]
        public string[] Unordered_Case_Insensitive { get; set; }

        // Order matters, case doesn't matter
        [OrderedEquality, StringEquality(StringComparison.OrdinalIgnoreCase)]
        public string[] Ordered_Case_Insensitive { get; set; }

        // Order doesn't matter, case matters
        [UnorderedEquality, StringEquality(StringComparison.Ordinal)]
        public string[] Unordered_Case_Sensitive { get; set; }

        // Order matters, case matters
        [OrderedEquality, StringEquality(StringComparison.Ordinal)]
        public string[] Ordered_Case_Sensitive { get; set; }

        // Default expectation: Order matters, case matters
        public string[] DefaultBehaviour { get; set; }
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
                Unordered_Case_Insensitive = new[] { "a", "b", "c" },
                Ordered_Case_Insensitive = new[] { "a", "b", "c" },
                Unordered_Case_Sensitive = new[] { "a", "b", "c" },
                Ordered_Case_Sensitive = new[] { "a", "b", "c" },
                DefaultBehaviour = new[] { "a", "b", "c" },
            };
        }

        public override object Factory2()
        {
            return new Sample
            {
                Unordered_Case_Insensitive = new[] { "b", "A", "c" },
                Ordered_Case_Insensitive = new[] { "A", "b", "c" },
                Unordered_Case_Sensitive = new[] { "b", "a", "c" },
                Ordered_Case_Sensitive = new[] { "a", "b", "c" },
                DefaultBehaviour = new[] { "a", "b", "c" },
            };
        }

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }

    // Now test if the equals operator can return false.
    // Unordered_Case_Insensitive: "a", "b", "c" vs "b", "X", "c" (X is not in the first array)
    public class NotEqualsTest_Unordered_Case_Insensitive : EqualityTestCase
    {
        public override bool Expected => false;

        public override object Factory1() => new Sample
        {
            Unordered_Case_Insensitive = new[] { "a", "b", "c" },
        };

        public override object Factory2() => new Sample
        {
            Unordered_Case_Insensitive = new[] { "b", "X", "c" },
        };

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }

    // Ordered_Case_Insensitive: "a", "b", "c" vs "A", "b", "c" (a is different case)
    public class NotEqualsTest_Ordered_Case_Insensitive : EqualityTestCase
    {
        public override bool Expected => false;

        public override object Factory1() => new Sample
        {
            Ordered_Case_Insensitive = new[] { "a", "b", "c" },
        };

        public override object Factory2() => new Sample
        {
            Ordered_Case_Insensitive = new[] { "b", "A", "c" },
        };

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }

    // Unordered_Case_Sensitive: "a", "b", "c" vs "b", "a", "C" (C is different case)
    public class NotEqualsTest_Unordered_Case_Sensitive : EqualityTestCase
    {
        public override bool Expected => false;

        public override object Factory1() => new Sample
        {
            Unordered_Case_Sensitive = new[] { "a", "b", "c" },
        };

        public override object Factory2() => new Sample
        {
            Unordered_Case_Sensitive = new[] { "b", "a", "C" },
        };

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }

    // Ordered_Case_Sensitive: "a", "b", "c" vs "b", "a", "C" (C is different case)
    public class NotEqualsTest_Ordered_Case_Sensitive : EqualityTestCase
    {
        public override bool Expected => false;

        public override object Factory1() => new Sample
        {
            Ordered_Case_Sensitive = new[] { "a", "b", "c" },
        };

        public override object Factory2() => new Sample
        {
            Ordered_Case_Sensitive = new[] { "b", "a", "C" },
        };

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }

    // DefaultBehaviour: "a", "b", "c" vs "b", "a", "C" (C is different case)
    public class NotEqualsTest_DefaultBehaviour : EqualityTestCase
    {
        public override bool Expected => false;

        public override object Factory1() => new Sample
        {
            DefaultBehaviour = new[] { "a", "b", "c" },
        };

        public override object Factory2() => new Sample
        {
            DefaultBehaviour = new[] { "b", "a", "C" },
        };

        public override bool EqualsOperator(object value1, object value2) => (Sample)value1 == (Sample)value2;
        public override bool NotEqualsOperator(object value1, object value2) => (Sample)value1 != (Sample)value2;
    }
}