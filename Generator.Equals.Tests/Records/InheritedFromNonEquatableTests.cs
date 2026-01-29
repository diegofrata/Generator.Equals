using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] record inheriting from a non-[Equatable] base record.
/// The child should include inherited properties since the base won't handle them.
/// </summary>
public partial class InheritedFromNonEquatableTests : SnapshotTestBase
{
    // Base record WITHOUT [Equatable] - just a regular record with an attribute hint
    public abstract record Parent
    {
        [OrderedEquality]
        public virtual int[] Ints { get; init; } = null!;
    }

    // Child record WITH [Equatable] - should include Ints with inherited [OrderedEquality]
    // because Parent doesn't have [Equatable], so base.Equals() won't handle it
    [Equatable]
    public partial record Child : Parent
    {
        public override int[] Ints { get; init; } = null!;
    }

    public static TheoryData<Child, Child, bool> EqualityCases => new()
    {
        // Same array content
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 1, 2, 3 } }, true },
        // Different array content
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 1, 2, 4 } }, false },
        // Same content, different order (ordered equality - order matters!)
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 3, 2, 1 } }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Child a, Child b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                public abstract record InheritedFromNonEquatableParent
                                {
                                    [OrderedEquality]
                                    public virtual int[] Ints { get; init; } = null!;
                                }

                                [Equatable]
                                public partial record InheritedFromNonEquatableChild : InheritedFromNonEquatableParent
                                {
                                    public override int[] Ints { get; init; } = null!;
                                }
                                """;
}
