namespace Generator.Equals.Tests.RecordStructs
{
    public partial class PrimitiveEquality
    {
        [Equatable]
        public partial record struct Sample(string Name, int Age);
    }
}
