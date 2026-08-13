using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for equality in an inheritance hierarchy where intermediate record manually overrides Equals.
/// </summary>
public partial class OverridingEqualsTests : SnapshotTestBase
{
    [Equatable]
    public partial record Person(int Age);

    public record Manager(int Age, string Department) : Person(Age)
    {
        public virtual bool Equals(Manager? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Department == other.Department;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Department);
        }
    }

    [Equatable]
    public partial record SeniorManager(int Age, string Department, int Shares) : Manager(Age, Department);

    public static TheoryData<SeniorManager, SeniorManager, bool> EqualityCases => new()
    {
        // Same values
        { new SeniorManager(25, "IT", 1000), new SeniorManager(25, "IT", 1000), true },
        // Different Shares
        { new SeniorManager(25, "IT", 1000), new SeniorManager(25, "IT", 2000), false },
        // Different Department - for records, base.Equals IS called, so Department IS compared
        { new SeniorManager(25, "IT", 1000), new SeniorManager(25, "Sales", 1000), false },
        // Different Age - for records, base.Equals IS called, so Age IS compared
        { new SeniorManager(25, "IT", 1000), new SeniorManager(30, "IT", 1000), false },
    };

    [Theory]
    [MemberData(nameof(EqualityCases))]
    public void Equality(SeniorManager a, SeniorManager b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using System;
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record OverridingEqualsPerson(int Age);

                                public record OverridingEqualsManager(int Age, string Department) : OverridingEqualsPerson(Age)
                                {
                                    public virtual bool Equals(OverridingEqualsManager? other)
                                    {
                                        if (ReferenceEquals(null, other)) return false;
                                        if (ReferenceEquals(this, other)) return true;
                                        return base.Equals(other) && Department == other.Department;
                                    }

                                    public override int GetHashCode()
                                    {
                                        return HashCode.Combine(base.GetHashCode(), Department);
                                    }
                                }

                                [Equatable]
                                public partial record OverridingEqualsSeniorManager(int Age, string Department, int Shares) : OverridingEqualsManager(Age, Department);
                                """;
}
