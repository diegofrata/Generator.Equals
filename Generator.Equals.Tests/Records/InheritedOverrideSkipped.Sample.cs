namespace Generator.Equals.Tests.Records
{
    public partial class InheritedOverrideSkipped
    {
        // Base record with OrderedEquality
        [Equatable]
        public abstract partial record Parent
        {
            [OrderedEquality]
            public virtual int[] Ints { get; init; } = null!;
        }

        // Child record with IgnoreInheritedMembers = false (default)
        // The override should be skipped - base.Equals() handles comparison
        [Equatable]
        public partial record Child : Parent
        {
            public override int[] Ints { get; init; } = null!;
        }
    }
}
