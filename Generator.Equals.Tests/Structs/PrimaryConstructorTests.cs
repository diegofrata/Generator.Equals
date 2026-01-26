using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for C# 12 primary constructor support in structs.
/// </summary>
public partial class PrimaryConstructorTests : SnapshotTestBase
{
    // Primary constructor with explicit property declarations
    [Equatable]
    public partial struct Sample(string name, int age)
    {
        public string Name { get; } = name;
        public int Age { get; } = age;
    }

    public static TheoryData<Sample, Sample, bool> Cases => new()
    {
        // Same Name and Age
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
        // Different Name
        { new Sample("Dave", 35), new Sample("John", 35), false },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.Structs;

        [Equatable]
        public partial struct PrimaryCtorSample(string name, int age)
        {
            public string Name { get; } = name;
            public int Age { get; } = age;
        }
        """;
}
