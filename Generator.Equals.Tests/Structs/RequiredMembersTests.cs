using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for C# 11 required members support in structs.
/// </summary>
public partial class RequiredMembersTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
        public string? Optional { get; set; }
    }

    public static TheoryData<Sample, Sample, bool> Cases => new()
    {
        // Same required members
        {
            new Sample { Name = "Dave", Age = 35 },
            new Sample { Name = "Dave", Age = 35 },
            true
        },
        // Different Name (required)
        {
            new Sample { Name = "Dave", Age = 35 },
            new Sample { Name = "John", Age = 35 },
            false
        },
        // Different Age (required)
        {
            new Sample { Name = "Dave", Age = 35 },
            new Sample { Name = "Dave", Age = 40 },
            false
        },
        // Same required, different optional
        {
            new Sample { Name = "Dave", Age = 35, Optional = "A" },
            new Sample { Name = "Dave", Age = 35, Optional = "B" },
            false
        },
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
        public partial struct RequiredMembersSample
        {
            public required string Name { get; set; }
            public required int Age { get; set; }
            public string? Optional { get; set; }
        }
        """;
}
