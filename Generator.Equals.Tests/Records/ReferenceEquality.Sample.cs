namespace Generator.Equals.Tests.Records
{
    public partial class ReferenceEquality
    {
        [Equatable]
        public partial record Sample([property: ReferenceEquality] string Name);
    }
}