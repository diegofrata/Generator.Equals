using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] record that is marked [Obsolete].
/// The generator should suppress obsolete warnings in generated code.
/// </summary>
public partial class ObsoleteRecordTests : SnapshotTestBase
{
    [Equatable]
    [Obsolete("Make sure the obsolete on the object model does not add warnings")]
    public partial record Sample(string Name);

#pragma warning disable CS0618 // Type or member is obsolete
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

    [Fact]
    public void Inequality_DifferentName()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample("Dave"), new Sample("John")).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Dave", "John", Prop("Name")) });
    }
#pragma warning restore CS0618

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                [Obsolete("Make sure the obsolete on the object model does not add warnings")]
                                public partial record ObsoleteRecordSample(string Name);
                                """;
}
