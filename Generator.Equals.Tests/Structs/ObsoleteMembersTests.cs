using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [Equatable] struct with [Obsolete] properties.
/// The generator should suppress obsolete warnings when accessing these properties.
/// </summary>
public partial class ObsoleteMembersTests : SnapshotTestBase
{
    // DO NOT ADD [Obsolete] TO THIS MODEL. It would suppress the obsoletes on the properties by itself.
    // This is why there is a separate ObsoleteStruct test.
    [Equatable]
    public partial struct Sample
    {
        public Sample(string noComment, string comment)
        {
            NoComment = noComment;
            Comment = comment;
        }

        [Obsolete] public string NoComment { get; }
        [Obsolete("a comment")] public string Comment { get; }
    }

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
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct ObsoleteMembersSample
                                {
                                    public ObsoleteMembersSample(string noComment, string comment)
                                    {
                                        NoComment = noComment;
                                        Comment = comment;
                                    }

                                    [Obsolete] public string NoComment { get; }
                                    [Obsolete("a comment")] public string Comment { get; }
                                }
                                """;
}
