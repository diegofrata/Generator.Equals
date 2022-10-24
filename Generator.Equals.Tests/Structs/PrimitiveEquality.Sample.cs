namespace Generator.Equals.Tests.Structs
{
    public partial class PrimitiveEquality
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
            public int Age { get; }
        }
    }
}