using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [Equatable(IgnoreInheritedMembers = true)] which skips calling base.Equals()
/// and does not compare inherited members.
/// </summary>
public partial class IgnoreInheritedMembersTests : SnapshotTestBase
{
    public class SampleBase
    {
        public SampleBase(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [Equatable(IgnoreInheritedMembers = true)]
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
        // Same Age (Name is inherited and ignored because IgnoreInheritedMembers = true)
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
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                public class IgnoreInheritedMembersSampleBase
                                {
                                    public IgnoreInheritedMembersSampleBase(string name)
                                    {
                                        Name = name;
                                    }

                                    public string Name { get; }
                                }

                                [Equatable(Explicit = true, IgnoreInheritedMembers = true)]
                                public partial class IgnoreInheritedMembersSample : IgnoreInheritedMembersSampleBase
                                {
                                    public IgnoreInheritedMembersSample(string name, int age) : base(name)
                                    {
                                        Age = age;
                                    }

                                    [DefaultEquality]
                                    public int Age { get; }
                                }
                                """;
}
