namespace Generator.Equals.Tests.RecordStructs
{
    public partial class NullableEquality
    {
        [Equatable]
        public partial record struct Sample
        {
            [OrderedEquality] public string[]? Addresses { get; init; }
        }
    }
}