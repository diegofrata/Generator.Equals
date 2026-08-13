using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for default record struct equality with primitive types (no [Equatable] attribute).
/// </summary>
public partial class DefaultBehaviorRecordStructsWithPrimitivesTests : SnapshotTestBase
{
    public record struct Sample(string Name, int Age);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
        // Different Name
        { new Sample("Dave", 35), new Sample("John", 35), false },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);
}
