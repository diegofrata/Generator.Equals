using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality with obsolete members.
/// Verifies that generated code handles obsolete properties without warnings.
/// </summary>
public partial class ObsoleteMembersTests : SnapshotTestBase
{
    // DO NOT ADD [Obsolete] TO THIS MODEL. It would suppress the obsoletes on the properties by itself.
    // This is why there is a separate ObsoleteClass test.
    [Equatable]
    public partial class Sample
    {
        public Sample(string value)
        {
#pragma warning disable CS0612
            NoComment = value;
#pragma warning restore CS0612
#pragma warning disable CS0618
            Comment = value;
#pragma warning restore CS0618
        }

        [Obsolete]
        public string NoComment { get; }

        [Obsolete("Having a comment causes a different error code")]
        public string Comment { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        { new Sample("Dave"), new Sample("Dave"), true },
        // Different values
        { new Sample("Dave"), new Sample("John"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class ObsoleteMembersSample
                                {
                                    public ObsoleteMembersSample(string value)
                                    {
                                #pragma warning disable CS0612
                                        NoComment = value;
                                #pragma warning restore CS0612
                                #pragma warning disable CS0618
                                        Comment = value;
                                #pragma warning restore CS0618
                                    }

                                    [Obsolete]
                                    public string NoComment { get; }

                                    [Obsolete("Having a comment causes a different error code")]
                                    public string Comment { get; }
                                }
                                """;
}
