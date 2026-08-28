using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Structs;

/// <summary>
/// Fields are opt-in. A plain, un-annotated field must NOT participate in equality by default;
/// only properties are compared by default. A field joins the comparison when it carries
/// <c>[DefaultEquality]</c> (or another equality attribute). This is the intended v5 contract and a
/// regression guard for issue #86 - between 3.2.0 and 4.x a refactor silently began comparing plain
/// fields, and no test covered the plain-field case to catch it.
/// </summary>
public partial class PlainFieldExclusionTests
{
    [Equatable]
    public partial struct Sample
    {
        public Sample(string id, string note)
        {
            Id = id;
            _note = note;
        }

        public string Id { get; }   // property: compared by default
        readonly string _note;      // plain field: excluded by default
    }

    [Fact]
    public void PlainField_DiffersOnly_StillEqual()
    {
        // The two instances differ only in the un-annotated field, so they must be equal.
        EqualityAssert.VerifyStruct(new Sample("same", "note-a"), new Sample("same", "note-b"), expectedEqual: true);
    }

    [Fact]
    public void Property_Differs_NotEqual()
    {
        // Sanity check: the property is still compared by default.
        EqualityAssert.VerifyStruct(new Sample("a", "note"), new Sample("b", "note"), expectedEqual: false);
    }

    [Equatable]
    public partial struct OptInSample
    {
        public OptInSample(string id, string note)
        {
            Id = id;
            _note = note;
        }

        public string Id { get; }
        [DefaultEquality] readonly string _note;   // opted back in explicitly
    }

    [Fact]
    public void OptedInField_Differs_NotEqual()
    {
        // With [DefaultEquality] the field participates in the comparison again.
        EqualityAssert.VerifyStruct(new OptInSample("same", "note-a"), new OptInSample("same", "note-b"), expectedEqual: false);
    }
}
