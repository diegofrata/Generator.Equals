namespace Generator.Equals.Tests.Classes
{
    public partial class BaseEquality
    {
        [Equatable]
        public partial class Person
        {
            public Person(int age)
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