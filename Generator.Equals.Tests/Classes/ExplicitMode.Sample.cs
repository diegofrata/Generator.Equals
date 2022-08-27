namespace Generator.Equals.Tests.Classes;

public partial class ExplicitMode
{
    [Equatable(Explicit = true)]
    public partial class Sample
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