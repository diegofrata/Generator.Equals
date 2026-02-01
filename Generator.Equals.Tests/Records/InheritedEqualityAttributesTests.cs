using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for inheritance of equality attributes from parent to child record.
/// Child can inherit parent's attribute or override with its own.
/// </summary>
public partial class InheritedEqualityAttributesTests : SnapshotTestBase
{
    // Base record with OrderedEquality on a virtual property
    [Equatable(IgnoreInheritedMembers = true)]
    public abstract partial record Parent
    {
        [OrderedEquality]
        public virtual int[] Ints { get; init; } = null!;
    }

    // Child record that overrides the property - should inherit [OrderedEquality]
    [Equatable(IgnoreInheritedMembers = true)]
    public partial record Child : Parent
    {
        public override int[] Ints { get; init; } = null!;
    }

    // Child that overrides with its own attribute (should use child's attribute)
    [Equatable(IgnoreInheritedMembers = true)]
    public partial record ChildWithOwnAttribute : Parent
    {
        [UnorderedEquality]
        public override int[] Ints { get; init; } = null!;
    }

    public static TheoryData<Child, Child, bool> ChildEqualityCases => new()
    {
        // Same array content
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 1, 2, 3 } }, true },
        // Different content
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 1, 2, 4 } }, false },
        // Same content, different order (ordered equality - order matters!)
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 3, 2, 1 } }, false },
    };

    [Theory]
    [MemberData(nameof(ChildEqualityCases))]
    public void ChildEquality(Child a, Child b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<ChildWithOwnAttribute, ChildWithOwnAttribute, bool> ChildWithOwnAttributeCases => new()
    {
        // Same content, different order (unordered equality - order doesn't matter!)
        { new ChildWithOwnAttribute { Ints = new[] { 1, 2, 3 } }, new ChildWithOwnAttribute { Ints = new[] { 3, 2, 1 } }, true },
        // Same content
        { new ChildWithOwnAttribute { Ints = new[] { 1, 2, 3 } }, new ChildWithOwnAttribute { Ints = new[] { 1, 2, 3 } }, true },
        // Different content
        { new ChildWithOwnAttribute { Ints = new[] { 1, 2, 3 } }, new ChildWithOwnAttribute { Ints = new[] { 1, 2, 4 } }, false },
    };

    [Theory]
    [MemberData(nameof(ChildWithOwnAttributeCases))]
    public void ChildWithOwnAttributeEquality(ChildWithOwnAttribute a, ChildWithOwnAttribute b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable(IgnoreInheritedMembers = true)]
                                public abstract partial record InheritedEqualityAttributesParent
                                {
                                    [OrderedEquality]
                                    public virtual int[] Ints { get; init; } = null!;
                                }

                                [Equatable(IgnoreInheritedMembers = true)]
                                public partial record InheritedEqualityAttributesChild : InheritedEqualityAttributesParent
                                {
                                    public override int[] Ints { get; init; } = null!;
                                }

                                [Equatable(IgnoreInheritedMembers = true)]
                                public partial record InheritedEqualityAttributesChildWithOwnAttribute : InheritedEqualityAttributesParent
                                {
                                    [UnorderedEquality]
                                    public override int[] Ints { get; init; } = null!;
                                }
                                """;
}
