namespace Generator.Equals.Tests.Classes
{
    public partial class InheritedOverrideSkipped
    {
        // Base class with [Equatable] and OrderedEquality
        [Equatable]
        public abstract partial class Parent
        {
            [OrderedEquality]
            public virtual int[] Ints { get; set; } = null!;
        }

        // Child class with [Equatable] - override should be skipped
        // because Parent has [Equatable] and handles it via base.Equals()
        [Equatable]
        public partial class Child : Parent
        {
            public override int[] Ints { get; set; } = null!;
        }
    }
}
