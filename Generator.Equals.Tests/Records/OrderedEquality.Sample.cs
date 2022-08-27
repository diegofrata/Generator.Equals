namespace Generator.Equals.Tests.Records
{
    public partial class OrderedEquality
    {
        [Equatable]
        public partial record Sample([property: OrderedEquality] string[] Addresses);
    }
}