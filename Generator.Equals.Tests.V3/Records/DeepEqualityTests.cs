using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Records;

/// <summary>
/// Tests for nested [Equatable] records (deep equality).
/// </summary>
public partial class DeepEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample(Person Person);

    [Equatable]
    public partial record Person(int Age);

    [Equatable]
    public partial record Manager(int Age, string Department) : Person(Age);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same nested Person
        { new Sample(new Person(25)), new Sample(new Person(25)), true },
        // Different Age
        { new Sample(new Person(25)), new Sample(new Person(30)), false },
        // Same Manager
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(25, "IT")), true },
        // Different Manager Department
        { new Sample(new Manager(25, "IT")), new Sample(new Manager(25, "Sales")), false },
        // Person vs Manager with same Age (different types)
        { new Sample(new Person(25)), new Sample(new Manager(25, "IT")), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Records;

        [Equatable]
        public partial record DeepEqualitySample(DeepEqualityPerson Person);

        [Equatable]
        public partial record DeepEqualityPerson(int Age);

        [Equatable]
        public partial record DeepEqualityManager(int Age, string Department) : DeepEqualityPerson(Age);
        """;
}
