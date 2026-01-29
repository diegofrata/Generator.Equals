using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable(IgnoreInheritedMembers = true)] record.
/// base.Equals() is not called, so inherited members are not compared.
/// </summary>
public partial class IgnoreInheritedMembersTests : SnapshotTestBase
{
    public partial record SampleBase(string Name);

    [Equatable(IgnoreInheritedMembers = true)]
    public partial record Sample(string Name, [property: DefaultEquality] int Age) : SampleBase(Name);

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Age (Name is ignored because IgnoreInheritedMembers = true + Explicit mode)
        { new Sample("Dave", 35), new Sample("John", 35), true },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
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
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                public partial record IgnoreInheritedMembersSampleBase(string Name);

                                [Equatable(Explicit = true, IgnoreInheritedMembers = true)]
                                public partial record IgnoreInheritedMembersSample(string Name, [property: DefaultEquality] int Age) : IgnoreInheritedMembersSampleBase(Name);
                                """;
}
