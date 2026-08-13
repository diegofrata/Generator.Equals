using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [StringEquality] attribute with different StringComparison options.
/// </summary>
public partial class StringEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class SampleCaseInsensitive
    {
        public SampleCaseInsensitive(string name)
        {
            Name = name;
        }

        [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
        public string Name { get; }
    }

    [Equatable]
    public partial class SampleCaseSensitive
    {
        public SampleCaseSensitive(string name)
        {
            Name = name;
        }

        [StringEquality(StringComparison.CurrentCulture)]
        public string Name { get; }
    }

    public static TheoryData<SampleCaseInsensitive, SampleCaseInsensitive, bool> CaseInsensitiveCases => new()
    {
        // Same case
        { new SampleCaseInsensitive("BAR"), new SampleCaseInsensitive("BAR"), true },
        // Different case, should be equal (case insensitive)
        { new SampleCaseInsensitive("BAR"), new SampleCaseInsensitive("bar"), true },
        // Completely different values
        { new SampleCaseInsensitive("BAR"), new SampleCaseInsensitive("foo"), false },
    };

    [Theory]
    [MemberData(nameof(CaseInsensitiveCases))]
    public void CaseInsensitiveEquality(SampleCaseInsensitive a, SampleCaseInsensitive b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleCaseSensitive, SampleCaseSensitive, bool> CaseSensitiveCases => new()
    {
        // Same case
        { new SampleCaseSensitive("Foo"), new SampleCaseSensitive("Foo"), true },
        // Different case, should NOT be equal (case sensitive)
        { new SampleCaseSensitive("Foo"), new SampleCaseSensitive("foo"), false },
        // Completely different values
        { new SampleCaseSensitive("Foo"), new SampleCaseSensitive("Bar"), false },
    };

    [Theory]
    [MemberData(nameof(CaseSensitiveCases))]
    public void CaseSensitiveEquality(SampleCaseSensitive a, SampleCaseSensitive b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void CaseInsensitive_DifferentValues_ReportsInequality()
    {
        var a = new SampleCaseInsensitive("BAR");
        var b = new SampleCaseInsensitive("foo");

        var diffs = SampleCaseInsensitive.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("BAR", "foo", Prop("Name")) });
    }

    [Fact]
    public void CaseSensitive_DifferentCase_ReportsInequality()
    {
        var a = new SampleCaseSensitive("Foo");
        var b = new SampleCaseSensitive("foo");

        var diffs = SampleCaseSensitive.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Foo", "foo", Prop("Name")) });
    }

    [Fact]
    public void CaseSensitive_CompletelyDifferent_ReportsInequality()
    {
        var a = new SampleCaseSensitive("Foo");
        var b = new SampleCaseSensitive("Bar");

        var diffs = SampleCaseSensitive.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Foo", "Bar", Prop("Name")) });
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class StringEqualityCaseInsensitiveSample
                                {
                                    public StringEqualityCaseInsensitiveSample(string name)
                                    {
                                        Name = name;
                                    }

                                    [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
                                    public string Name { get; }
                                }

                                [Equatable]
                                public partial class StringEqualityCaseSensitiveSample
                                {
                                    public StringEqualityCaseSensitiveSample(string name)
                                    {
                                        Name = name;
                                    }

                                    [StringEquality(StringComparison.CurrentCulture)]
                                    public string Name { get; }
                                }
                                """;
}
