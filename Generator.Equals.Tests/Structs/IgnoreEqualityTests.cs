using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [IgnoreEquality] attribute on struct properties.
/// </summary>
public partial class IgnoreEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
    {
        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        [IgnoreEquality] public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Name, different Age (Age is ignored)
        { new Sample("Dave", 35), new Sample("Dave", 85), true },
        // Different Name (not ignored)
        { new Sample("Dave", 35), new Sample("John", 35), false },
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

        namespace Generator.Equals.Tests.Structs;

        [Equatable]
        public partial struct IgnoreEqualitySample
        {
            public IgnoreEqualitySample(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
            [IgnoreEquality] public int Age { get; }
        }
        """;
}
