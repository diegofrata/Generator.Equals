namespace Generator.Equals.Tests.Structs
{
    public partial class GenericParameterEquality
    {
        [Equatable]
        public partial struct Sample<TName, TAge>
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