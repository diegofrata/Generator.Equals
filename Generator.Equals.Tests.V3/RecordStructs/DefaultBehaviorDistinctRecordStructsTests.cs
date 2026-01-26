using FluentAssertions;
using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.RecordStructs;

/// <summary>
/// Tests for default behavior with distinct record struct types.
/// Different record struct types with same property values should not be equal.
/// </summary>
public partial class DefaultBehaviorDistinctRecordStructsTests : SnapshotTestBase
{
    public record struct Sample1(string Name, int Age);
    public record struct Sample2(string Name, int Age);

    [Fact]
    public void DistinctRecordStructsWithSameValues_AreNotEqual()
    {
        var a = new Sample1("Dave", 35);
        var b = new Sample2("Dave", 35);

        a.Equals(b).Should().BeFalse("different record struct types should not be equal");
    }

    [Fact]
    public void SameRecordStructTypeWithSameValues_AreEqual()
    {
        var a = new Sample1("Dave", 35);
        var b = new Sample1("Dave", 35);

        EqualityAssert.VerifyStruct(a, b, true);
    }
}
