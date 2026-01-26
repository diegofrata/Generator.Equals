using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.RecordStructs;

/// <summary>
/// Tests for [Equatable(Explicit = true)] record struct mode where only marked properties are compared.
/// </summary>
public partial class ExplicitModeTests : SnapshotTestBase
{
    [Equatable(Explicit = true)]
    public partial record struct Sample(string Name, [property: DefaultEquality] int Age);

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

        namespace Generator.Equals.Tests.V3.RecordStructs;

        [Equatable(Explicit = true)]
        public partial record struct ExplicitModeSample(string Name, [property: DefaultEquality] int Age);
        """;
}
