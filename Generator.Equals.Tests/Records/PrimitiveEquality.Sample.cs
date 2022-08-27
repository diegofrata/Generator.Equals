namespace Generator.Equals.Tests.Records
{
    public partial class PrimitiveEquality
    {
        [Equatable]
        public partial record Sample(string Name, int Age);
    }
}