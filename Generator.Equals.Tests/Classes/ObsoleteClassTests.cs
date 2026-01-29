using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality on classes marked with [Obsolete].
/// Verifies that generated code handles obsolete types without warnings.
/// </summary>
public partial class ObsoleteClassTests : SnapshotTestBase
{
#pragma warning disable CS0618
    [Equatable]
    [Obsolete("Make sure the obsolete on the object model does not add warnings")]
    public partial class Sample
    {
        public Sample(string value)
        {
            Something = value;
        }

        public string Something { get; }
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
#pragma warning restore CS0618

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                [Obsolete("Make sure the obsolete on the object model does not add warnings")]
                                public partial class ObsoleteClassSample
                                {
                                    public ObsoleteClassSample(string value)
                                    {
                                        Something = value;
                                    }

                                    public string Something { get; }
                                }
                                """;
}
