namespace Generator.Equals.Tests.Classes
{
    public partial class SealedClass
    {
        [Equatable]
        public sealed partial class Sample
        {
            public Sample(string[] addresses)
            {
                Addresses = addresses;
            }

            [OrderedEquality] public string[] Addresses { get; }
        }
    }
}