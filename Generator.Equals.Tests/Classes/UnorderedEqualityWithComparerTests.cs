using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [UnorderedEquality] with custom comparers (StringComparison, custom IEqualityComparer).
/// </summary>
public partial class UnorderedEqualityWithComparerTests : SnapshotTestBase
{
    [Equatable]
    public partial class SampleWithStringComparison
    {
        public SampleWithStringComparison(List<string> tags)
        {
            Tags = tags;
        }

        [UnorderedEquality(StringComparison.OrdinalIgnoreCase)]
        public List<string> Tags { get; }
    }

    [Equatable]
    public partial class SampleWithCustomComparer
    {
        public SampleWithCustomComparer(List<string> names)
        {
            Names = names;
        }

        [UnorderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
        public List<string> Names { get; }
    }

    [Equatable]
    public partial class SampleWithLengthComparer
    {
        public SampleWithLengthComparer(List<string> values)
        {
            Values = values;
        }

        [UnorderedEquality(typeof(LengthEqualityComparer))]
        public List<string> Values { get; }
    }

    class LengthEqualityComparer : IEqualityComparer<string>
    {
        public static readonly LengthEqualityComparer Default = new();
        public bool Equals(string? x, string? y) => x?.Length == y?.Length;
        public int GetHashCode(string obj) => obj.Length.GetHashCode();
    }

    public static TheoryData<SampleWithStringComparison, SampleWithStringComparison, bool> StringComparisonCases => new()
    {
        // Same content, different case and order (case insensitive + unordered)
        { new SampleWithStringComparison(["FOO", "BAR"]), new SampleWithStringComparison(["bar", "foo"]), true },
        // Different content
        { new SampleWithStringComparison(["FOO", "BAR"]), new SampleWithStringComparison(["foo", "baz"]), false },
    };

    [Theory]
    [MemberData(nameof(StringComparisonCases))]
    public void StringComparisonEquality(SampleWithStringComparison a, SampleWithStringComparison b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithCustomComparer, SampleWithCustomComparer, bool> CustomComparerCases => new()
    {
        // Same content, different case and order (case insensitive + unordered)
        { new SampleWithCustomComparer(["FOO", "BAR"]), new SampleWithCustomComparer(["bar", "foo"]), true },
        // Different content
        { new SampleWithCustomComparer(["FOO", "BAR"]), new SampleWithCustomComparer(["foo", "baz"]), false },
    };

    [Theory]
    [MemberData(nameof(CustomComparerCases))]
    public void CustomComparerEquality(SampleWithCustomComparer a, SampleWithCustomComparer b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithLengthComparer, SampleWithLengthComparer, bool> LengthComparerCases => new()
    {
        // Same lengths, different content and order (length comparison + unordered)
        { new SampleWithLengthComparer(["aaa", "bb"]), new SampleWithLengthComparer(["cc", "bbb"]), true },
        // Different lengths
        { new SampleWithLengthComparer(["aaa", "bb"]), new SampleWithLengthComparer(["bb", "aaaa"]), false },
    };

    [Theory]
    [MemberData(nameof(LengthComparerCases))]
    public void LengthComparerEquality(SampleWithLengthComparer a, SampleWithLengthComparer b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System;
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class UnorderedEqualityWithComparerSampleWithStringComparison
                                {
                                    public UnorderedEqualityWithComparerSampleWithStringComparison(List<string> tags)
                                    {
                                        Tags = tags;
                                    }

                                    [UnorderedEquality(StringComparison.OrdinalIgnoreCase)]
                                    public List<string> Tags { get; }
                                }

                                [Equatable]
                                public partial class UnorderedEqualityWithComparerSampleWithCustomComparer
                                {
                                    public UnorderedEqualityWithComparerSampleWithCustomComparer(List<string> names)
                                    {
                                        Names = names;
                                    }

                                    [UnorderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
                                    public List<string> Names { get; }
                                }

                                [Equatable]
                                public partial class UnorderedEqualityWithComparerSampleWithLengthComparer
                                {
                                    public UnorderedEqualityWithComparerSampleWithLengthComparer(List<string> values)
                                    {
                                        Values = values;
                                    }

                                    [UnorderedEquality(typeof(UnorderedLengthEqualityComparer))]
                                    public List<string> Values { get; }
                                }

                                class UnorderedLengthEqualityComparer : IEqualityComparer<string>
                                {
                                    public static readonly UnorderedLengthEqualityComparer Default = new();
                                    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
                                    public int GetHashCode(string obj) => obj.Length.GetHashCode();
                                }
                                """;
}
