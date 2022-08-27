namespace Generator.Equals.Tests.Records
{
    public partial class MultiplePartialsEquality
    {
        [Equatable]
        public partial record Sample
        {
            [OrderedEquality] public string[]? Addresses { get; init; }
        }
    
        public partial record Sample
        {
            public string FirstName { get; init; } = string.Empty;
            public string LastName { get; init; } = string.Empty;
        }
    
        public partial record Sample
        {
            [IgnoreEquality] public int Age { get; init; }
        }
    }
}