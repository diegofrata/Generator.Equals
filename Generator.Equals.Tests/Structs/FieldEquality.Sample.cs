namespace Generator.Equals.Tests.Structs
{
    public partial class FieldEquality
    {
        [Equatable]
        public partial struct Sample
        {
            public Sample(string[] addresses)
            {
                _addresses = addresses;
            }
            
            [OrderedEquality] readonly string[] _addresses;
        };
    }
}