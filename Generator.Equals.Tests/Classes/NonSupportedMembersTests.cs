using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests that non-supported members (static properties, indexers) are properly ignored.
/// </summary>
public partial class NonSupportedMembersTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static int StaticProperty { get; }

        public int this[int index] => index;
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Name
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

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class NonSupportedMembersSample
                                {
                                    public NonSupportedMembersSample(string name)
                                    {
                                        Name = name;
                                    }

                                    public string Name { get; }

                                    public static int StaticProperty { get; }

                                    public int this[int index] => index;
                                }
                                """;
}
