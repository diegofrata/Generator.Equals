using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Diff tests for collection properties: ordered, unordered, set, and dictionary.
/// </summary>
public partial class CollectionDiffTests
{
    static (string Path, object? Left, object? Right) Diff(string path, object? left, object? right)
        => (path, left, right);

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

        diffs.Should().BeEquivalentTo(new[] { Diff("Items[1]", 2, 99) });
    }

    [Fact]
    public void OrderedCollection_ExtraItemInRight_ReportsNullLeft()
    {
        var a = new Sample { Items = [1, 2] };
        var b = new Sample { Items = [1, 2, 3] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Items[2]", null, 3) });
    }

    [Fact]
    public void OrderedCollection_ExtraItemInLeft_ReportsNullRight()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [1, 2] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Items[2]", 3, null) });
    }

    [Fact]
    public void OrderedCollection_MultipleDifferences_ReportsAll()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [9, 2, 8] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Items[0]", 1, 9),
            Diff("Items[2]", 3, 8)
        });
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

        diffs.Should().BeEquivalentTo(new[] { Diff("Tags[+]", null, "c") });
    }

    [Fact]
    public void UnorderedSet_RemovedItem_ReportsRemoval()
    {
        var a = new Sample { Tags = ["a", "b", "c"] };
        var b = new Sample { Tags = ["a", "b"] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Tags[-]", "c", null) });
    }

    [Fact]
    public void UnorderedSet_MultipleChanges_ReportsAllChanges()
    {
        var a = new Sample { Tags = ["a", "b"] };
        var b = new Sample { Tags = ["b", "c"] };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Tags[-]", "a", null),
            Diff("Tags[+]", null, "c")
        });
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

        diffs.Should().BeEquivalentTo(new[] { Diff("Properties[y]", null, 2) });
    }

    [Fact]
    public void Dictionary_RemovedKey_ReportsRemoval()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Properties[y]", 2, null) });
    }

    [Fact]
    public void Dictionary_ChangedValue_ReportsChange()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 99 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Diff("Properties[x]", 1, 99) });
    }

    [Fact]
    public void Dictionary_NullVsNonNull_ReportsAllKeysAdded()
    {
        var a = new Sample { Properties = null };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Properties[x]", null, 1),
            Diff("Properties[y]", null, 2)
        });
    }

    [Fact]
    public void Dictionary_NonNullVsNull_ReportsAllKeysRemoved()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = null };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Properties[x]", 1, null),
            Diff("Properties[y]", 2, null)
        });
    }

    [Fact]
    public void Dictionary_MultipleChanges_ReportsAll()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 99, ["z"] = 3 } };

        var diffs = Sample.EqualityComparer.Default.Diff(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Diff("Properties[x]", 1, 99),
            Diff("Properties[y]", 2, null),
            Diff("Properties[z]", null, 3)
        });
    }

    #endregion
}
