using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] sealed record.
/// </summary>
public partial class SealedRecordTests : SnapshotTestBase
{
    [Equatable]
    public sealed partial record Sample([property: OrderedEquality] string[] Addresses);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content
        { new Sample(new[] { "10 Some Street" }), new Sample(new[] { "10 Some Street" }), true },
        // Different content
        { new Sample(new[] { "10 Some Street" }), new Sample(new[] { "11 Some Street" }), false },
        // Same content, different order
        { new Sample(new[] { "A", "B" }), new Sample(new[] { "B", "A" }), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public sealed partial record SealedRecordSample([property: OrderedEquality] string[] Addresses);
                                """;
}
