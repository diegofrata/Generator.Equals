namespace Generator.Equals.Tests.Records
{
    public partial class BaseEquality
    {
        [Equatable]
        public partial record Person(int Age);

        [Equatable]
        public partial record Manager(int Age, string Department) : Person(Age);
    }
}