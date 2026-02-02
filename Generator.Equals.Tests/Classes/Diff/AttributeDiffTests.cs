using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Diff tests for equality attributes: IgnoreEquality, StringEquality, ReferenceEquality.
/// </summary>
public partial class AttributeDiffTests
{
    static (string Path, object? Left, object? Right) Diff(string path, object? left, object? right)
        => (path, left, right);

    [Equatable]
    public partial class IgnoredPropertySample
    {
        public string? Name { get; set; }

        [IgnoreEquality]
        public int IgnoredAge { get; set; }
    }

    [Equatable]
    public partial class StringComparisonSample
    {
        [StringEquality(StringComparison.OrdinalIgnoreCase)]
        public string? CaseInsensitiveName { get; set; }

        public string? CaseSensitiveName { get; set; }
    }

    [Equatable]
    public partial class ReferenceEqualitySample
    {
        [ReferenceEquality]
        public object? Reference { get; set; }

        public string? Name { get; set; }
    }

    #region IgnoreEquality Tests

    [Fact]
    public void IgnoredProperty_NotReportedInDiff()
    {
        var a = new IgnoredPropertySample { Name = "Alice", IgnoredAge = 30 };
        var b = new IgnoredPropertySample { Name = "Alice", IgnoredAge = 99 };

        var diffs = IgnoredPropertySample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void IgnoredProperty_OnlyReportsNonIgnored()
    {
        var a = new IgnoredPropertySample { Name = "Alice", IgnoredAge = 30 };
        var b = new IgnoredPropertySample { Name = "Bob", IgnoredAge = 99 };

        var diffs = IgnoredPropertySample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Name", "Alice", "Bob") });
    }

    #endregion

    #region StringEquality Tests

    [Fact]
    public void CaseInsensitiveString_SameCaseDifferent_NoDiff()
    {
        var a = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "Test" };
        var b = new StringComparisonSample { CaseInsensitiveName = "ALICE", CaseSensitiveName = "Test" };

        var diffs = StringComparisonSample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void CaseSensitiveString_SameCaseDifferent_ReportsDiff()
    {
        var a = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "Test" };
        var b = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "TEST" };

        var diffs = StringComparisonSample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("CaseSensitiveName", "Test", "TEST") });
    }

    [Fact]
    public void CaseInsensitiveString_ActuallyDifferent_ReportsDiff()
    {
        var a = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "Test" };
        var b = new StringComparisonSample { CaseInsensitiveName = "Bob", CaseSensitiveName = "Test" };

        var diffs = StringComparisonSample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("CaseInsensitiveName", "Alice", "Bob") });
    }

    #endregion

    #region ReferenceEquality Tests

    [Fact]
    public void ReferenceEquality_SameReference_NoDiff()
    {
        var sharedRef = new object();
        var a = new ReferenceEqualitySample { Reference = sharedRef, Name = "Test" };
        var b = new ReferenceEqualitySample { Reference = sharedRef, Name = "Test" };

        var diffs = ReferenceEqualitySample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void ReferenceEquality_DifferentReference_ReportsDiff()
    {
        var ref1 = new object();
        var ref2 = new object();
        var a = new ReferenceEqualitySample { Reference = ref1, Name = "Test" };
        var b = new ReferenceEqualitySample { Reference = ref2, Name = "Test" };

        var diffs = ReferenceEqualitySample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Reference", ref1, ref2) });
    }

    [Fact]
    public void ReferenceEquality_EqualButDifferentReference_ReportsDiff()
    {
        // Two strings with same content but different references (force no interning)
        var str1 = new string("test".ToCharArray());
        var str2 = new string("test".ToCharArray());

        var a = new ReferenceEqualitySample { Reference = str1, Name = "Test" };
        var b = new ReferenceEqualitySample { Reference = str2, Name = "Test" };

        var diffs = ReferenceEqualitySample.EqualityComparer.Default.Diff(a, b).ToList();

        // Reference equality should report diff even if content is equal
        diffs.Should().BeEquivalentTo(new[] { Diff("Reference", str1, str2) });
    }

    #endregion
}
