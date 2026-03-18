using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] record with non-supported members (static properties, indexers).
/// These members should be ignored in equality comparison.
/// </summary>
public partial class NonSupportedMembersTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample(string Name)
    {
        public static int StaticProperty { get; }

        public int this[int index] => index;
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        { new Sample("Dave"), new Sample("Dave"), true },
        // Different Name
        { new Sample("Dave"), new Sample("John"), false },
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
    public void Inequality_DifferentName()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave"), new Sample("John")).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Dave", "John", Prop("Name")) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record NonSupportedMembersSample(string Name)
                                {
                                    public static int StaticProperty { get; }

                                    public int this[int index] => index;
                                }
                                """;
}
