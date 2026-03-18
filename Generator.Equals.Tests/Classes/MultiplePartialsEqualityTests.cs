using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality on types defined across multiple partial declarations.
/// </summary>
public partial class MultiplePartialsEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        [OrderedEquality] public string[]? Addresses { get; set; }
    }

    public partial class Sample
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public partial class Sample
    {
        [IgnoreEquality] public int Age { get; set; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same values (Age is ignored)
        {
            new Sample { FirstName = "Dave", Age = 35, Addresses = ["10 Downing St", "Bricklane"] },
            new Sample { FirstName = "Dave", Age = 42, Addresses = ["10 Downing St", "Bricklane"] },
            true
        },
        // Different FirstName
        {
            new Sample { FirstName = "Dave", Age = 35, Addresses = ["10 Downing St"] },
            new Sample { FirstName = "John", Age = 35, Addresses = ["10 Downing St"] },
            false
        },
        // Different Addresses
        {
            new Sample { FirstName = "Dave", Addresses = ["A"] },
            new Sample { FirstName = "Dave", Addresses = ["B"] },
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
            new Sample { FirstName = "Dave", Age = 35, Addresses = ["10 Downing St"] },
            new Sample { FirstName = "John", Age = 35, Addresses = ["10 Downing St"] }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("Dave", "John", Prop("FirstName")) });
    }

    [Fact]
    public void Inequality_DifferentAddresses()
    {
        var diffs = Sample.EqualityComparer.Default.Inequalities(
            new Sample { FirstName = "Dave", Addresses = ["A"] },
            new Sample { FirstName = "Dave", Addresses = ["B"] }).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq("A", "B", Prop("Addresses"), Idx(0)) });
    }

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class MultiplePartialsSample
                                {
                                    [OrderedEquality] public string[]? Addresses { get; set; }
                                }
                                public partial class MultiplePartialsSample
                                {
                                    public string FirstName { get; set; } = string.Empty;
                                    public string LastName { get; set; } = string.Empty;
                                }
                                public partial class MultiplePartialsSample
                                {
                                    [IgnoreEquality] public int Age { get; set; }
                                }
                                """;
}
