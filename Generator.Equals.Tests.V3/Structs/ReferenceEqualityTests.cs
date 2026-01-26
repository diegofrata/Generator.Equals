using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Structs;

/// <summary>
/// Tests for [ReferenceEquality] attribute on struct property.
/// </summary>
public partial class ReferenceEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        public Sample(string name)
        {
            Name = name;
        }

        [ReferenceEquality] public string Name { get; }
    }

    private static readonly string SharedName = "Dave";

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same reference
        { new Sample(SharedName), new Sample(SharedName), true },
        // Different references with same value (reference equality, so not equal)
        // Note: string literals are interned, so we need to create new strings
        { new Sample(new string("Dave".ToCharArray())), new Sample(new string("Dave".ToCharArray())), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Structs;

        [Equatable]
        public partial struct ReferenceEqualitySample
        {
            public ReferenceEqualitySample(string name)
            {
                Name = name;
            }

            [ReferenceEquality] public string Name { get; }
        }
        """;
}
