using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for equality on generic types.
/// </summary>
public partial class GenericParameterEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class Sample<TName, TAge>
    {
        public Sample(TName name, TAge age)
        {
            Name = name;
            Age = age;
        }

        public TName Name { get; }
        public TAge Age { get; }
    }

    public static TheoryData<Sample<string, int>, Sample<string, int>, bool> EqualityCases => new()
    {
        // Same values
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("Dave", 35), true },
        // Different Age
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("Dave", 37), false },
        // Different Name
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("John", 35), false },
        // All different
        { new Sample<string, int>("Dave", 35), new Sample<string, int>("John", 40), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample<string, int> a, Sample<string, int> b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.Classes;

        [Equatable]
        public partial class GenericSample<TName, TAge>
        {
            public GenericSample(TName name, TAge age)
            {
                Name = name;
                Age = age;
            }

            public TName Name { get; }
            public TAge Age { get; }
        }
        """;
}
