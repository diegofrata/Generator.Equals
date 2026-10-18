using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// A record that enforces the <c>EqualityContract</c> guard itself (a root record, or one with
/// IgnoreInheritedMembers) must have Inequalities report a contract mismatch as one whole-object inequality,
/// mirroring Equals: two records of different runtime types are unequal even when every member matches.
/// </summary>
public partial class EqualityContractMismatchTests : SnapshotTestBase
{
    [Equatable]
    public partial record Vehicle(string Vin);

    public record Van(string Vin) : Vehicle(Vin);

    [Fact]
    public void RootRecord_ContractMismatch_IsReportedByInequalities()
    {
        var vehicle = new Vehicle("v");
        var van = new Van("v");

        EqualityAssert.Verify(vehicle, van, false);

        Vehicle.EqualityComparer.Default.Inequalities(vehicle, van).Should()
            .Equal([Ineq(vehicle, van)], "an EqualityContract mismatch is reported as one whole-object inequality");
    }

    public partial record Base(string Name);

    [Equatable(IgnoreInheritedMembers = true)]
    public partial record Child(string Name, int Age) : Base(Name);

    public record GrandChild(string Name, int Age) : Child(Name, Age);

    [Fact]
    public void IgnoringInheritedMembers_ContractMismatch_IsReportedByInequalities()
    {
        var child = new Child("n", 1);
        var grandChild = new GrandChild("n", 1);

        EqualityAssert.Verify(child, grandChild, false);

        Child.EqualityComparer.Default.Inequalities(child, grandChild).Should()
            .Equal([Ineq(child, grandChild)], "IgnoreInheritedMembers records enforce the contract guard themselves");

        Child.EqualityComparer.Default.Inequalities(child, new Child("n", 1)).Should()
            .BeEmpty("equal instances have no inequalities");
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record EqualityContractMismatchVehicle(string Vin);

                                public record EqualityContractMismatchVan(string Vin) : EqualityContractMismatchVehicle(Vin);

                                public partial record EqualityContractMismatchBase(string Name);

                                [Equatable(IgnoreInheritedMembers = true)]
                                public partial record EqualityContractMismatchChild(string Name, int Age) : EqualityContractMismatchBase(Name);
                                """;
}
