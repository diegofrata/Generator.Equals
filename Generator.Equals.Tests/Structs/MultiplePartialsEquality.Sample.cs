namespace Generator.Equals.Tests.Structs
{
    public partial class MultiplePartialsEquality
    {
#pragma warning disable 0282
        [Equatable]
        public partial struct Sample
        {
            [OrderedEquality] public string[]? Addresses { get; set; }
        }

        public partial struct Sample
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
        }

        public partial struct Sample
        {
            public Sample()
            {
                Age = 0;
                Addresses = new string[] { };
            }

            [IgnoreEquality] public int Age { get; set; }
        }
#pragma warning restore 0282 
    }
}