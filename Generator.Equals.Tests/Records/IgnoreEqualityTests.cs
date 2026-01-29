using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [IgnoreEquality] attribute on record properties.
/// </summary>
public partial class IgnoreEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample(string Name, [property: IgnoreEquality] int Age);

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
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record IgnoreEqualitySample(string Name, [property: IgnoreEquality] int Age);
                                """;
}
