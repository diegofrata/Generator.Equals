namespace Generator.Equals.Tests.Structs
{
    public partial class DefaultBehavior
    {
        public struct Sample
        {
            public Sample(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
            public int Age { get; }
        }
    }
}