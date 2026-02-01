using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [UnorderedEquality] on Dictionary properties.
/// Dictionary order should not matter when comparing.
/// </summary>
public partial class DictionaryEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        [UnorderedEquality] public Dictionary<string, int>? Properties { get; set; }
    }

    [Fact]
    public void SameContentsDifferentOrder_ShouldBeEqual()
    {
        var random = new Random(42);
        var items = Enumerable.Range(1, 1000);

        var a = new Sample
        {
            Properties = items.ToDictionary(x => x.ToString(), x => x)
        };
        var b = new Sample
        {
            Properties = items
                .OrderBy(_ => random.NextDouble())
                .ToDictionary(x => x.ToString(), x => x)
        };

        EqualityAssert.Verify(a, b, true);
    }

    [Fact]
    public void DifferentContents_ShouldNotBeEqual()
    {
        var a = new Sample
        {
            Properties = Enumerable.Range(1, 1000).ToDictionary(x => x.ToString(), x => x)
        };
        var b = new Sample
        {
            Properties = Enumerable.Range(2, 999).ToDictionary(x => x.ToString(), x => x)
        };

        EqualityAssert.Verify(a, b, false);
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same content
        {
            new Sample { Properties = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 } },
            new Sample { Properties = new Dictionary<string, int> { ["a"] = 1, ["b"] = 2 } },
            true
        },
        // Different values
        {
            new Sample { Properties = new Dictionary<string, int> { ["a"] = 1 } },
            new Sample { Properties = new Dictionary<string, int> { ["a"] = 2 } },
            false
        },
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
                                public partial class DictionaryEqualitySample
                                {
                                    [UnorderedEquality] public Dictionary<string, int>? Properties { get; set; }
                                }
                                """;
}
