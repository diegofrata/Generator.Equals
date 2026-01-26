using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.RecordStructs;

/// <summary>
/// Tests for [Equatable] record struct with non-supported members (static properties, indexers).
/// These members should be ignored in equality comparison.
/// </summary>
public partial class NonSupportedMembersTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct Sample(string Name)
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
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.RecordStructs;

        [Equatable]
        public partial record struct NonSupportedMembersSample(string Name)
        {
            public static int StaticProperty { get; }

            public int this[int index] => index;
        }
        """;
}
