using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [OrderedEquality] attribute on array property.
/// </summary>
public partial class OrderedEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct Sample([property: OrderedEquality] string[] Addresses);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same content
        { new Sample(["10 Some Street"]), new Sample(["10 Some Street"]), true },
        // Different content
        { new Sample(["10 Some Street"]), new Sample(["11 Some Street"]), false },
        // Same content, different order (ordered equality - order matters!)
        { new Sample(["A", "B"]), new Sample(["B", "A"]), false },
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

                                namespace Generator.Equals.Tests.RecordStructs;

                                [Equatable]
                                public partial record struct OrderedEqualitySample([property: OrderedEquality] string[] Addresses);
                                """;
}
