namespace Generator.Equals.Tests.Structs
{
    public partial class IgnoreEquality
    {
        [Equatable]
        public partial struct Sample
        {
            public Sample(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }

            [IgnoreEquality] public int Age { get; }
        }
    }
}