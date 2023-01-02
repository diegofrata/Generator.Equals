namespace Generator.Equals.Tests.Classes
{
    public partial class ExplicitMode
    {
        [Equatable(Explicit = true)]
        public partial class Sample
        {
            bool _flag;
            
            public Sample(string name, int age, bool flag)
            {
                _flag = flag;
                Name = name;
                Age = age;
            }

            public string Name { get; }
        
            [DefaultEquality]
            public int Age { get; }
        }
    }
}