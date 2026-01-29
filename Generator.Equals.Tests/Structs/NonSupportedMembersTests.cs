using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Tests for [Equatable] struct with non-supported members (static properties, indexers).
/// These members should be ignored in equality comparison.
/// </summary>
public partial class NonSupportedMembersTests : SnapshotTestBase
{
    [Equatable]
    public partial struct Sample
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
        // Same values
        { new Sample("Dave"), new Sample("Dave"), true },
        // Different Name
        { new Sample("Dave"), new Sample("John"), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Structs;

                                [Equatable]
                                public partial struct NonSupportedMembersSample
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
