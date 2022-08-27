namespace Generator.Equals.Tests.Classes
{
    public partial class NonSupportedMembers
    {
        [Equatable]
        public partial class Sample
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