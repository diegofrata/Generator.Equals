namespace Generator.Equals.Tests.Records
{
    public partial class GenericParameterEquality
    {
        [Equatable]
        public partial record Sample<TName, TAge>(TName Name, TAge Age);
    }
}