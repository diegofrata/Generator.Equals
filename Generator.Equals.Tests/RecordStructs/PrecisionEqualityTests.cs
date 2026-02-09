using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.RecordStructs;

/// <summary>
/// Tests for [PrecisionEquality] attribute on record structs with float, double, decimal, integer, and nullable types.
/// </summary>
public partial class PrecisionEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record struct SampleDouble
    {
        public SampleDouble(double value)
        {
            Value = value;
        }

        [PrecisionEquality(0.001)]
        public double Value { get; init; }
    }

    [Equatable]
    public partial record struct SampleFloat
    {
        public SampleFloat(float value)
        {
            Value = value;
        }

        [PrecisionEquality(0.01)]
        public float Value { get; init; }
    }

    [Equatable]
    public partial record struct SampleDecimal
    {
        public SampleDecimal(decimal value)
        {
            Value = value;
        }

        [PrecisionEquality(0.0001)]
        public decimal Value { get; init; }
    }

    [Equatable]
    public partial record struct SampleNullableDouble
    {
        public SampleNullableDouble(double? value)
        {
            Value = value;
        }

        [PrecisionEquality(0.001)]
        public double? Value { get; init; }
    }

    [Equatable]
    public partial record struct SampleInt
    {
        public SampleInt(int value)
        {
            Value = value;
        }

        [PrecisionEquality(5)]
        public int Value { get; init; }
    }

    [Equatable]
    public partial record struct SampleNullableInt
    {
        public SampleNullableInt(int? value)
        {
            Value = value;
        }

        [PrecisionEquality(5)]
        public int? Value { get; init; }
    }

    public static TheoryData<SampleDouble, SampleDouble, bool> DoubleCases => new()
    {
        // Exact match
        { new SampleDouble(1.0), new SampleDouble(1.0), true },
        // Within tolerance
        { new SampleDouble(1.0), new SampleDouble(1.0005), true },
        // Outside tolerance
        { new SampleDouble(1.0), new SampleDouble(1.002), false },
    };

    [Theory]
    [MemberData(nameof(DoubleCases))]
    public void DoubleEquality(SampleDouble a, SampleDouble b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    public static TheoryData<SampleFloat, SampleFloat, bool> FloatCases => new()
    {
        // Exact match
        { new SampleFloat(1.0f), new SampleFloat(1.0f), true },
        // Within tolerance
        { new SampleFloat(1.0f), new SampleFloat(1.005f), true },
        // Outside tolerance
        { new SampleFloat(1.0f), new SampleFloat(1.02f), false },
    };

    [Theory]
    [MemberData(nameof(FloatCases))]
    public void FloatEquality(SampleFloat a, SampleFloat b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

    public static TheoryData<SampleDecimal, SampleDecimal, bool> DecimalCases => new()
    {
        // Exact match
        { new SampleDecimal(1.0m), new SampleDecimal(1.0m), true },
        // Within tolerance
        { new SampleDecimal(1.0m), new SampleDecimal(1.00005m), true },
        // Outside tolerance
        { new SampleDecimal(1.0m), new SampleDecimal(1.0002m), false },
    };

    [Theory]
    [MemberData(nameof(DecimalCases))]
    public void DecimalEquality(SampleDecimal a, SampleDecimal b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

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
        EqualityAssert.VerifyStruct(a, b, expected);

    public static TheoryData<SampleInt, SampleInt, bool> IntCases => new()
    {
        // Exact match
        { new SampleInt(10), new SampleInt(10), true },
        // Within tolerance (diff = 4, tolerance = 5)
        { new SampleInt(10), new SampleInt(14), true },
        // Outside tolerance (diff = 6, tolerance = 5)
        { new SampleInt(10), new SampleInt(16), false },
    };

    [Theory]
    [MemberData(nameof(IntCases))]
    public void IntEquality(SampleInt a, SampleInt b, bool expected) =>
        EqualityAssert.VerifyStruct(a, b, expected);

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
        EqualityAssert.VerifyStruct(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.RecordStructs;

                                [Equatable]
                                public partial record struct PrecisionEqualityDoubleSample
                                {
                                    public PrecisionEqualityDoubleSample(double value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.001)]
                                    public double Value { get; init; }
                                }

                                [Equatable]
                                public partial record struct PrecisionEqualityFloatSample
                                {
                                    public PrecisionEqualityFloatSample(float value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.01)]
                                    public float Value { get; init; }
                                }

                                [Equatable]
                                public partial record struct PrecisionEqualityDecimalSample
                                {
                                    public PrecisionEqualityDecimalSample(decimal value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.0001)]
                                    public decimal Value { get; init; }
                                }

                                [Equatable]
                                public partial record struct PrecisionEqualityNullableDoubleSample
                                {
                                    public PrecisionEqualityNullableDoubleSample(double? value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(0.001)]
                                    public double? Value { get; init; }
                                }

                                [Equatable]
                                public partial record struct PrecisionEqualityIntSample
                                {
                                    public PrecisionEqualityIntSample(int value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(5)]
                                    public int Value { get; init; }
                                }

                                [Equatable]
                                public partial record struct PrecisionEqualityNullableIntSample
                                {
                                    public PrecisionEqualityNullableIntSample(int? value)
                                    {
                                        Value = value;
                                    }

                                    [PrecisionEquality(5)]
                                    public int? Value { get; init; }
                                }
                                """;
}
