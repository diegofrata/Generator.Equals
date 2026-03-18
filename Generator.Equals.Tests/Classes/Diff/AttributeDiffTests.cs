using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Diff tests for equality attributes: IgnoreEquality, StringEquality, ReferenceEquality.
/// </summary>
public partial class AttributeDiffTests
{
    static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);
    static MemberPathSegment Idx(int i) => MemberPathSegment.Index(i);

    static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);

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

        var diffs = IgnoredPropertySample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void IgnoredProperty_OnlyReportsNonIgnored()
    {
        var a = new IgnoredPropertySample { Name = "Alice", IgnoredAge = 30 };
        var b = new IgnoredPropertySample { Name = "Bob", IgnoredAge = 99 };

        var diffs = IgnoredPropertySample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("Name")) });
    }

    #endregion

    #region StringEquality Tests

    [Fact]
    public void CaseInsensitiveString_SameCaseDifferent_NoDiff()
    {
        var a = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "Test" };
        var b = new StringComparisonSample { CaseInsensitiveName = "ALICE", CaseSensitiveName = "Test" };

        var diffs = StringComparisonSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void CaseSensitiveString_SameCaseDifferent_ReportsDiff()
    {
        var a = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "Test" };
        var b = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "TEST" };

        var diffs = StringComparisonSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Test", "TEST", Prop("CaseSensitiveName")) });
    }

    [Fact]
    public void CaseInsensitiveString_ActuallyDifferent_ReportsDiff()
    {
        var a = new StringComparisonSample { CaseInsensitiveName = "Alice", CaseSensitiveName = "Test" };
        var b = new StringComparisonSample { CaseInsensitiveName = "Bob", CaseSensitiveName = "Test" };

        var diffs = StringComparisonSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("CaseInsensitiveName")) });
    }

    #endregion

    #region ReferenceEquality Tests

    [Fact]
    public void ReferenceEquality_SameReference_NoDiff()
    {
        var sharedRef = new object();
        var a = new ReferenceEqualitySample { Reference = sharedRef, Name = "Test" };
        var b = new ReferenceEqualitySample { Reference = sharedRef, Name = "Test" };

        var diffs = ReferenceEqualitySample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void ReferenceEquality_DifferentReference_ReportsDiff()
    {
        var ref1 = new object();
        var ref2 = new object();
        var a = new ReferenceEqualitySample { Reference = ref1, Name = "Test" };
        var b = new ReferenceEqualitySample { Reference = ref2, Name = "Test" };

        var diffs = ReferenceEqualitySample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(ref1, ref2, Prop("Reference")) });
    }

    [Fact]
    public void ReferenceEquality_EqualButDifferentReference_ReportsDiff()
    {
        // Two strings with same content but different references (force no interning)
        var str1 = new string("test".ToCharArray());
        var str2 = new string("test".ToCharArray());

        var a = new ReferenceEqualitySample { Reference = str1, Name = "Test" };
        var b = new ReferenceEqualitySample { Reference = str2, Name = "Test" };

        var diffs = ReferenceEqualitySample.EqualityComparer.Default.Inequalities(a, b).ToList();

        // Reference equality should report diff even if content is equal
        diffs.Should().BeEquivalentTo(new[] { Ineq(str1, str2, Prop("Reference")) });
    }

    #endregion

    #region Field Inequality Tests

    [Equatable]
    public partial class FieldSample
    {
        public FieldSample(string name, int age)
        {
            _name = name;
            _age = age;
        }

        [DefaultEquality] readonly string _name;
        [DefaultEquality] readonly int _age;
    }

    [Fact]
    public void Field_SameValues_NoDiff()
    {
        var a = new FieldSample("Alice", 30);
        var b = new FieldSample("Alice", 30);

        var diffs = FieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Field_DifferentValues_ReportsDiff()
    {
        var a = new FieldSample("Alice", 30);
        var b = new FieldSample("Bob", 35);

        var diffs = FieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Alice", "Bob", Prop("_name")),
            Ineq(30, 35, Prop("_age"))
        });
    }

    [Fact]
    public void Field_OnlyChangedFieldReported()
    {
        var a = new FieldSample("Alice", 30);
        var b = new FieldSample("Alice", 35);

        var diffs = FieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(30, 35, Prop("_age")) });
    }

    [Equatable]
    public partial class CollectionFieldSample
    {
        public CollectionFieldSample(string[] items)
        {
            _items = items;
        }

        [OrderedEquality] readonly string[] _items;
    }

    [Fact]
    public void CollectionField_DifferentElements_ReportsElementDiffs()
    {
        var a = new CollectionFieldSample(["a", "b"]);
        var b = new CollectionFieldSample(["a", "c"]);

        var diffs = CollectionFieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("b", "c", Prop("_items"), Idx(1)) });
    }

    [Fact]
    public void CollectionField_SameElements_NoDiff()
    {
        var a = new CollectionFieldSample(["a", "b"]);
        var b = new CollectionFieldSample(["a", "b"]);

        var diffs = CollectionFieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Equatable]
    public partial class IgnoredFieldSample
    {
        public IgnoredFieldSample(string name, int secret)
        {
            _name = name;
            _secret = secret;
        }

        [DefaultEquality] readonly string _name;
        [IgnoreEquality] readonly int _secret;
    }

    [Fact]
    public void IgnoredField_NotReportedInDiff()
    {
        var a = new IgnoredFieldSample("Alice", 42);
        var b = new IgnoredFieldSample("Alice", 99);

        var diffs = IgnoredFieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void IgnoredField_OnlyReportsNonIgnored()
    {
        var a = new IgnoredFieldSample("Alice", 42);
        var b = new IgnoredFieldSample("Bob", 99);

        var diffs = IgnoredFieldSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Alice", "Bob", Prop("_name")) });
    }

    #endregion
}
