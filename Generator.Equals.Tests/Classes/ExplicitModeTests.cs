using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for explicit mode where only marked members are included in equality.
/// </summary>
public partial class ExplicitModeTests : SnapshotTestBase
{
    [Equatable(Explicit = true)]
    public partial class Sample
    {
        bool _flag;

        public Sample(string name, int age, bool flag)
        {
            _flag = flag;
            Name = name;
            Age = age;
        }

        public string Name { get; }

        [DefaultEquality]
        public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Age (the only marked member) - should be equal regardless of Name and _flag
        { new Sample("Dave", 35, true), new Sample("John", 35, false), true },
        // Different Age - should not be equal
        { new Sample("Dave", 35, true), new Sample("John", 40, false), false },
        // Same everything
        { new Sample("Dave", 35, true), new Sample("Dave", 35, true), true },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable(Explicit = true)]
                                public partial class ExplicitModeSample
                                {
                                    bool _flag;

                                    public ExplicitModeSample(string name, int age, bool flag)
                                    {
                                        _flag = flag;
                                        Name = name;
                                        Age = age;
                                    }

                                    public string Name { get; }

                                    [DefaultEquality]
                                    public int Age { get; }
                                }
                                """;
}
