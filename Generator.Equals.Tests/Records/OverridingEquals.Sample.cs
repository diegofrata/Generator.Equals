using System;

namespace Generator.Equals.Tests.Records
{
    public partial class OverridingEquals
    {
        [Equatable]
        public partial record Person(int Age);

        public record Manager(int Age, string Department) : Person(Age)
        {
            public virtual bool Equals(Manager? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return base.Equals(other) && Department == other.Department;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(base.GetHashCode(), Department);
            }
        }

        [Equatable]
        public partial record SeniorManager(int Age, string Department, int Shares) : Manager(Age, Department);
    }
}