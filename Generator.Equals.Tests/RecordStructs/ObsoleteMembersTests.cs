using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [Equatable] record struct with [Obsolete] properties.
/// The generator should suppress obsolete warnings when accessing these properties.
/// </summary>
public partial class ObsoleteMembersTests : SnapshotTestBase
{
    // DO NOT ADD [Obsolete] TO THIS MODEL. It would suppress the obsoletes on the properties by itself.
    // This is why there is a separate ObsoleteRecord test.
    [Equatable]
    public partial record struct Sample(
        [property: Obsolete] string NoComment,
        [property: Obsolete("a comment")] string Comment
    );

#pragma warning disable CS0618 // Type or member is obsolete
    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        { new Sample("A", "B"), new Sample("A", "B"), true },
        // Different NoComment
        { new Sample("A", "B"), new Sample("X", "B"), false },
        // Different Comment
        { new Sample("A", "B"), new Sample("A", "Y"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);
#pragma warning restore CS0618

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using System;
        using Generator.Equals;

        namespace Generator.Equals.Tests.RecordStructs;

        [Equatable]
        public partial record struct ObsoleteMembersSample(
            [property: Obsolete] string NoComment,
            [property: Obsolete("a comment")] string Comment
        );
        """;
}
