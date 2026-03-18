using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [Equatable] record split across multiple partial declarations.
/// </summary>
public partial class MultiplePartialsEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record Sample
    {
        [OrderedEquality] public string[]? Addresses { get; init; }
    }

    public partial record Sample
    {
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
    }

    public partial record Sample
    {
        [IgnoreEquality] public int Age { get; init; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values
        {
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            true
        },
        // Same values, different Age (ignored)
        {
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 40 },
            true
        },
        // Different FirstName
        {
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = ["A"], FirstName = "Jane", LastName = "Doe", Age = 30 },
            false
        },
        // Different Addresses
        {
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = ["B"], FirstName = "John", LastName = "Doe", Age = 30 },
            false
        },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    [Fact]
    public void Inequality_DifferentFirstName()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = ["A"], FirstName = "Jane", LastName = "Doe", Age = 30 }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("John", "Jane", Prop("FirstName")) });
    }

    [Fact]
    public void Inequality_DifferentAddresses()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample { Addresses = ["A"], FirstName = "John", LastName = "Doe", Age = 30 },
            new Sample { Addresses = ["B"], FirstName = "John", LastName = "Doe", Age = 30 }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("A", "B", Prop("Addresses"), Idx(0)) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record MultiplePartialsEqualitySample
                                {
                                    [OrderedEquality] public string[]? Addresses { get; init; }
                                }

                                public partial record MultiplePartialsEqualitySample
                                {
                                    public string FirstName { get; init; } = string.Empty;
                                    public string LastName { get; init; } = string.Empty;
                                }

                                public partial record MultiplePartialsEqualitySample
                                {
                                    [IgnoreEquality] public int Age { get; init; }
                                }
                                """;
}
