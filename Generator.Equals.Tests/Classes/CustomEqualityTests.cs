using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [CustomEquality] attribute with custom IEqualityComparer implementations.
/// </summary>
public partial class CustomEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
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
        // Same length strings should be equal (custom comparer compares by length)
        { new Sample("My String", "My String", "My String"), new Sample("My ____ng", "My ____ng", "My ____ng"), true },
        // Different length strings should not be equal
        { new Sample("My String", "My String", "My String"), new Sample("My String ", "My String ", "My String "), false },
        // All same
        { new Sample("abc", "abc", "abc"), new Sample("abc", "abc", "abc"), true },
        // Same length but different content
        { new Sample("abc", "abc", "abc"), new Sample("xyz", "xyz", "xyz"), true },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class CustomEqualitySample
                                {
                                    public CustomEqualitySample(string name1, string name2, string name3)
                                    {
                                        Name1 = name1;
                                        Name2 = name2;
                                        Name3 = name3;
                                    }

                                    [CustomEquality(typeof(Comparer1))] public string Name1 { get; }
                                    [CustomEquality(typeof(Comparer2), nameof(Comparer2.Instance))] public string Name2 { get; }
                                    [CustomEquality(typeof(CustomLengthEqualityComparer))] public string Name3 { get; }
                                }

                                class Comparer1
                                {
                                    public static readonly CustomLengthEqualityComparer Default = new();
                                }

                                class Comparer2
                                {
                                    public static readonly CustomLengthEqualityComparer Instance = new();
                                }

                                class CustomLengthEqualityComparer : IEqualityComparer<string>
                                {
                                    public bool Equals(string? x, string? y) => x?.Length == y?.Length;
                                    public int GetHashCode(string obj) => obj.Length.GetHashCode();
                                }
                                """;
}
