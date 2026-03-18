using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [ReferenceEquality] attribute which uses reference equality instead of value equality.
/// </summary>
public partial class ReferenceEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string name)
        {
            Name = name;
        }

        [ReferenceEquality] public string Name { get; }
    }

    [Fact]
    public void SameReference_ShouldBeEqual()
    {
        // Interned strings share the same reference
        var internedString = string.Intern($"Dave{35}");
        var a = new Sample(internedString);
        var b = new Sample(internedString);
        EqualityAssert.Verify(a, b, true);
    }

    [Fact]
    public void DifferentReferences_SameValue_ShouldNotBeEqual()
    {
        // Non-interned strings with same value have different references
        var a = new Sample($"Dave{35}");
        var b = new Sample($"Dave{35}");
        EqualityAssert.Verify(a, b, false);
    }

    [Fact]
    public void DifferentReferences_SameValue_ReportsInequality()
    {
        var nameA = $"Dave{35}";
        var nameB = $"Dave{35}";
        var a = new Sample(nameA);
        var b = new Sample(nameB);

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(nameA, nameB, Prop("Name")) });
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class ReferenceEqualitySample
                                {
                                    public ReferenceEqualitySample(string name)
                                    {
                                        Name = name;
                                    }

                                    [ReferenceEquality] public string Name { get; }
                                }
                                """;
}
