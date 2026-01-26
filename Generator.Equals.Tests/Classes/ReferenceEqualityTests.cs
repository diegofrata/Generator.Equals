using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [ReferenceEquality] attribute which uses reference equality instead of value equality.
/// </summary>
public partial class ReferenceEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample
    {
        public Sample(string name)
        {
            Name = name;
        }

        [ReferenceEquality] public string Name { get; }
    }

    [Fact]
    public void SameReference_ShouldBeEqual()
    {
        // Interned strings share the same reference
        var internedString = string.Intern($"Dave{35}");
        var a = new Sample(internedString);
        var b = new Sample(internedString);
        EqualityAssert.Verify(a, b, true);
    }

    [Fact]
    public void DifferentReferences_SameValue_ShouldNotBeEqual()
    {
        // Non-interned strings with same value have different references
        var a = new Sample($"Dave{35}");
        var b = new Sample($"Dave{35}");
        EqualityAssert.Verify(a, b, false);
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.Classes;

        [Equatable]
        public partial class ReferenceEqualitySample
        {
            public ReferenceEqualitySample(string name)
            {
                Name = name;
            }

            [ReferenceEquality] public string Name { get; }
        }
        """;
}
