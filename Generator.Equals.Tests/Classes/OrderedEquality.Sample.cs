namespace Generator.Equals.Tests.Classes
{
    public partial class OrderedEquality
    {
        [Equatable]
        public partial class Sample
        {
            public Sample(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }
    }
}