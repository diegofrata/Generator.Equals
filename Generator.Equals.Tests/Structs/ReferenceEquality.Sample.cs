namespace Generator.Equals.Tests.Structs
{
    public partial class ReferenceEquality
    {
        [Equatable]
        public partial struct Sample
        {
            public Sample(string name)
            {
                Name = name;
            }

            [ReferenceEquality] public string Name { get; }
        }
    }
}