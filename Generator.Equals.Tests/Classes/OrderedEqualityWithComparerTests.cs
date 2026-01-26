using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [OrderedEquality] with custom comparers (StringComparison, custom IEqualityComparer).
/// </summary>
public partial class OrderedEqualityWithComparerTests : SnapshotTestBase
{
    [Equatable]
    public partial class SampleWithStringComparison
    {
        public SampleWithStringComparison(string[] tags)
        {
            Tags = tags;
        }

        [OrderedEquality(StringComparison.OrdinalIgnoreCase)]
        public string[] Tags { get; }
    }

    [Equatable]
    public partial class SampleWithCustomComparer
    {
        public SampleWithCustomComparer(string[] names)
        {
            Names = names;
        }

        [OrderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
        public string[] Names { get; }
    }

    [Equatable]
    public partial class SampleWithLengthComparer
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
        { new SampleWithStringComparison(new[] { "FOO", "BAR" }), new SampleWithStringComparison(new[] { "foo", "bar" }), true },
        // Different content
        { new SampleWithStringComparison(new[] { "FOO", "BAR" }), new SampleWithStringComparison(new[] { "foo", "baz" }), false },
    };

    [Theory]
    [MemberData(nameof(StringComparisonCases))]
    public void StringComparisonEquality(SampleWithStringComparison a, SampleWithStringComparison b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithCustomComparer, SampleWithCustomComparer, bool> CustomComparerCases => new()
    {
        // Same content, different case (case insensitive via StringComparer)
        { new SampleWithCustomComparer(new[] { "FOO", "BAR" }), new SampleWithCustomComparer(new[] { "foo", "bar" }), true },
        // Different content
        { new SampleWithCustomComparer(new[] { "FOO", "BAR" }), new SampleWithCustomComparer(new[] { "foo", "baz" }), false },
    };

    [Theory]
    [MemberData(nameof(CustomComparerCases))]
    public void CustomComparerEquality(SampleWithCustomComparer a, SampleWithCustomComparer b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleWithLengthComparer, SampleWithLengthComparer, bool> LengthComparerCases => new()
    {
        // Same lengths, different content
        { new SampleWithLengthComparer(new[] { "aaa", "bb" }), new SampleWithLengthComparer(new[] { "bbb", "cc" }), true },
        // Different lengths
        { new SampleWithLengthComparer(new[] { "aaa", "bb" }), new SampleWithLengthComparer(new[] { "aaaa", "bb" }), false },
    };

    [Theory]
    [MemberData(nameof(LengthComparerCases))]
    public void LengthComparerEquality(SampleWithLengthComparer a, SampleWithLengthComparer b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using System;
        using System.Collections.Generic;
        using Generator.Equals;

        namespace Generator.Equals.Tests.Classes;

        [Equatable]
        public partial class OrderedEqualityWithComparerSampleWithStringComparison
        {
            public OrderedEqualityWithComparerSampleWithStringComparison(string[] tags)
            {
                Tags = tags;
            }

            [OrderedEquality(StringComparison.OrdinalIgnoreCase)]
            public string[] Tags { get; }
        }

        [Equatable]
        public partial class OrderedEqualityWithComparerSampleWithCustomComparer
        {
            public OrderedEqualityWithComparerSampleWithCustomComparer(string[] names)
            {
                Names = names;
            }

            [OrderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
            public string[] Names { get; }
        }

        [Equatable]
        public partial class OrderedEqualityWithComparerSampleWithLengthComparer
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
