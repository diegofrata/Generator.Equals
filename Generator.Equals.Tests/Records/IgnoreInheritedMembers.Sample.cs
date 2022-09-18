namespace Generator.Equals.Tests.Records
{
    public partial class IgnoreInheritedMembers
    {
        public partial record SampleBase(string Name);

        [Equatable(Explicit = true, IgnoreInheritedMembers = true)]
        public partial record Sample(string Name, [property: DefaultEquality]int Age) : SampleBase(Name);
    }
}