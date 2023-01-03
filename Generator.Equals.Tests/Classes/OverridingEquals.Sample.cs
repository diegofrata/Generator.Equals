using System;

namespace Generator.Equals.Tests.Classes
{
    public partial class OverridingEquals
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

        public class Manager : Person, IEquatable<Manager>
        {
            public Manager(int age, string department) : base(age)
            {
                Department = department;
            }

            public string Department { get; }

            public bool Equals(Manager? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return base.Equals(other) && Department == other.Department;
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Manager)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(base.GetHashCode(), Department);
            }
        }

        [Equatable]
        public partial class SeniorManager : Manager
        {
            public SeniorManager(int age, string department, int shares) : base(age, department)
            {
                Shares = shares;
            }

            public int Shares { get; }
        }
    }
}