namespace Generator.Equals.Tests.RecordStructs
{
    public partial class GenericParameterEquality
    {
        [Equatable]
        public partial record struct Sample<TName, TAge>(TName Name, TAge Age);
    }
}