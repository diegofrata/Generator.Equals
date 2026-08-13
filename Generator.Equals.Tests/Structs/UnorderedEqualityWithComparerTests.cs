using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [UnorderedEquality] with custom comparers (StringComparison, custom IEqualityComparer).
/// </summary>
public partial class UnorderedEqualityWithComparerTests : SnapshotTestBase
{
    [Equatable]
    public partial struct SampleWithStringComparison
    {
        public SampleWithStringComparison(List<string> tags)
        {
            Tags = tags;
        }

        [UnorderedEquality(StringComparison.OrdinalIgnoreCase)]
        public List<string> Tags { get; }
    }

    [Equatable]
    public partial struct SampleWithCustomComparer
    {
        public SampleWithCustomComparer(List<string> names)
        {
            Names = names;
        }

        [UnorderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
        public List<string> Names { get; }
    }

    [Equatable]
    public partial struct SampleWithLengthComparer
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
        EqualityAssert.VerifyStruct(a, b, expected);

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
        EqualityAssert.VerifyStruct(a, b, expected);

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
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    [Fact]
    public void Inequality_StringComparison_DifferentContent_ReportsAddedAndRemoved()
    {
        var a = new SampleWithStringComparison(["FOO", "BAR"]);
        var b = new SampleWithStringComparison(["foo", "baz"]);

        var diffs = SampleWithStringComparison.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("FOO", null, Prop("Tags"), MemberPathSegment.Removed()),
            Ineq("BAR", null, Prop("Tags"), MemberPathSegment.Removed()),
            Ineq(null, "foo", Prop("Tags"), MemberPathSegment.Added()),
            Ineq(null, "baz", Prop("Tags"), MemberPathSegment.Added())
        });
    }

    [Fact]
    public void Inequality_CustomComparer_DifferentContent_ReportsAddedAndRemoved()
    {
        var a = new SampleWithCustomComparer(["FOO", "BAR"]);
        var b = new SampleWithCustomComparer(["foo", "baz"]);

        var diffs = SampleWithCustomComparer.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("FOO", null, Prop("Names"), MemberPathSegment.Removed()),
            Ineq("BAR", null, Prop("Names"), MemberPathSegment.Removed()),
            Ineq(null, "foo", Prop("Names"), MemberPathSegment.Added()),
            Ineq(null, "baz", Prop("Names"), MemberPathSegment.Added())
        });
    }

    [Fact]
    public void Inequality_LengthComparer_DifferentLengths_ReportsAddedAndRemoved()
    {
        var a = new SampleWithLengthComparer(["aaa", "bb"]);
        var b = new SampleWithLengthComparer(["bb", "aaaa"]);

        var diffs = SampleWithLengthComparer.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("aaa", null, Prop("Values"), MemberPathSegment.Removed()),
            Ineq(null, "aaaa", Prop("Values"), MemberPathSegment.Added())
        });
    }

    const string SampleSource = """
                                using System;
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct UnorderedEqualityWithComparerSampleWithStringComparison
                                {
                                    public UnorderedEqualityWithComparerSampleWithStringComparison(List<string> tags)
                                    {
                                        Tags = tags;
                                    }

                                    [UnorderedEquality(StringComparison.OrdinalIgnoreCase)]
                                    public List<string> Tags { get; }
                                }

                                [Equatable]
                                public partial struct UnorderedEqualityWithComparerSampleWithCustomComparer
                                {
                                    public UnorderedEqualityWithComparerSampleWithCustomComparer(List<string> names)
                                    {
                                        Names = names;
                                    }

                                    [UnorderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
                                    public List<string> Names { get; }
                                }

                                [Equatable]
                                public partial struct UnorderedEqualityWithComparerSampleWithLengthComparer
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
