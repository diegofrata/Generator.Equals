using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [CustomEquality] attribute with custom comparers on record struct properties.
/// </summary>
public partial class CustomEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct Sample
    {
        public Sample(string name1, string name2, string name3)
        {
            Name1 = name1;
            Name2 = name2;
            Name3 = name3;
        }

        [CustomEquality(typeof(Comparer1))] public string Name1 { get; }
        [CustomEquality(typeof(Comparer2), nameof(Comparer2.Instance))] public string Name2 { get; }
        [CustomEquality(typeof(LengthEqualityComparer))] public string Name3 { get; }
    }

    class Comparer1
    {
        public static readonly LengthEqualityComparer Default = new();
    }

    class Comparer2
    {
        public static readonly LengthEqualityComparer Instance = new();
    }

    class LengthEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string? x, string? y) => x?.Length == y?.Length;
        public int GetHashCode(string obj) => obj.Length.GetHashCode();
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same lengths for all
        { new Sample("aaa", "bbb", "ccc"), new Sample("xxx", "yyy", "zzz"), true },
        // Different length for Name1
        { new Sample("aaa", "bbb", "ccc"), new Sample("aa", "yyy", "zzz"), false },
        // Different length for Name2
        { new Sample("aaa", "bbb", "ccc"), new Sample("xxx", "yy", "zzz"), false },
        // Different length for Name3
        { new Sample("aaa", "bbb", "ccc"), new Sample("xxx", "yyy", "zz"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.RecordStructs;

                                [Equatable]
                                public partial record struct CustomEqualitySample
                                {
                                    public CustomEqualitySample(string name1, string name2, string name3)
                                    {
                                        Name1 = name1;
                                        Name2 = name2;
                                        Name3 = name3;
                                    }

                                    [CustomEquality(typeof(CustomEqualityComparer1))] public string Name1 { get; }
                                    [CustomEquality(typeof(CustomEqualityComparer2), nameof(CustomEqualityComparer2.Instance))] public string Name2 { get; }
                                    [CustomEquality(typeof(CustomEqualityLengthEqualityComparer))] public string Name3 { get; }
                                }

                                class CustomEqualityComparer1
                                {
                                    public static readonly CustomEqualityLengthEqualityComparer Default = new();
                                }

                                class CustomEqualityComparer2
                                {
                                    public static readonly CustomEqualityLengthEqualityComparer Instance = new();
                                }

                                class CustomEqualityLengthEqualityComparer : IEqualityComparer<string>
                                {
                                    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
                                    public int GetHashCode(string obj) => obj.Length.GetHashCode();
                                }
                                """;
}
