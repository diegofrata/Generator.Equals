namespace Generator.Equals.Tests.Structs
{
    public partial class NullableEquality
    {
        [Equatable]
        public partial struct Sample
        {
            [OrderedEquality] public string[]? Addresses { get; set; }
        }
    }
}