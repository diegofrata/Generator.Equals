namespace H.Generators.SnapshotTests;

public partial class Tests
{
    [TestMethod]
    public Task ReadmeClass()
    {
        return CheckSourceAsync(@"
[Equatable]
partial class MyClass
{
    [OrderedEquality] 
    public string[]? Fruits { get; set; }
}");
    }

    [TestMethod]
    public Task ReadmeRecord()
    {
        return CheckSourceAsync(@"
[Equatable]
partial record MyRecord(
    [property: OrderedEquality] string[]? Fruits
);");
    }
}