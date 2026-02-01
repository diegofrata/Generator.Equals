using FluentAssertions;

namespace Generator.Equals.Tests.Diff.Classes;

/// <summary>
/// Diff tests for collection properties: ordered, unordered, set, and dictionary.
/// </summary>
public partial class CollectionDiffTests
{
    [Equatable]
    public partial class Sample
    {
        [OrderedEquality]
        public List<int>? Items { get; set; }

        [UnorderedEquality]
        public HashSet<string>? Tags { get; set; }

        [UnorderedEquality]
        public Dictionary<string, int>? Properties { get; set; }
    }

    #region Ordered Collection Tests

    [Fact]
    public void OrderedCollection_SameItems_NoDifferences()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [1, 2, 3] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void OrderedCollection_DifferentItem_ReportsIndex()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [1, 99, 3] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Items[1]");
        diffs[0].Left.Should().Be(2);
        diffs[0].Right.Should().Be(99);
    }

    [Fact]
    public void OrderedCollection_ExtraItemInRight_ReportsNullLeft()
    {
        var a = new Sample { Items = [1, 2] };
        var b = new Sample { Items = [1, 2, 3] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Items[2]");
        diffs[0].Left.Should().BeNull();
        diffs[0].Right.Should().Be(3);
    }

    [Fact]
    public void OrderedCollection_ExtraItemInLeft_ReportsNullRight()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [1, 2] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Items[2]");
        diffs[0].Left.Should().Be(3);
        diffs[0].Right.Should().BeNull();
    }

    #endregion

    #region Unordered Collection (Set) Tests

    [Fact]
    public void UnorderedSet_SameItems_NoDifferences()
    {
        var a = new Sample { Tags = ["a", "b", "c"] };
        var b = new Sample { Tags = ["c", "b", "a"] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void UnorderedSet_AddedItem_ReportsAddition()
    {
        var a = new Sample { Tags = ["a", "b"] };
        var b = new Sample { Tags = ["a", "b", "c"] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Tags[+]");
        diffs[0].Left.Should().BeNull();
        diffs[0].Right.Should().Be("c");
    }

    [Fact]
    public void UnorderedSet_RemovedItem_ReportsRemoval()
    {
        var a = new Sample { Tags = ["a", "b", "c"] };
        var b = new Sample { Tags = ["a", "b"] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Tags[-]");
        diffs[0].Left.Should().Be("c");
        diffs[0].Right.Should().BeNull();
    }

    [Fact]
    public void UnorderedSet_MultipleChanges_ReportsAllChanges()
    {
        var a = new Sample { Tags = ["a", "b"] };
        var b = new Sample { Tags = ["b", "c"] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "Tags[-]" && (string?)d.Left == "a");
        diffs.Should().Contain(d => d.Path == "Tags[+]" && (string?)d.Right == "c");
    }

    #endregion

    #region Dictionary Tests

    [Fact]
    public void Dictionary_SameItems_NoDifferences()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["y"] = 2, ["x"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Dictionary_AddedKey_ReportsAddition()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Properties[y]");
        diffs[0].Left.Should().BeNull();
        diffs[0].Right.Should().Be(2);
    }

    [Fact]
    public void Dictionary_RemovedKey_ReportsRemoval()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Properties[y]");
        diffs[0].Left.Should().Be(2);
        diffs[0].Right.Should().BeNull();
    }

    [Fact]
    public void Dictionary_ChangedValue_ReportsChange()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 99 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().ContainSingle();
        diffs[0].Path.Should().Be("Properties[x]");
        diffs[0].Left.Should().Be(1);
        diffs[0].Right.Should().Be(99);
    }

    [Fact]
    public void Dictionary_NullVsNonNull_ReportsAllKeysAdded()
    {
        var a = new Sample { Properties = null };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "Properties[x]" && d.Left == null && (int?)d.Right == 1);
        diffs.Should().Contain(d => d.Path == "Properties[y]" && d.Left == null && (int?)d.Right == 2);
    }

    [Fact]
    public void Dictionary_NonNullVsNull_ReportsAllKeysRemoved()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = null };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d => d.Path == "Properties[x]" && (int?)d.Left == 1 && d.Right == null);
        diffs.Should().Contain(d => d.Path == "Properties[y]" && (int?)d.Left == 2 && d.Right == null);
    }

    #endregion
}
