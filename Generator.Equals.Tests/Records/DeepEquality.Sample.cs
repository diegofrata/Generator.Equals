namespace Generator.Equals.Tests.Records
{
    public partial class DeepEquality
    {
        [Equatable]
        public partial record Sample(Person Person);

        [Equatable]
        public partial record Person(int Age);

        [Equatable]
        public partial record Manager(int Age, string Department) : Person(Age);
    }
}