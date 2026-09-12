using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// A plain class (no [Equatable], no hand-written equality) sitting between an [Equatable] leaf and an
/// [Equatable] root. The leaf delegates to the root's comparer, which knows nothing about the intermediate,
/// so the intermediate's members must be collected and compared explicitly by the leaf.
/// Also covers Inequalities reporting a runtime-type mismatch for a root class.
/// </summary>
public partial class PlainIntermediateTests : SnapshotTestBase
{
    [Equatable]
    public partial class Vehicle
    {
        public Vehicle(string vin) => Vin = vin;

        public string Vin { get; }
    }

    public class Truck : Vehicle
    {
        public Truck(string vin, int payloadKg) : base(vin) => PayloadKg = payloadKg;

        public int PayloadKg { get; }
    }

    [Equatable]
    public partial class DumpTruck : Truck
    {
        public DumpTruck(string vin, int payloadKg, bool tipping) : base(vin, payloadKg) => Tipping = tipping;

        public bool Tipping { get; }
    }

    public static TheoryData<DumpTruck, DumpTruck, bool> Cases => new()
    {
        { new DumpTruck("v", 1, true), new DumpTruck("v", 1, true), true },
        // Root member differs
        { new DumpTruck("v", 1, true), new DumpTruck("w", 1, true), false },
        // Plain intermediate member differs -> must be detected (was silently ignored before)
        { new DumpTruck("v", 1, true), new DumpTruck("v", 2, true), false },
        // Leaf member differs
        { new DumpTruck("v", 1, true), new DumpTruck("v", 1, false), false },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void Equality(DumpTruck a, DumpTruck b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void IntermediateMember_IsReportedByInequalities()
    {
        var inequalities = DumpTruck.EqualityComparer.Default
            .Inequalities(new DumpTruck("v", 1, true), new DumpTruck("v", 2, true))
            .ToList();

        inequalities.Should().Equal([Ineq(1, 2, Prop("PayloadKg"))]);
    }

    public class Van : Vehicle
    {
        public Van(string vin) : base(vin) { }
    }

    [Fact]
    public void RuntimeTypeMismatch_IsReportedByInequalities()
    {
        var vehicle = new Vehicle("v");
        var van = new Van("v");

        EqualityAssert.Verify(vehicle, van, false);

        Vehicle.EqualityComparer.Default.Inequalities(vehicle, van).Should()
            .Equal([Ineq(vehicle, van)], "a runtime-type mismatch is reported as one whole-object inequality");
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class PlainIntermediateVehicle
                                {
                                    public PlainIntermediateVehicle(string vin) => Vin = vin;

                                    public string Vin { get; }
                                }

                                public class PlainIntermediateTruck : PlainIntermediateVehicle
                                {
                                    public PlainIntermediateTruck(string vin, int payloadKg) : base(vin) => PayloadKg = payloadKg;

                                    public int PayloadKg { get; }
                                }

                                [Equatable]
                                public partial class PlainIntermediateDumpTruck : PlainIntermediateTruck
                                {
                                    public PlainIntermediateDumpTruck(string vin, int payloadKg, bool tipping) : base(vin, payloadKg) => Tipping = tipping;

                                    public bool Tipping { get; }
                                }
                                """;
}
