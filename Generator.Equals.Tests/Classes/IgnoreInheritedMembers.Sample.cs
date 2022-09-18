namespace Generator.Equals.Tests.Classes
{
    public partial class IgnoreInheritedMembers
    {
        public class SampleBase
        {
            public SampleBase(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        [Equatable(Explicit = true, IgnoreInheritedMembers = true)]
        public partial class Sample : SampleBase
        {
            public Sample(string name, int age) : base(name)
            {
                Age = age;
            }

            [DefaultEquality]
            public int Age { get; }
        }
    }
}