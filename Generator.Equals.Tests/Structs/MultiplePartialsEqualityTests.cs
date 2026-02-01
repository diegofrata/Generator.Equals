using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [Equatable] struct split across multiple partial declarations.
/// </summary>
public partial class MultiplePartialsEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        [OrderedEquality] public string[]? Addresses { get; init; }
    }

    public partial struct Sample
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }

    public partial struct Sample
    {
        [IgnoreEquality] public int Age { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        {
            new Sample { Addresses = new[] { "A" }, FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = new[] { "A" }, FirstName = "John", LastName = "Doe", Age = 30 },
            true
        },
        // Same values, different Age (ignored)
        {
            new Sample { Addresses = new[] { "A" }, FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = new[] { "A" }, FirstName = "John", LastName = "Doe", Age = 40 },
            true
        },
        // Different FirstName
        {
            new Sample { Addresses = new[] { "A" }, FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = new[] { "A" }, FirstName = "Jane", LastName = "Doe", Age = 30 },
            false
        },
        // Different Addresses
        {
            new Sample { Addresses = new[] { "A" }, FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = new[] { "B" }, FirstName = "John", LastName = "Doe", Age = 30 },
            false
        },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct MultiplePartialsEqualitySample
                                {
                                    [OrderedEquality] public string[]? Addresses { get; init; }
                                }

                                public partial struct MultiplePartialsEqualitySample
                                {
                                    public string FirstName { get; init; }
                                    public string LastName { get; init; }
                                }

                                public partial struct MultiplePartialsEqualitySample
                                {
                                    [IgnoreEquality] public int Age { get; init; }
                                }
                                """;
}
