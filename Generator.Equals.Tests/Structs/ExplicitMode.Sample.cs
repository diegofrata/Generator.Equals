namespace Generator.Equals.Tests.Structs
{
    public partial class ExplicitMode
    {
        [Equatable(Explicit = true)]
        public partial struct Sample
        {
            public Sample(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
        
            [DefaultEquality]
            public int Age { get; }
        }
    }
}