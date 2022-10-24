namespace Generator.Equals.Tests.RecordStructs
{
    public partial class ReferenceEquality
    {
        [Equatable]
        public partial record struct Sample([property: ReferenceEquality] string Name);
    }
}