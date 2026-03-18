using System.Collections.Immutable;
using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality attributes on immutable reference-type collections
/// (ImmutableList, ImmutableHashSet). These are reference types unlike ImmutableArray,
/// so they go through the nullable code paths.
/// </summary>
public partial class ImmutableCollectionEqualityTests
{
    [Equatable]
    public partial class OrderedSample
    {
        [OrderedEquality] public ImmutableList<string>? Items { get; set; }
    }

    [Equatable]
    public partial class SetSample
    {
        [SetEquality] public ImmutableHashSet<string>? Tags { get; set; }
    }

    #region ImmutableList — OrderedEquality

    [Fact]
    public void OrderedList_SameContent_Equal()
    {
        var a = new OrderedSample { Items = ["A", "B", "C"] };
        var b = new OrderedSample { Items = ["A", "B", "C"] };

        EqualityAssert.Verify(a, b, expectedEqual: true);
    }

    [Fact]
    public void OrderedList_DifferentOrder_NotEqual()
    {
        var a = new OrderedSample { Items = ["A", "B"] };
        var b = new OrderedSample { Items = ["B", "A"] };

        EqualityAssert.Verify(a, b, expectedEqual: false);
    }

    [Fact]
    public void OrderedList_DifferentContent_NotEqual()
    {
        var a = new OrderedSample { Items = ["A"] };
        var b = new OrderedSample { Items = ["B"] };

        EqualityAssert.Verify(a, b, expectedEqual: false);
    }

    [Fact]
    public void OrderedList_BothNull_Equal()
    {
        var a = new OrderedSample { Items = null };
        var b = new OrderedSample { Items = null };

        EqualityAssert.Verify(a, b, expectedEqual: true);
    }

    [Fact]
    public void OrderedList_NullVsNonNull_NotEqual()
    {
        var a = new OrderedSample { Items = null };
        var b = new OrderedSample { Items = ["A"] };

        EqualityAssert.Verify(a, b, expectedEqual: false);
    }

    [Fact]
    public void OrderedList_Inequality_DifferentContent()
    {
        var a = new OrderedSample { Items = ["A"] };
        var b = new OrderedSample { Items = ["B"] };

        var diffs = OrderedSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("A", "B", Prop("Items"), Idx(0)) });
    }

    [Fact]
    public void OrderedList_Inequality_DifferentLength()
    {
        var a = new OrderedSample { Items = ["A", "B"] };
        var b = new OrderedSample { Items = ["A"] };

        var diffs = OrderedSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("B", null, Prop("Items"), Idx(1)) });
    }

    #endregion

    #region ImmutableHashSet — SetEquality

    [Fact]
    public void HashSet_SameContent_Equal()
    {
        var a = new SetSample { Tags = ["X", "Y", "Z"] };
        var b = new SetSample { Tags = ["Z", "Y", "X"] };

        EqualityAssert.Verify(a, b, expectedEqual: true);
    }

    [Fact]
    public void HashSet_DifferentContent_NotEqual()
    {
        var a = new SetSample { Tags = ["X", "Y"] };
        var b = new SetSample { Tags = ["X", "Z"] };

        EqualityAssert.Verify(a, b, expectedEqual: false);
    }

    [Fact]
    public void HashSet_BothNull_Equal()
    {
        var a = new SetSample { Tags = null };
        var b = new SetSample { Tags = null };

        EqualityAssert.Verify(a, b, expectedEqual: true);
    }

    [Fact]
    public void HashSet_Inequality_AddedAndRemoved()
    {
        var a = new SetSample { Tags = ["X", "Y"] };
        var b = new SetSample { Tags = ["X", "Z"] };

        var diffs = SetSample.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[]
        {
            Ineq("Y", null, Prop("Tags"), MemberPathSegment.Removed()),
            Ineq(null, "Z", Prop("Tags"), MemberPathSegment.Added())
        });
    }

    #endregion
}
