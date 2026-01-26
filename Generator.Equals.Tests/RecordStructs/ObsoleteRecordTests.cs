using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [Equatable] record struct that is marked [Obsolete].
/// The generator should suppress obsolete warnings in generated code.
/// </summary>
public partial class ObsoleteRecordTests : SnapshotTestBase
{
    [Equatable]
    [Obsolete("Make sure the obsolete on the object model does not add warnings")]
    public partial record struct Sample(string Name);

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
        [Obsolete("Make sure the obsolete on the object model does not add warnings")]
        public partial record struct ObsoleteRecordSample(string Name);
        """;
}
