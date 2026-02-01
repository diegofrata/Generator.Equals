using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [Equatable] class inheriting from a non-[Equatable] base class.
/// The child should include inherited properties since the base won't handle them.
/// </summary>
public partial class InheritedFromNonEquatableTests : SnapshotTestBase
{
    // Base class WITHOUT [Equatable] - just a regular class with an attribute hint
    public abstract class Parent
    {
        [OrderedEquality]
        public virtual int[] Ints { get; set; } = null!;
    }

    // Child class WITH [Equatable] - should include Ints with inherited [OrderedEquality]
    // because Parent doesn't have [Equatable], so base.Equals() won't handle it
    [Equatable]
    public partial class Child : Parent
    {
        public override int[] Ints { get; set; } = null!;
    }

    public static TheoryData<Child, Child, bool> EqualityCases => new()
    {
        // Same array content
        { new Child { Ints = [1, 2, 3] }, new Child { Ints = [1, 2, 3] }, true },
        // Different array content
        { new Child { Ints = [1, 2, 3] }, new Child { Ints = [1, 2, 4] }, false },
        // Same content, different order (ordered equality - order matters!)
        { new Child { Ints = [1, 2, 3] }, new Child { Ints = [3, 2, 1] }, false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Child a, Child b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                public abstract class InheritedFromNonEquatableParent
                                {
                                    [OrderedEquality]
                                    public virtual int[] Ints { get; set; } = null!;
                                }

                                [Equatable]
                                public partial class InheritedFromNonEquatableChild : InheritedFromNonEquatableParent
                                {
                                    public override int[] Ints { get; set; } = null!;
                                }
                                """;
}
