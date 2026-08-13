using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [SetEquality] with custom comparers (StringComparison, custom IEqualityComparer).
/// </summary>
public partial class SetEqualityWithComparerTests : SnapshotTestBase
{
    [Equatable]
    public partial record SampleWithStringComparison
    {
        public SampleWithStringComparison(HashSet<string>? tags)
        {
            Tags = tags;
        }

        [SetEquality(StringComparison.OrdinalIgnoreCase)]
        public HashSet<string>? Tags { get; }
    }

    [Equatable]
    public partial record SampleWithCustomComparer
    {
        public SampleWithCustomComparer(HashSet<string>? names)
        {
            Names = names;
        }

        [SetEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
        public HashSet<string>? Names { get; }
    }

    [Equatable]
    public partial record SampleWithLengthComparer
    {
        public SampleWithLengthComparer(HashSet<string>? values)
        {
            Values = values;
        }

        [SetEquality(typeof(LengthEqualityComparer))]
        public HashSet<string>? Values { get; }
    }

    class LengthEqualityComparer : IEqualityComparer<string>
    {
        public static readonly LengthEqualityComparer Default = new();
        public bool Equals(string? x, string? y) => x?.Length == y?.Length;
        public int GetHashCode(string obj) => obj.Length.GetHashCode();
    }

    public static TheoryData<SampleWithStringComparison, SampleWithStringComparison, bool> StringComparisonCases => new()
    {
        // Same elements different case (set + case insensitive)
        { new SampleWithStringComparison(["FOO", "BAR"]), new SampleWithStringComparison(["bar", "foo"]), true },
        // Different elements
        { new SampleWithStringComparison(["FOO", "BAR"]), new SampleWithStringComparison(["foo", "baz"]), false },
    };

    [Theory]
    [MemberData(nameof(StringComparisonCases))]
    public void StringComparisonEquality(SampleWithStringComparison a, SampleWithStringComparison b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithCustomComparer, SampleWithCustomComparer, bool> CustomComparerCases => new()
    {
        // Same elements different case (set + case insensitive)
        { new SampleWithCustomComparer(["FOO", "BAR"]), new SampleWithCustomComparer(["bar", "foo"]), true },
        // Different elements
        { new SampleWithCustomComparer(["FOO", "BAR"]), new SampleWithCustomComparer(["foo", "baz"]), false },
    };

    [Theory]
    [MemberData(nameof(CustomComparerCases))]
    public void CustomComparerEquality(SampleWithCustomComparer a, SampleWithCustomComparer b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithLengthComparer, SampleWithLengthComparer, bool> LengthComparerCases => new()
    {
        // Same lengths (set + length comparison)
        { new SampleWithLengthComparer(["aaa", "bb"]), new SampleWithLengthComparer(["bbb", "cc"]), true },
        // Different lengths
        { new SampleWithLengthComparer(["aaa", "bb"]), new SampleWithLengthComparer(["aaaa", "bb"]), false },
    };

    [Theory]
    [MemberData(nameof(LengthComparerCases))]
    public void LengthComparerEquality(SampleWithLengthComparer a, SampleWithLengthComparer b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

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

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record SetEqualityWithComparerSampleWithStringComparison
                                {
                                    public SetEqualityWithComparerSampleWithStringComparison(HashSet<string>? tags)
                                    {
                                        Tags = tags;
                                    }

                                    [SetEquality(StringComparison.OrdinalIgnoreCase)]
                                    public HashSet<string>? Tags { get; }
                                }

                                [Equatable]
                                public partial record SetEqualityWithComparerSampleWithCustomComparer
                                {
                                    public SetEqualityWithComparerSampleWithCustomComparer(HashSet<string>? names)
                                    {
                                        Names = names;
                                    }

                                    [SetEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
                                    public HashSet<string>? Names { get; }
                                }

                                [Equatable]
                                public partial record SetEqualityWithComparerSampleWithLengthComparer
                                {
                                    public SetEqualityWithComparerSampleWithLengthComparer(HashSet<string>? values)
                                    {
                                        Values = values;
                                    }

                                    [SetEquality(typeof(SetLengthEqualityComparer))]
                                    public HashSet<string>? Values { get; }
                                }

                                class SetLengthEqualityComparer : IEqualityComparer<string>
                                {
                                    public static readonly SetLengthEqualityComparer Default = new();
                                    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
                                    public int GetHashCode(string obj) => obj.Length.GetHashCode();
                                }
                                """;
}
