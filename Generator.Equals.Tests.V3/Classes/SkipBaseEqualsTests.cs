using Generator.Equals.Tests.V3.Infrastructure;

namespace Generator.Equals.Tests.V3.Classes;

/// <summary>
/// Tests for [Equatable(SkipBaseEquals = true)] which skips calling base.Equals().
/// </summary>
public partial class SkipBaseEqualsTests : SnapshotTestBase
{
    public class SampleBase
    {
        public SampleBase(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [Equatable(Explicit = true, SkipBaseEquals = true)]
    public partial class Sample : SampleBase
    {
        public Sample(string name, int age) : base(name)
        {
            Age = age;
        }

        [DefaultEquality]
        public int Age { get; }
    }

    public static TheoryData<Sample, Sample, bool> EqualityCases => new()
    {
        // Same Age (Name is inherited and ignored because base.Equals() is skipped)
        { new Sample("Dave", 35), new Sample("John", 35), true },
        // Different Age
        { new Sample("Dave", 35), new Sample("John", 40), false },
        // Same everything
        { new Sample("Dave", 35), new Sample("Dave", 35), true },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(Sample a, Sample b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw);

    private const string SampleSource = """
        using Generator.Equals;

        namespace Generator.Equals.Tests.V3.Classes;

        public class SkipBaseEqualsSampleBase
        {
            public SkipBaseEqualsSampleBase(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }

        [Equatable(Explicit = true, SkipBaseEquals = true)]
        public partial class SkipBaseEqualsSample : SkipBaseEqualsSampleBase
        {
            public SkipBaseEqualsSample(string name, int age) : base(name)
            {
                Age = age;
            }

            [DefaultEquality]
            public int Age { get; }
        }
        """;
}
