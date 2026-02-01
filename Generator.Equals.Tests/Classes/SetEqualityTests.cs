using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [SetEquality] on collection properties.
/// Duplicates are ignored and order doesn't matter (set semantics).
/// </summary>
public partial class SetEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        [SetEquality] public List<int>? Properties { get; set; }
    }

    [Fact]
    public void SameSetWithDifferentDuplicates_ShouldBeEqual()
    {
        // Both have the same unique elements {1,2,3,4,5} but different duplicates
        var a = new Sample { Properties = [1, 2, 3, 4, 5, 5, 4, 3, 2, 1] };
        var b = new Sample { Properties = [1, 5, 2, 4, 3] };

        EqualityAssert.Verify(a, b, true);
    }

    [Fact]
    public void DifferentSets_ShouldNotBeEqual()
    {
        var a = new Sample { Properties = [1, 2, 3, 4, 5] };
        var b = new Sample { Properties = [1, 2, 3, 4] };

        EqualityAssert.Verify(a, b, false);
    }

    [Fact]
    public void DifferentSets_HashCodesMayCollide()
    {
        // Note: SetEquality uses XOR for hash code which can have collisions
        var a = new Sample { Properties = [1, 2, 3, 4, 5] };
        var b = new Sample { Properties = [1, 2, 3, 4] };

        // Hash codes may or may not be equal for different sets (not guaranteed)
        // This test just verifies the behavior exists
        _ = a.GetHashCode();
        _ = b.GetHashCode();
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Both null
        { new Sample { Properties = null }, new Sample { Properties = null }, true },
        // Same set
        { new Sample { Properties = [1, 2, 3] }, new Sample { Properties = [1, 2, 3] }, true },
        // Same set, different order
        { new Sample { Properties = [1, 2, 3] }, new Sample { Properties = [3, 2, 1] }, true },
        // Same set with duplicates
        { new Sample { Properties = [1, 1, 2, 2] }, new Sample { Properties = [1, 2] }, true },
        // Different sets
        { new Sample { Properties = [1, 2, 3] }, new Sample { Properties = [1, 2, 4] }, false },
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
                                public partial class SetEqualitySample
                                {
                                    [SetEquality] public List<int>? Properties { get; set; }
                                }
                                """;
}
