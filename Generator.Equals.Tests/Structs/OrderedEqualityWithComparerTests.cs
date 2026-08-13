using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [OrderedEquality] with custom comparers (StringComparison, custom IEqualityComparer).
/// </summary>
public partial class OrderedEqualityWithComparerTests : SnapshotTestBase
{
    [Equatable]
    public partial struct SampleWithStringComparison
    {
        public SampleWithStringComparison(string[] tags)
        {
            Tags = tags;
        }

        [OrderedEquality(StringComparison.OrdinalIgnoreCase)]
        public string[] Tags { get; }
    }

    [Equatable]
    public partial struct SampleWithCustomComparer
    {
        public SampleWithCustomComparer(string[] names)
        {
            Names = names;
        }

        [OrderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
        public string[] Names { get; }
    }

    [Equatable]
    public partial struct SampleWithLengthComparer
    {
        public SampleWithLengthComparer(string[] values)
        {
            Values = values;
        }

        [OrderedEquality(typeof(LengthEqualityComparer))]
        public string[] Values { get; }
    }

    class LengthEqualityComparer : IEqualityComparer<string>
    {
        public static readonly LengthEqualityComparer Default = new();
        public bool Equals(string? x, string? y) => x?.Length == y?.Length;
        public int GetHashCode(string obj) => obj.Length.GetHashCode();
    }

    public static TheoryData<SampleWithStringComparison, SampleWithStringComparison, bool> StringComparisonCases => new()
    {
        // Same content, different case (case insensitive)
        { new SampleWithStringComparison(["FOO", "BAR"]), new SampleWithStringComparison(["foo", "bar"]), true },
        // Different content
        { new SampleWithStringComparison(["FOO", "BAR"]), new SampleWithStringComparison(["foo", "baz"]), false },
    };

    [Theory]
    [MemberData(nameof(StringComparisonCases))]
    public void StringComparisonEquality(SampleWithStringComparison a, SampleWithStringComparison b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    public static TheoryData<SampleWithCustomComparer, SampleWithCustomComparer, bool> CustomComparerCases => new()
    {
        // Same content, different case (case insensitive via StringComparer)
        { new SampleWithCustomComparer(["FOO", "BAR"]), new SampleWithCustomComparer(["foo", "bar"]), true },
        // Different content
        { new SampleWithCustomComparer(["FOO", "BAR"]), new SampleWithCustomComparer(["foo", "baz"]), false },
    };

    [Theory]
    [MemberData(nameof(CustomComparerCases))]
    public void CustomComparerEquality(SampleWithCustomComparer a, SampleWithCustomComparer b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    public static TheoryData<SampleWithLengthComparer, SampleWithLengthComparer, bool> LengthComparerCases => new()
    {
        // Same lengths, different content
        { new SampleWithLengthComparer(["aaa", "bb"]), new SampleWithLengthComparer(["bbb", "cc"]), true },
        // Different lengths
        { new SampleWithLengthComparer(["aaa", "bb"]), new SampleWithLengthComparer(["aaaa", "bb"]), false },
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
    public void Inequality_StringComparison_DifferentContent_ReportsDiffsAtIndices()
    {
        var a = new SampleWithStringComparison(["FOO", "BAR"]);
        var b = new SampleWithStringComparison(["foo", "baz"]);

        var diffs = SampleWithStringComparison.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("FOO", "foo", Prop("Tags"), Idx(0)),
            Ineq("BAR", "baz", Prop("Tags"), Idx(1))
        });
    }

    [Fact]
    public void Inequality_CustomComparer_DifferentContent_ReportsDiffsAtIndices()
    {
        var a = new SampleWithCustomComparer(["FOO", "BAR"]);
        var b = new SampleWithCustomComparer(["foo", "baz"]);

        var diffs = SampleWithCustomComparer.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("FOO", "foo", Prop("Names"), Idx(0)),
            Ineq("BAR", "baz", Prop("Names"), Idx(1))
        });
    }

    [Fact]
    public void Inequality_LengthComparer_DifferentLengths_ReportsDiffAtIndex()
    {
        var a = new SampleWithLengthComparer(["aaa", "bb"]);
        var b = new SampleWithLengthComparer(["aaaa", "bb"]);

        var diffs = SampleWithLengthComparer.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("aaa", "aaaa", Prop("Values"), Idx(0)) });
    }

    const string SampleSource = """
                                using System;
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct OrderedEqualityWithComparerSampleWithStringComparison
                                {
                                    public OrderedEqualityWithComparerSampleWithStringComparison(string[] tags)
                                    {
                                        Tags = tags;
                                    }

                                    [OrderedEquality(StringComparison.OrdinalIgnoreCase)]
                                    public string[] Tags { get; }
                                }

                                [Equatable]
                                public partial struct OrderedEqualityWithComparerSampleWithCustomComparer
                                {
                                    public OrderedEqualityWithComparerSampleWithCustomComparer(string[] names)
                                    {
                                        Names = names;
                                    }

                                    [OrderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
                                    public string[] Names { get; }
                                }

                                [Equatable]
                                public partial struct OrderedEqualityWithComparerSampleWithLengthComparer
                                {
                                    public OrderedEqualityWithComparerSampleWithLengthComparer(string[] values)
                                    {
                                        Values = values;
                                    }

                                    [OrderedEquality(typeof(OrderedLengthEqualityComparer))]
                                    public string[] Values { get; }
                                }

                                class OrderedLengthEqualityComparer : IEqualityComparer<string>
                                {
                                    public static readonly OrderedLengthEqualityComparer Default = new();
                                    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
                                    public int GetHashCode(string obj) => obj.Length.GetHashCode();
                                }
                                """;
}
