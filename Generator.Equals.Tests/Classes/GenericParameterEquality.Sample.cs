namespace Generator.Equals.Tests.Classes
{
    public partial class GenericParameterEquality
    {
        [Equatable]
        public partial class Sample<TName, TAge>
        {
            public Sample(TName name, TAge age)
            {
                Name = name;
                Age = age;
            }

            public TName Name { get; }
            public TAge Age { get; }
        }
    }
}