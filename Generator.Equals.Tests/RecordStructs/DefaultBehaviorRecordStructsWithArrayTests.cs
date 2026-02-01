using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for default record struct equality with array types (no [Equatable] attribute).
/// Arrays use reference equality by default.
/// </summary>
public partial class DefaultBehaviorRecordStructsWithArrayTests : SnapshotTestBase
{
    public record struct Sample(string Name, string[] Addresses);

    static readonly string[] SharedAddresses = ["10 Some Street", "11 Some Street"];

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values with shared array reference
        { new Sample("Dave", SharedAddresses), new Sample("Dave", SharedAddresses), true },
        // Different arrays with same content (reference equality, so not equal)
        { new Sample("Dave", ["10 Some Street"]), new Sample("Dave", ["10 Some Street"]), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);
}
