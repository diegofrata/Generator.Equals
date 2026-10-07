using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] record inheriting from a non-[Equatable] base record. A record base always owns its
/// equality (compiler-synthesized here), so the child delegates to base.Equals() rather than re-comparing the
/// base's members, and Inequalities reports the base portion as one whole-object inequality.
/// </summary>
public partial class InheritedFromNonEquatableTests : SnapshotTestBase
{
    // Base record WITHOUT [Equatable] - just a regular record with an attribute hint
    public abstract record Parent
    {
        [OrderedEquality]
        public virtual int[] Ints { get; init; } = null!;
    }

    // Child record WITH [Equatable] - its override of Ints is compared here with the inherited
    // [OrderedEquality]; Parent's own (null) backing field is covered by base.Equals()
    [Equatable]
    public partial record Child : Parent
    {
        public override int[] Ints { get; init; } = null!;
    }

    public record PlainBase(int A);

    [Equatable]
    public partial record Leaf(int A, int B) : PlainBase(A);

    public static TheoryData<Leaf, Leaf, bool> LeafCases => new()
    {
        { new Leaf(1, 2), new Leaf(1, 2), true },
        // Base member differs -> detected via base.Equals()
        { new Leaf(1, 2), new Leaf(9, 2), false },
        // Leaf member differs
        { new Leaf(1, 2), new Leaf(1, 9), false },
    };

    [Theory]
    [MemberData(nameof(LeafCases))]
    public void LeafEquality(Leaf a, Leaf b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void PlainBaseMember_IsReportedCoarselyByInequalities()
    {
        var a = new Leaf(1, 2);

        // The plain base is opaque to the comparer, so its portion is ONE whole-object inequality...
        Leaf.EqualityComparer.Default.Inequalities(a, new Leaf(9, 2)).Should()
            .Equal([Ineq(a, new Leaf(9, 2))], "a plain record base is reported coarsely");

        // ...while the leaf's own members keep per-member blame.
        Leaf.EqualityComparer.Default.Inequalities(a, new Leaf(1, 9)).Should()
            .Equal([Ineq(2, 9, Prop("B"))]);
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

                                public record InheritedFromNonEquatablePlainBase(int A);

                                [Equatable]
                                public partial record InheritedFromNonEquatableLeaf(int A, int B) : InheritedFromNonEquatablePlainBase(A);
                                """;
}
