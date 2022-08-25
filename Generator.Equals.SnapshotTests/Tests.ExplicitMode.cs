namespace Generator.Equals.SnapshotTests;

public partial class Tests
{
    [TestMethod]
    public Task ExplicitMode()
    {
        return CheckSourceAsync(@"
[Equatable(Explicit = true)]
partial class MyClass
{
    [DefaultEquality] 
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}");
    }
}