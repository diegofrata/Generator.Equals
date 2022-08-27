namespace Generator.Equals.Tests.Classes
{
    public partial class NullableEquality
    {
        [Equatable]
        public partial class Sample
        {
            [OrderedEquality] public string[]? Addresses { get; set; }
        }
    }
}