using FluentAssertions;

namespace Generator.Equals.Tests.Classes.Diff;

/// <summary>
/// Diff tests for collection properties: ordered, unordered, set, and dictionary.
/// </summary>
public partial class CollectionDiffTests
{
    static MemberPathSegment Prop(string name) => MemberPathSegment.Property(name);
    static MemberPathSegment Idx(int i) => MemberPathSegment.Index(i);
    static MemberPathSegment DKey(object k) => MemberPathSegment.Key(k);

    static Inequality Ineq(object? left, object? right, params MemberPathSegment[] path)
        => new(new MemberPath(path), left, right);

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

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void OrderedCollection_DifferentItem_ReportsIndex()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [1, 99, 3] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(2, 99, Prop("Items"), Idx(1)) });
    }

    [Fact]
    public void OrderedCollection_ExtraItemInRight_ReportsNullLeft()
    {
        var a = new Sample { Items = [1, 2] };
        var b = new Sample { Items = [1, 2, 3] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, 3, Prop("Items"), Idx(2)) });
    }

    [Fact]
    public void OrderedCollection_ExtraItemInLeft_ReportsNullRight()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [1, 2] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(3, null, Prop("Items"), Idx(2)) });
    }

    [Fact]
    public void OrderedCollection_MultipleDifferences_ReportsAll()
    {
        var a = new Sample { Items = [1, 2, 3] };
        var b = new Sample { Items = [9, 2, 8] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(1, 9, Prop("Items"), Idx(0)),
            Ineq(3, 8, Prop("Items"), Idx(2))
        });
    }

    #endregion

    #region Unordered Collection (Set) Tests

    [Fact]
    public void UnorderedSet_SameItems_NoDifferences()
    {
        var a = new Sample { Tags = ["a", "b", "c"] };
        var b = new Sample { Tags = ["c", "b", "a"] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void UnorderedSet_AddedItem_ReportsAddition()
    {
        var a = new Sample { Tags = ["a", "b"] };
        var b = new Sample { Tags = ["a", "b", "c"] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, "c", Prop("Tags"), MemberPathSegment.Added()) });
    }

    [Fact]
    public void UnorderedSet_RemovedItem_ReportsRemoval()
    {
        var a = new Sample { Tags = ["a", "b", "c"] };
        var b = new Sample { Tags = ["a", "b"] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("c", null, Prop("Tags"), MemberPathSegment.Removed()) });
    }

    [Fact]
    public void UnorderedSet_MultipleChanges_ReportsAllChanges()
    {
        var a = new Sample { Tags = ["a", "b"] };
        var b = new Sample { Tags = ["b", "c"] };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("a", null, Prop("Tags"), MemberPathSegment.Removed()),
            Ineq(null, "c", Prop("Tags"), MemberPathSegment.Added())
        });
    }

    #endregion

    #region Dictionary Tests

    [Fact]
    public void Dictionary_SameItems_NoDifferences()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["y"] = 2, ["x"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEmpty();
    }

    [Fact]
    public void Dictionary_AddedKey_ReportsAddition()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, 2, Prop("Properties"), DKey("y")) });
    }

    [Fact]
    public void Dictionary_RemovedKey_ReportsRemoval()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(2, null, Prop("Properties"), DKey("y")) });
    }

    [Fact]
    public void Dictionary_ChangedValue_ReportsChange()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 99 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(1, 99, Prop("Properties"), DKey("x")) });
    }

    [Fact]
    public void Dictionary_NullVsNonNull_ReportsAllKeysAdded()
    {
        var a = new Sample { Properties = null };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(null, 1, Prop("Properties"), DKey("x")),
            Ineq(null, 2, Prop("Properties"), DKey("y"))
        });
    }

    [Fact]
    public void Dictionary_NonNullVsNull_ReportsAllKeysRemoved()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = null };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(1, null, Prop("Properties"), DKey("x")),
            Ineq(2, null, Prop("Properties"), DKey("y"))
        });
    }

    [Fact]
    public void Dictionary_MultipleChanges_ReportsAll()
    {
        var a = new Sample { Properties = new Dictionary<string, int> { ["x"] = 1, ["y"] = 2 } };
        var b = new Sample { Properties = new Dictionary<string, int> { ["x"] = 99, ["z"] = 3 } };

        var diffs = Sample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(1, 99, Prop("Properties"), DKey("x")),
            Ineq(2, null, Prop("Properties"), DKey("y")),
            Ineq(null, 3, Prop("Properties"), DKey("z"))
        });
    }

    #endregion

    #region Dictionary with non-string keys

    [Equatable]
    public partial class IntKeyDictSample
    {
        [UnorderedEquality]
        public Dictionary<int, string>? Scores { get; set; }
    }

    [Fact]
    public void IntKeyDictionary_AddedKey_ReportsUnquotedKey()
    {
        var a = new IntKeyDictSample { Scores = new() { [1] = "low" } };
        var b = new IntKeyDictSample { Scores = new() { [1] = "low", [42] = "high" } };

        var diffs = IntKeyDictSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(null, "high", Prop("Scores"), DKey(42)) });
    }

    [Fact]
    public void IntKeyDictionary_ChangedAndRemoved_ReportsAll()
    {
        var a = new IntKeyDictSample { Scores = new() { [1] = "low", [2] = "mid" } };
        var b = new IntKeyDictSample { Scores = new() { [1] = "high" } };

        var diffs = IntKeyDictSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("low", "high", Prop("Scores"), DKey(1)),
            Ineq("mid", null, Prop("Scores"), DKey(2))
        });
    }

    #endregion

    #region Set with complex (non-string) elements

    [Equatable]
    public partial class IntSetSample
    {
        [SetEquality]
        public HashSet<int>? Numbers { get; set; }
    }

    [Fact]
    public void IntSet_AddedAndRemoved_ReportsItemsAsValues()
    {
        var a = new IntSetSample { Numbers = [1, 2, 3] };
        var b = new IntSetSample { Numbers = [2, 3, 4] };

        var diffs = IntSetSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        // Sets report removed/added items as Left/Right values, path is just the property
        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(1, null, Prop("Numbers"), MemberPathSegment.Removed()),
            Ineq(null, 4, Prop("Numbers"), MemberPathSegment.Added())
        });
    }

    [Equatable]
    public partial class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    [Equatable]
    public partial class CoordinateSetSample
    {
        [SetEquality]
        public HashSet<Coordinate>? Points { get; set; }
    }

    [Fact]
    public void ComplexObjectSet_AddedAndRemoved_ReportsObjectsAsValues()
    {
        var p1 = new Coordinate { X = 0, Y = 0 };
        var p2 = new Coordinate { X = 1, Y = 1 };
        var p3 = new Coordinate { X = 2, Y = 2 };

        var a = new CoordinateSetSample { Points = [p1, p2] };
        var b = new CoordinateSetSample { Points = [p2, p3] };

        var diffs = CoordinateSetSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        // Complex objects in sets: path includes [+]/[-] markers
        diffs.Should().HaveCount(2);
        diffs.Should().Contain(d =>
            d.Path == new MemberPath(new[] { Prop("Points"), MemberPathSegment.Removed() })
            && d.Left == p1 && d.Right == null);
        diffs.Should().Contain(d =>
            d.Path == new MemberPath(new[] { Prop("Points"), MemberPathSegment.Added() })
            && d.Left == null && d.Right == p3);
    }

    #endregion

    #region Dictionary with equatable value type (drill-down)

    [Equatable]
    public partial class CoordinateDictSample
    {
        [UnorderedEquality]
        public Dictionary<string, Coordinate>? Points { get; set; }
    }

    [Fact]
    public void EquatableValueDict_ValueChanged_DrillsDownToPropertyDiffs()
    {
        var a = new CoordinateDictSample
        {
            Points = new() { ["origin"] = new Coordinate { X = 0, Y = 0 } }
        };
        var b = new CoordinateDictSample
        {
            Points = new() { ["origin"] = new Coordinate { X = 5, Y = 0 } }
        };

        var diffs = CoordinateDictSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(0, 5, Prop("Points"), DKey("origin"), Prop("X"))
        });
    }

    [Fact]
    public void EquatableValueDict_KeyAdded_ReportsWholeValue()
    {
        var a = new CoordinateDictSample
        {
            Points = new() { ["origin"] = new Coordinate { X = 0, Y = 0 } }
        };
        var newPoint = new Coordinate { X = 1, Y = 1 };
        var b = new CoordinateDictSample
        {
            Points = new() { ["origin"] = new Coordinate { X = 0, Y = 0 }, ["other"] = newPoint }
        };

        var diffs = CoordinateDictSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq(null, newPoint, Prop("Points"), DKey("other"))
        });
    }

    [Fact]
    public void EquatableValueDict_KeyRemoved_ReportsWholeValue()
    {
        var origin = new Coordinate { X = 0, Y = 0 };
        var a = new CoordinateDictSample
        {
            Points = new() { ["origin"] = origin, ["other"] = new Coordinate { X = 1, Y = 1 } }
        };
        var b = new CoordinateDictSample
        {
            Points = new() { ["origin"] = new Coordinate { X = 0, Y = 0 } }
        };

        var diffs = CoordinateDictSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().HaveCount(1);
        diffs[0].Path.Should().Be(new MemberPath(new[] { Prop("Points"), DKey("other") }));
        diffs[0].Left.Should().NotBeNull();
        diffs[0].Right.Should().BeNull();
    }

    [Fact]
    public void EquatableValueDict_MixedChanges_ReportsAll()
    {
        var a = new CoordinateDictSample
        {
            Points = new()
            {
                ["origin"] = new Coordinate { X = 0, Y = 0 },
                ["removed"] = new Coordinate { X = 9, Y = 9 }
            }
        };
        var addedPoint = new Coordinate { X = 5, Y = 5 };
        var b = new CoordinateDictSample
        {
            Points = new()
            {
                ["origin"] = new Coordinate { X = 0, Y = 3 },
                ["added"] = addedPoint
            }
        };

        var diffs = CoordinateDictSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().HaveCount(3);
        // Value changed — drills down to Y property
        diffs.Should().Contain(d =>
            d.Path == new MemberPath(new[] { Prop("Points"), DKey("origin"), Prop("Y") })
            && (int)d.Left! == 0 && (int)d.Right! == 3);
        // Key removed — whole value
        diffs.Should().Contain(d =>
            d.Path == new MemberPath(new[] { Prop("Points"), DKey("removed") })
            && d.Left != null && d.Right == null);
        // Key added — whole value
        diffs.Should().Contain(d =>
            d.Path == new MemberPath(new[] { Prop("Points"), DKey("added") })
            && d.Left == null && d.Right == addedPoint);
    }

    #endregion
}
