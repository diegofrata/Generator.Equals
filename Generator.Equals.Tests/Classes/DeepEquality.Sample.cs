namespace Generator.Equals.Tests.Classes
{
    public partial class DeepEquality
    {
        [Equatable]
        public partial class Sample
        {
            public Sample(Person person)
            {
                Person = person;
            }

            public Person Person { get; }
        }

        [Equatable]
        public partial class Person
        {
            protected Person(int age)
            {
                Age = age;
            }
            
            public int Age { get; }
        }

        [Equatable]
        public partial class Manager : Person
        {
            public Manager(int age, string department) : base(age)
            {
                Department = department;
            }

            public string Department { get; }
        }
    }
}