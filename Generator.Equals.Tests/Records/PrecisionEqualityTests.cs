using Generator.Equals.Tests.Infrastructure;

namespace Generator.Equals.Tests.Records;

/// <summary>
/// Tests for [PrecisionEquality] attribute on records with float, double, decimal, integer, and nullable types.
/// </summary>
public partial class PrecisionEqualityTests : SnapshotTestBase
{
    [Equatable]
    public partial record SampleDouble
    {
        [PrecisionEquality(0.001)]
        public double Value { get; init; }
    }

    [Equatable]
    public partial record SampleFloat
    {
        [PrecisionEquality(0.01)]
        public float Value { get; init; }
    }

    [Equatable]
    public partial record SampleDecimal
    {
        [PrecisionEquality(0.0001)]
        public decimal Value { get; init; }
    }

    [Equatable]
    public partial record SampleNullableDouble
    {
        [PrecisionEquality(0.001)]
        public double? Value { get; init; }
    }

    [Equatable]
    public partial record SampleInt
    {
        [PrecisionEquality(5)]
        public int Value { get; init; }
    }

    [Equatable]
    public partial record SampleNullableInt
    {
        [PrecisionEquality(5)]
        public int? Value { get; init; }
    }

    public static TheoryData<SampleDouble, SampleDouble, bool> DoubleCases => new()
    {
        // Exact match
        { new SampleDouble { Value = 1.0 }, new SampleDouble { Value = 1.0 }, true },
        // Within tolerance
        { new SampleDouble { Value = 1.0 }, new SampleDouble { Value = 1.0005 }, true },
        // Outside tolerance
        { new SampleDouble { Value = 1.0 }, new SampleDouble { Value = 1.002 }, false },
    };

    [Theory]
    [MemberData(nameof(DoubleCases))]
    public void DoubleEquality(SampleDouble a, SampleDouble b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleFloat, SampleFloat, bool> FloatCases => new()
    {
        // Exact match
        { new SampleFloat { Value = 1.0f }, new SampleFloat { Value = 1.0f }, true },
        // Within tolerance
        { new SampleFloat { Value = 1.0f }, new SampleFloat { Value = 1.005f }, true },
        // Outside tolerance
        { new SampleFloat { Value = 1.0f }, new SampleFloat { Value = 1.02f }, false },
    };

    [Theory]
    [MemberData(nameof(FloatCases))]
    public void FloatEquality(SampleFloat a, SampleFloat b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleDecimal, SampleDecimal, bool> DecimalCases => new()
    {
        // Exact match
        { new SampleDecimal { Value = 1.0m }, new SampleDecimal { Value = 1.0m }, true },
        // Within tolerance
        { new SampleDecimal { Value = 1.0m }, new SampleDecimal { Value = 1.00005m }, true },
        // Outside tolerance
        { new SampleDecimal { Value = 1.0m }, new SampleDecimal { Value = 1.0002m }, false },
    };

    [Theory]
    [MemberData(nameof(DecimalCases))]
    public void DecimalEquality(SampleDecimal a, SampleDecimal b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleNullableDouble, SampleNullableDouble, bool> NullableDoubleCases => new()
    {
        // Both null
        { new SampleNullableDouble { Value = null }, new SampleNullableDouble { Value = null }, true },
        // One null
        { new SampleNullableDouble { Value = 1.0 }, new SampleNullableDouble { Value = null }, false },
        { new SampleNullableDouble { Value = null }, new SampleNullableDouble { Value = 1.0 }, false },
        // Within tolerance
        { new SampleNullableDouble { Value = 1.0 }, new SampleNullableDouble { Value = 1.0005 }, true },
        // Outside tolerance
        { new SampleNullableDouble { Value = 1.0 }, new SampleNullableDouble { Value = 1.002 }, false },
    };

    [Theory]
    [MemberData(nameof(NullableDoubleCases))]
    public void NullableDoubleEquality(SampleNullableDouble a, SampleNullableDouble b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleInt, SampleInt, bool> IntCases => new()
    {
        // Exact match
        { new SampleInt { Value = 10 }, new SampleInt { Value = 10 }, true },
        // Within tolerance (diff = 4, tolerance = 5)
        { new SampleInt { Value = 10 }, new SampleInt { Value = 14 }, true },
        // Outside tolerance (diff = 6, tolerance = 5)
        { new SampleInt { Value = 10 }, new SampleInt { Value = 16 }, false },
    };

    [Theory]
    [MemberData(nameof(IntCases))]
    public void IntEquality(SampleInt a, SampleInt b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    public static TheoryData<SampleNullableInt, SampleNullableInt, bool> NullableIntCases => new()
    {
        // Both null
        { new SampleNullableInt { Value = null }, new SampleNullableInt { Value = null }, true },
        // One null
        { new SampleNullableInt { Value = 10 }, new SampleNullableInt { Value = null }, false },
        { new SampleNullableInt { Value = null }, new SampleNullableInt { Value = 10 }, false },
        // Within tolerance
        { new SampleNullableInt { Value = 10 }, new SampleNullableInt { Value = 14 }, true },
        // Outside tolerance
        { new SampleNullableInt { Value = 10 }, new SampleNullableInt { Value = 16 }, false },
    };

    [Theory]
    [MemberData(nameof(NullableIntCases))]
    public void NullableIntEquality(SampleNullableInt a, SampleNullableInt b, bool expected) =>
        EqualityAssert.Verify(a, b, expected);

    [Theory]
    [MemberData(nameof(TargetFrameworks))]
    public Task VerifyGeneratedCode(TargetFramework fw) =>
        VerifyGeneratedSource(SampleSource, fw, ct: TestContext.Current.CancellationToken);

    const string SampleSource = """
                                using Generator.Equals;

                                namespace Generator.Equals.Tests.Records;

                                [Equatable]
                                public partial record PrecisionEqualityDoubleSample
                                {
                                    [PrecisionEquality(0.001)]
                                    public double Value { get; init; }
                                }

                                [Equatable]
                                public partial record PrecisionEqualityFloatSample
                                {
                                    [PrecisionEquality(0.01)]
                                    public float Value { get; init; }
                                }

                                [Equatable]
                                public partial record PrecisionEqualityDecimalSample
                                {
                                    [PrecisionEquality(0.0001)]
                                    public decimal Value { get; init; }
                                }

                                [Equatable]
                                public partial record PrecisionEqualityNullableDoubleSample
                                {
                                    [PrecisionEquality(0.001)]
                                    public double? Value { get; init; }
                                }

                                [Equatable]
                                public partial record PrecisionEqualityIntSample
                                {
                                    [PrecisionEquality(5)]
                                    public int Value { get; init; }
                                }

                                [Equatable]
                                public partial record PrecisionEqualityNullableIntSample
                                {
                                    [PrecisionEquality(5)]
                                    public int? Value { get; init; }
                                }
                                """;
}
