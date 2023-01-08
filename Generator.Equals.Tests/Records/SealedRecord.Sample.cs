namespace Generator.Equals.Tests.Records
{
    public partial class SealedRecord
    {
        [Equatable]
        public sealed partial record Sample([property: OrderedEquality] string[] Addresses);
    }
}