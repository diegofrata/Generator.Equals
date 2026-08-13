using FluentAssertions;
using Generator.Equals.Tests.Infrastructure;
using static Generator.Equals.Tests.Infrastructure.InequalityHelpers;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Tests for [PrecisionEquality] attribute with float, double, decimal, integer, and nullable types.
/// </summary>
public partial class PrecisionEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial class SampleDouble
    {
        public SampleDouble(double value)
        {
            Value = value;
        }

        [PrecisionEquality(0.001)]
        public double Value { get; }
    }

    [Equatable]
    public partial class SampleFloat
    {
        public SampleFloat(float value)
        {
            Value = value;
        }

        [PrecisionEquality(0.01)]
        public float Value { get; }
    }

    [Equatable]
    public partial class SampleDecimal
    {
        public SampleDecimal(decimal value)
        {
            Value = value;
        }

        [PrecisionEquality(0.0001)]
        public decimal Value { get; }
    }

    [Equatable]
    public partial class SampleNullableDouble
    {
        public SampleNullableDouble(double? value)
        {
            Value = value;
        }

        [PrecisionEquality(0.001)]
        public double? Value { get; }
    }

    [Equatable]
    public partial class SampleInt
    {
        public SampleInt(int value)
        {
            Value = value;
        }

        [PrecisionEquality(5)]
        public int Value { get; }
    }

    [Equatable]
    public partial class SampleNullableInt
    {
        public SampleNullableInt(int? value)
        {
            Value = value;
        }

        [PrecisionEquality(5)]
        public int? Value { get; }
    }

    public static TheoryData<SampleDouble, SampleDouble, bool> DoubleCases => new()
    {
        // Exact match
        { new SampleDouble(1.0), new SampleDouble(1.0), true },
        // Within tolerance
        { new SampleDouble(1.0), new SampleDouble(1.0005), true },
        // Outside tolerance
        { new SampleDouble(1.0), new SampleDouble(1.002), false },
        // Negative values within tolerance
        { new SampleDouble(-5.0), new SampleDouble(-5.0005), true },
        // Negative values outside tolerance
        { new SampleDouble(-5.0), new SampleDouble(-5.002), false },
    };

    [Theory]
    [MemberData(nameof(DoubleCases))]
    public void DoubleEquality(SampleDouble a, SampleDouble b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleFloat, SampleFloat, bool> FloatCases => new()
    {
        // Exact match
        { new SampleFloat(1.0f), new SampleFloat(1.0f), true },
        // Within tolerance
        { new SampleFloat(1.0f), new SampleFloat(1.005f), true },
        // Outside tolerance
        { new SampleFloat(1.0f), new SampleFloat(1.02f), false },
        // Negative values within tolerance
        { new SampleFloat(-3.0f), new SampleFloat(-3.005f), true },
    };

    [Theory]
    [MemberData(nameof(FloatCases))]
    public void FloatEquality(SampleFloat a, SampleFloat b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleDecimal, SampleDecimal, bool> DecimalCases => new()
    {
        // Exact match
        { new SampleDecimal(1.0m), new SampleDecimal(1.0m), true },
        // Within tolerance
        { new SampleDecimal(1.0m), new SampleDecimal(1.00005m), true },
        // Outside tolerance
        { new SampleDecimal(1.0m), new SampleDecimal(1.0002m), false },
        // Negative values within tolerance
        { new SampleDecimal(-2.0m), new SampleDecimal(-2.00005m), true },
    };

    [Theory]
    [MemberData(nameof(DecimalCases))]
    public void DecimalEquality(SampleDecimal a, SampleDecimal b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleNullableDouble, SampleNullableDouble, bool> NullableDoubleCases => new()
    {
        // Both null
        { new SampleNullableDouble(null), new SampleNullableDouble(null), true },
        // One null
        { new SampleNullableDouble(1.0), new SampleNullableDouble(null), false },
        { new SampleNullableDouble(null), new SampleNullableDouble(1.0), false },
        // Within tolerance
        { new SampleNullableDouble(1.0), new SampleNullableDouble(1.0005), true },
        // Outside tolerance
        { new SampleNullableDouble(1.0), new SampleNullableDouble(1.002), false },
    };

    [Theory]
    [MemberData(nameof(NullableDoubleCases))]
    public void NullableDoubleEquality(SampleNullableDouble a, SampleNullableDouble b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleInt, SampleInt, bool> IntCases => new()
    {
        // Exact match
        { new SampleInt(10), new SampleInt(10), true },
        // Within tolerance (diff = 4, tolerance = 5)
        { new SampleInt(10), new SampleInt(14), true },
        // Outside tolerance (diff = 6, tolerance = 5)
        { new SampleInt(10), new SampleInt(16), false },
        // Negative values within tolerance
        { new SampleInt(-10), new SampleInt(-14), true },
    };

    [Theory]
    [MemberData(nameof(IntCases))]
    public void IntEquality(SampleInt a, SampleInt b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleNullableInt, SampleNullableInt, bool> NullableIntCases => new()
    {
        // Both null
        { new SampleNullableInt(null), new SampleNullableInt(null), true },
        // One null
        { new SampleNullableInt(10), new SampleNullableInt(null), false },
        { new SampleNullableInt(null), new SampleNullableInt(10), false },
        // Within tolerance
        { new SampleNullableInt(10), new SampleNullableInt(14), true },
        // Outside tolerance
        { new SampleNullableInt(10), new SampleNullableInt(16), false },
    };

    [Theory]
    [MemberData(nameof(NullableIntCases))]
    public void NullableIntEquality(SampleNullableInt a, SampleNullableInt b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Fact]
    public void Double_OutsideTolerance_ReportsInequality()
    {
        var a = new SampleDouble(1.0);
        var b = new SampleDouble(1.002);

        var diffs = SampleDouble.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(1.0, 1.002, Prop("Value")) });
    }

    [Fact]
    public void Double_NegativeOutsideTolerance_ReportsInequality()
    {
        var a = new SampleDouble(-5.0);
        var b = new SampleDouble(-5.002);

        var diffs = SampleDouble.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(-5.0, -5.002, Prop("Value")) });
    }

    [Fact]
    public void Float_OutsideTolerance_ReportsInequality()
    {
        var a = new SampleFloat(1.0f);
        var b = new SampleFloat(1.02f);

        var diffs = SampleFloat.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(1.0f, 1.02f, Prop("Value")) });
    }

    [Fact]
    public void Decimal_OutsideTolerance_ReportsInequality()
    {
        var a = new SampleDecimal(1.0m);
        var b = new SampleDecimal(1.0002m);

        var diffs = SampleDecimal.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(1.0m, 1.0002m, Prop("Value")) });
    }

    [Fact]
    public void NullableDouble_OneNull_ReportsInequality()
    {
        var a = new SampleNullableDouble(1.0);
        var b = new SampleNullableDouble(null);

        var diffs = SampleNullableDouble.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq((double?)1.0, (double?)null, Prop("Value")) });
    }

    [Fact]
    public void NullableDouble_NullVsValue_ReportsInequality()
    {
        var a = new SampleNullableDouble(null);
        var b = new SampleNullableDouble(1.0);

        var diffs = SampleNullableDouble.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq((double?)null, (double?)1.0, Prop("Value")) });
    }

    [Fact]
    public void NullableDouble_OutsideTolerance_ReportsInequality()
    {
        var a = new SampleNullableDouble(1.0);
        var b = new SampleNullableDouble(1.002);

        var diffs = SampleNullableDouble.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq((double?)1.0, (double?)1.002, Prop("Value")) });
    }

    [Fact]
    public void Int_OutsideTolerance_ReportsInequality()
    {
        var a = new SampleInt(10);
        var b = new SampleInt(16);

        var diffs = SampleInt.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq(10, 16, Prop("Value")) });
    }

    [Fact]
    public void NullableInt_OneNull_ReportsInequality()
    {
        var a = new SampleNullableInt(10);
        var b = new SampleNullableInt(null);

        var diffs = SampleNullableInt.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq((int?)10, (int?)null, Prop("Value")) });
    }

    [Fact]
    public void NullableInt_NullVsValue_ReportsInequality()
    {
        var a = new SampleNullableInt(null);
        var b = new SampleNullableInt(10);

        var diffs = SampleNullableInt.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq((int?)null, (int?)10, Prop("Value")) });
    }

    [Fact]
    public void NullableInt_OutsideTolerance_ReportsInequality()
    {
        var a = new SampleNullableInt(10);
        var b = new SampleNullableInt(16);

        var diffs = SampleNullableInt.EqualityComparer.Default.Inequalities(a, b).ToList();

        diffs.Should().BeEquivalentTo(new[] { Ineq((int?)10, (int?)16, Prop("Value")) });
    }

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Classes;

                                [Equatable]
                                public partial class PrecisionEqualityDoubleSample
                                {
                                    public PrecisionEqualityDoubleSample(double value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.001)]
                                    public double Value { get; }
                                }

                                [Equatable]
                                public partial class PrecisionEqualityFloatSample
                                {
                                    public PrecisionEqualityFloatSample(float value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.01)]
                                    public float Value { get; }
                                }

                                [Equatable]
                                public partial class PrecisionEqualityDecimalSample
                                {
                                    public PrecisionEqualityDecimalSample(decimal value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.0001)]
                                    public decimal Value { get; }
                                }

                                [Equatable]
                                public partial class PrecisionEqualityNullableDoubleSample
                                {
                                    public PrecisionEqualityNullableDoubleSample(double? value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.001)]
                                    public double? Value { get; }
                                }

                                [Equatable]
                                public partial class PrecisionEqualityIntSample
                                {
                                    public PrecisionEqualityIntSample(int value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(5)]
                                    public int Value { get; }
                                }

                                [Equatable]
                                public partial class PrecisionEqualityNullableIntSample
                                {
                                    public PrecisionEqualityNullableIntSample(int? value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(5)]
                                    public int? Value { get; }
                                }
                                """;
}
