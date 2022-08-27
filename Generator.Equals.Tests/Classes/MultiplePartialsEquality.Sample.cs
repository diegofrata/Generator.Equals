namespace Generator.Equals.Tests.Classes
{
    public partial class MultiplePartialsEquality
    {
        [Equatable]
        public partial class Sample
        {
            [OrderedEquality] public string[]? Addresses { get; set; }
        }
        public partial class Sample
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
        }
        public partial class Sample
        {
            [IgnoreEquality] public int Age { get; set; }
        }

    }
}