namespace Generator.Equals.Tests.Classes
{
    public partial class ReferenceEquality
    {
        [Equatable]
        public partial class Sample
        {
            public Sample(string name)
            {
                Name = name;
            }

            [ReferenceEquality] public string Name { get; }
        }
    }
}