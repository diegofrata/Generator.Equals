using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Records;

/// <summary>
/// Tests for [Equatable(SkipBaseEquals = true)] record.
/// base.Equals() is not called, so inherited members are not compared.
/// </summary>
public partial class SkipBaseEqualsTests : SnapshotTestBase
{
    public partial record SampleBase(string Name);

    [Equatable(Explicit = true, SkipBaseEquals = true)]
    public partial record Sample(string Name, [property: DefaultEquality] int Age) : SampleBase(Name);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Age (Name is ignored because base.Equals() is skipped + Explicit mode)
        { new Sample("Dave", 35), new Sample("John", 35), true },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
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

        namespace Generator.Equals.Tests.V3.Records;

        public partial record SkipBaseEqualsSampleBase(string Name);

        [Equatable(Explicit = true, SkipBaseEquals = true)]
        public partial record SkipBaseEqualsSample(string Name, [property: DefaultEquality] int Age) : SkipBaseEqualsSampleBase(Name);
        """;
}
