namespace Generator.Equals.Tests.Structs
{
    public partial class NonSupportedMembers
    {
        [Equatable]
        public partial struct Sample
        {
            public Sample(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public static int StaticProperty { get; }

            public int this[int index] => index;
        }
    }
}