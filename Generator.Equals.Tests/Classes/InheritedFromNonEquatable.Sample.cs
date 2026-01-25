namespace Generator.Equals.Tests.Classes
{
    public partial class InheritedFromNonEquatable
    {
        // Base class WITHOUT [Equatable] - just a regular class with an attribute hint
        public abstract class Parent
        {
            [OrderedEquality]
            public virtual int[] Ints { get; set; } = null!;
        }

        // Child class WITH [Equatable] - should include Ints with inherited [OrderedEquality]
        // because Parent doesn't have [Equatable], so base.Equals() won't handle it
        [Equatable]
        public partial class Child : Parent
        {
            public override int[] Ints { get; set; } = null!;
        }
    }
}
