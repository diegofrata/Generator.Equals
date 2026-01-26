using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Records;

/// <summary>
/// Tests for equality in record inheritance hierarchy.
/// </summary>
public partial class BaseEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Person(int Age);

    [Equatable]
    public partial record Manager(int Age, string Department) : Person(Age);

    public static TheoryData<Manager, Manager, bool> EqualityCases => new()
    {
        // Same values
        { new Manager(25, "IT"), new Manager(25, "IT"), true },
        // Different Department
        { new Manager(25, "IT"), new Manager(25, "Sales"), false },
        // Different Age
        { new Manager(25, "IT"), new Manager(30, "IT"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Manager a, Manager b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Records;

        [Equatable]
        public partial record BaseEqualityPerson(int Age);

        [Equatable]
        public partial record BaseEqualityManager(int Age, string Department) : BaseEqualityPerson(Age);
        """;
}
