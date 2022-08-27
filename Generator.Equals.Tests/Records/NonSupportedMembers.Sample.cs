namespace Generator.Equals.Tests.Records
{
    public partial class NonSupportedMembers
    {
        [Equatable]
        public partial record Sample(string Name)
        {
            public static int StaticProperty { get; }

            public int this[int index] => index;
        }
    }
}