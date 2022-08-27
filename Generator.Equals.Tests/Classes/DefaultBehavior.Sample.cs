namespace Generator.Equals.Tests.Classes
{
    public partial class DefaultBehavior
    {
        public class Sample
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