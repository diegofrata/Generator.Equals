namespace Generator.Equals.Tests.Records
{
    public partial class FieldEquality
    {
        [Equatable]
        public partial record Sample
        {
            public Sample(string[] addresses)
            {
                _addresses = addresses;
            }
            
            [OrderedEquality] readonly string[] _addresses;
        };
    }
}