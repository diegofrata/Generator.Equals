using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [StringEquality] attribute with different StringComparison options.
/// </summary>
public partial class StringEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct SampleCaseInsensitive
    {
        [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
        public string Name { get; init; }
    }

    [Equatable]
    public partial record struct SampleCaseSensitive
    {
        [StringEquality(StringComparison.CurrentCulture)]
        public string Name { get; init; }
    }

    public static TheoryData<SampleCaseInsensitive, SampleCaseInsensitive, bool> CaseInsensitiveCases => new()
    {
        // Same case
        { new SampleCaseInsensitive { Name = "Dave" }, new SampleCaseInsensitive { Name = "Dave" }, true },
        // Different case (case insensitive)
        { new SampleCaseInsensitive { Name = "Dave" }, new SampleCaseInsensitive { Name = "DAVE" }, true },
        // Different values
        { new SampleCaseInsensitive { Name = "Dave" }, new SampleCaseInsensitive { Name = "John" }, false },
    };

    [Theory]
    [MemberData(nameof(CaseInsensitiveCases))]
    public void CaseInsensitiveEquality(SampleCaseInsensitive a, SampleCaseInsensitive b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    public static TheoryData<SampleCaseSensitive, SampleCaseSensitive, bool> CaseSensitiveCases => new()
    {
        // Same case
        { new SampleCaseSensitive { Name = "Dave" }, new SampleCaseSensitive { Name = "Dave" }, true },
        // Different case (case sensitive)
        { new SampleCaseSensitive { Name = "Dave" }, new SampleCaseSensitive { Name = "DAVE" }, false },
    };

    [Theory]
    [MemberData(nameof(CaseSensitiveCases))]
    public void CaseSensitiveEquality(SampleCaseSensitive a, SampleCaseSensitive b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using System;
        using Generator.Equals;

        namespace Generator.Equals.Tests.RecordStructs;

        [Equatable]
        public partial record struct StringEqualitySampleCaseInsensitive
        {
            [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
            public string Name { get; init; }
        }

        [Equatable]
        public partial record struct StringEqualitySampleCaseSensitive
        {
            [StringEquality(StringComparison.CurrentCulture)]
            public string Name { get; init; }
        }
        """;
}
