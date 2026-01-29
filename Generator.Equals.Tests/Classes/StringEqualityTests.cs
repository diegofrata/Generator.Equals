using Generator.Equals.Tests.Infrastructure;

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

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

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
