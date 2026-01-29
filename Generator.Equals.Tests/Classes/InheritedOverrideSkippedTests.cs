using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests that when both parent and child have [Equatable], the override is skipped
/// in child and comparison is handled via base.Equals().
/// </summary>
public partial class InheritedOverrideSkippedTests : SnapshotTestBase
{
    // Base class with [Equatable] and OrderedEquality
    [Equatable]
    public abstract partial class Parent
    {
        [OrderedEquality]
        public virtual int[] Ints { get; set; } = null!;
    }

    // Child class with [Equatable] - override should be skipped
    // because Parent has [Equatable] and handles it via base.Equals()
    [Equatable]
    public partial class Child : Parent
    {
        public override int[] Ints { get; set; } = null!;
    }

    public static TheoryData<Child, Child, bool> EqualityCases => new()
    {
        // Same array content
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 1, 2, 3 } }, true },
        // Same content, different order (ordered equality - order matters!)
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 3, 2, 1 } }, false },
        // Different content
        { new Child { Ints = new[] { 1, 2, 3 } }, new Child { Ints = new[] { 1, 2, 4 } }, false },
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

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public abstract partial class InheritedOverrideSkippedParent
                                {
                                    [OrderedEquality]
                                    public virtual int[] Ints { get; set; } = null!;
                                }

                                [Equatable]
                                public partial class InheritedOverrideSkippedChild : InheritedOverrideSkippedParent
                                {
                                    public override int[] Ints { get; set; } = null!;
                                }
                                """;
}
