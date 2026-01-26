using FluentAssertions;
using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Structs;

/// <summary>
/// Tests for default struct equality (no [Equatable] attribute).
/// Without [Equatable], structs use default value equality and don't have custom == operators.
/// </summary>
public partial class DefaultBehaviorTests : SnapshotTestBase
{
    public struct Sample
    {
        public Sample(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; }
        public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
        // Different Name
        { new Sample("Dave", 35), new Sample("John", 35), false },
        // Different Age
        { new Sample("Dave", 35), new Sample("Dave", 40), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected)
    {
        // Without [Equatable], structs use default value equality
        // They don't have custom == operators, so we just test Equals()
        a.Equals(b).Should().Be(expected);
    }
}
