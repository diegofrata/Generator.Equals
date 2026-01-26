using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [Equatable(Explicit = true)] struct mode where only marked properties are compared.
/// </summary>
public partial class ExplicitModeTests : SnapshotTestBase
{
    [Equatable(Explicit = true)]
    public partial struct Sample
    {
        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        [DefaultEquality] public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Age (Name is ignored in explicit mode)
        { new Sample("Dave", 35), new Sample("John", 35), true },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
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

        [Equatable(Explicit = true)]
        public partial struct ExplicitModeSample
        {
            public ExplicitModeSample(string name, int age)
            {
                Name = name;
                Age = age;
            }

            public string Name { get; }
            [DefaultEquality] public int Age { get; }
        }
        """;
}
