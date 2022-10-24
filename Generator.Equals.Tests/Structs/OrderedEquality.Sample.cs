namespace Generator.Equals.Tests.Structs
{
    public partial class OrderedEquality
    {
        [Equatable]
        public partial struct Sample
        {
            public Sample(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }
    }
}