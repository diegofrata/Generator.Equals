using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Classes;

/// <summary>
/// Tests for basic primitive type equality comparison.
/// </summary>
public partial class PrimitiveEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values should be equal
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
        // Different values should not be equal
        { new Sample("Dave", 35), new Sample("Joe", 77), false },
        // Same name, different age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
        // Different name, same age
        { new Sample("Dave", 35), new Sample("Joe", 35), false },
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

        namespace Generator.Equals.Tests.V3.Classes;

        [Equatable]
        public partial class PrimitiveEqualitySample
        {
            public PrimitiveEqualitySample(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
            public int Age { get; }
        }
        """;
}
