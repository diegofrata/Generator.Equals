namespace Generator.Equals.Tests.Classes
{
    public partial class InheritedEqualityAttributes
    {
        // Base class with OrderedEquality on a virtual property
        [Equatable(IgnoreInheritedMembers = true)]
        public abstract partial class Parent
        {
            [OrderedEquality]
            public virtual int[] Ints { get; set; } = null!;
        }

        // Child class that overrides the property - should inherit [OrderedEquality]
        [Equatable(IgnoreInheritedMembers = true)]
        public partial class Child : Parent
        {
            public override int[] Ints { get; set; } = null!;
        }

        // Child that overrides with its own attribute (should use child's attribute)
        [Equatable(IgnoreInheritedMembers = true)]
        public partial class ChildWithOwnAttribute : Parent
        {
            [UnorderedEquality]
            public override int[] Ints { get; set; } = null!;
        }
    }
}
