using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests that demonstrate default C# equality behavior (without [Equatable]).
/// Classes without [Equatable] use reference equality by default.
/// </summary>
public class DefaultBehaviorTests
{
    public class Sample
    {
        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }

    public static TheoryData<Func<Sample>, Func<Sample>, bool> Cases => new()
    {
        // Same reference should be equal
        { () => { var s = new Sample("Dave", 35); return s; }, () => { var s = new Sample("Dave", 35); return s; }, false },
    };

    [Fact]
    public void SameReferenceIsEqual()
    {
        var sample = new Sample("Dave", 35);
        sample.Equals(sample).Should().BeTrue();
    }

    [Fact]
    public void DifferentInstancesAreNotEqual()
    {
        var sample1 = new Sample("Dave", 35);
        var sample2 = new Sample("Dave", 35);
        sample1.Equals(sample2).Should().BeFalse("default C# classes use reference equality");
    }

    [Fact]
    public void DefaultOperatorsUseReferenceEquality()
    {
        var sample1 = new Sample("Dave", 35);
        var sample2 = new Sample("Dave", 35);
        var same = sample1;

        // Default == uses reference equality
        (sample1 == same).Should().BeTrue();
        (sample1 == sample2).Should().BeFalse();
        (sample1 != sample2).Should().BeTrue();
    }
}
