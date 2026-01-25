namespace Generator.Equals.Tests.Records
{
    public partial class InheritedEqualityAttributes
    {
        // Base record with OrderedEquality on a virtual property
        [Equatable(IgnoreInheritedMembers = true)]
        public abstract partial record Parent
        {
            [OrderedEquality]
            public virtual int[] Ints { get; init; } = null!;
        }

        // Child record that overrides the property - should inherit [OrderedEquality]
        [Equatable(IgnoreInheritedMembers = true)]
        public partial record Child : Parent
        {
            public override int[] Ints { get; init; } = null!;
        }

        // Child that overrides with its own attribute (should use child's attribute)
        [Equatable(IgnoreInheritedMembers = true)]
        public partial record ChildWithOwnAttribute : Parent
        {
            [UnorderedEquality]
            public override int[] Ints { get; init; } = null!;
        }

        // Grandchild to test multi-level inheritance
        [Equatable(IgnoreInheritedMembers = true)]
        public partial record GrandChild : Child
        {
            public override int[] Ints { get; init; } = null!;
        }
    }
}
