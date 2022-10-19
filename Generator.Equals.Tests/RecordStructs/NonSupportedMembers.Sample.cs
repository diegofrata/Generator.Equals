namespace Generator.Equals.Tests.RecordStructs
{
    public partial class NonSupportedMembers
    {
        [Equatable]
        public partial record struct Sample(string Name)
        {
            public static int StaticProperty { get; }

            public int this[int index] => index;
        }
    }
}