using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// The opt-in rule for fields (issue #86) must also hold for members inherited from a
/// non-[Equatable] base, which the generator pulls in via <c>CollectInheritedProperties</c>. An
/// inherited plain field must be excluded; an inherited property must still be compared; and an
/// inherited field carrying <c>[DefaultEquality]</c> must still be compared.
/// </summary>
public partial class InheritedFieldExclusionTests
{
    public abstract class Parent
    {
        public int Tag { get; set; }   // inherited property: compared by default
        public int Note;               // inherited plain field: excluded by default
    }

    [Equatable]
    public partial class Child : Parent
    {
    }

    [Fact]
    public void InheritedPlainField_DiffersOnly_StillEqual()
    {
        // Differ only in the inherited un-annotated field => equal, because it is excluded.
        EqualityAssert.Verify(
            new Child { Tag = 1, Note = 10 }, new Child { Tag = 1, Note = 20 }, expectedEqual: true);
    }

    [Fact]
    public void InheritedProperty_Differs_NotEqual()
    {
        // Sanity check: the inherited property is still compared.
        EqualityAssert.Verify(
            new Child { Tag = 1, Note = 10 }, new Child { Tag = 2, Note = 10 }, expectedEqual: false);
    }

    public abstract class OptInParent
    {
        public int Tag { get; set; }
        [DefaultEquality] public int Note;   // inherited field, opted in explicitly
    }

    [Equatable]
    public partial class OptInChild : OptInParent
    {
    }

    [Fact]
    public void InheritedOptedInField_Differs_NotEqual()
    {
        // With [DefaultEquality] the inherited field participates in the comparison again.
        EqualityAssert.Verify(
            new OptInChild { Tag = 1, Note = 10 }, new OptInChild { Tag = 1, Note = 20 }, expectedEqual: false);
    }
}
