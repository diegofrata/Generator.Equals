namespace Generator.Equals.Tests.RecordsStructs
{
    public partial class FieldEquality
    {
        [Equatable]
        public partial record struct Sample
        {
            public Sample(string[] addresses)
            {
                _addresses = addresses;
            }
            
            [OrderedEquality] readonly string[] _addresses;
        };
    }
}