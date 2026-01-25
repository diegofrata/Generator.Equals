namespace Generator.Equals.Tests.Records
{
    public partial class InheritedFromNonEquatable
    {
        // Base record WITHOUT [Equatable] - just a regular record with an attribute hint
        public abstract record Parent
        {
            [OrderedEquality]
            public virtual int[] Ints { get; init; } = null!;
        }

        // Child record WITH [Equatable] - should include Ints with inherited [OrderedEquality]
        // because Parent doesn't have [Equatable], so base.Equals() won't handle it
        [Equatable]
        public partial record Child : Parent
        {
            public override int[] Ints { get; init; } = null!;
        }
    }
}
