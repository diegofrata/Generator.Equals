using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [UnorderedEquality] attribute on Dictionary property.
/// </summary>
public partial class DictionaryEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample
    {
        [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same content
        { new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } }, new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } }, true },
        // Same content, different insertion order
        { new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } }, new Sample { Properties = new() { ["b"] = 2, ["a"] = 1 } }, true },
        // Different content
        { new Sample { Properties = new() { ["a"] = 1 } }, new Sample { Properties = new() { ["a"] = 2 } }, false },
        // One null, one not
        { new Sample { Properties = null }, new Sample { Properties = new() { ["a"] = 1 } }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    [Fact]
    public void Inequality_DifferentValue_ReportsDiffAtKey()
    {
        var a = new Sample { Properties = new() { ["a"] = 1 } };
        var b = new Sample { Properties = new() { ["a"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(1, 2, Prop("Properties"), DKey("a")) });
    }

    [Fact]
    public void Inequality_AddedKey_ReportsAddition()
    {
        var a = new Sample { Properties = new() { ["a"] = 1 } };
        var b = new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, 2, Prop("Properties"), DKey("b")) });
    }

    [Fact]
    public void Inequality_RemovedKey_ReportsRemoval()
    {
        var a = new Sample { Properties = new() { ["a"] = 1, ["b"] = 2 } };
        var b = new Sample { Properties = new() { ["a"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(2, null, Prop("Properties"), DKey("b")) });
    }

    [Fact]
    public void Inequality_NullVsNonNull_ReportsAddedKeys()
    {
        var a = new Sample { Properties = null };
        var b = new Sample { Properties = new() { ["a"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, 1, Prop("Properties"), DKey("a")) });
    }

    const string SampleSource = """
                                using System.Collections.Generic;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record DictionaryEqualitySample
                                {
                                    [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
                                }
                                """;
}
