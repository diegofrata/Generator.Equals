using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [Equatable] generic record struct.
/// </summary>
public partial class GenericParameterEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct Sample<TName, TAge>(TName Name, TAge Age);

    public static TheoryData<Sample<string, int>, Sample<string, int>, bool> EqualityCases => new()
    {
        // Same values
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("Dave", 35), true },
        // Different Name
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("John", 35), false },
        // Different Age
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("Dave", 40), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample<string, int> a, Sample<string, int> b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.RecordStructs;

                                [Equatable]
                                public partial record struct GenericParameterEqualitySample<TName, TAge>(TName Name, TAge Age);
                                """;
}
