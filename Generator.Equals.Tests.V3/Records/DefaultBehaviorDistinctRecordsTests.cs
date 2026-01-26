using FluentAssertions;
using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Records;

/// <summary>
/// Tests for default behavior with distinct record types.
/// Different record types with same property values should not be equal.
/// </summary>
public partial class DefaultBehaviorDistinctRecordsTests : SnapshotTestBase
{
    public record Sample1(string Name, int Age);
    public record Sample2(string Name, int Age);

    [Fact]
    public void DistinctRecordsWithSameValues_AreNotEqual()
    {
        var a = new Sample1("Dave", 35);
        var b = new Sample2("Dave", 35);

        a.Equals(b).Should().BeFalse("different record types should not be equal");
    }

    [Fact]
    public void SameRecordTypeWithSameValues_AreEqual()
    {
        var a = new Sample1("Dave", 35);
        var b = new Sample1("Dave", 35);

        EqualityAssert.Verify(a, b, true);
    }
}
