extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE010: [PrecisionEquality] on unsupported type.
/// </summary>
public sealed class GE010PrecisionEqualityOnUnsupportedTypeTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task PrecisionEqualityOnString_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.001)]
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.PrecisionEqualityOnUnsupportedType)
                .WithSpan(6, 6, 6, 30)
                .WithArguments("Name", "string"));
    }

    [Fact]
    public async Task PrecisionEqualityOnBool_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.001)]
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.PrecisionEqualityOnUnsupportedType)
                .WithSpan(6, 6, 6, 30)
                .WithArguments("IsActive", "bool"));
    }

    [Fact]
    public async Task PrecisionEqualityOnListDouble_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.001)]
                public List<double> Values { get; set; }
            }
            """;

        // Also reports GE001 because collection is missing collection equality attribute
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.PrecisionEqualityOnUnsupportedType)
                .WithSpan(7, 6, 7, 30)
                .WithArguments("Values", "List<double>"),
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(8, 12, 8, 24)
                .WithArguments("Values"));
    }

    [Fact]
    public async Task PrecisionEqualityOnByte_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(1)]
                public byte Value { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.PrecisionEqualityOnUnsupportedType)
                .WithSpan(6, 6, 6, 26)
                .WithArguments("Value", "byte"));
    }

    [Fact]
    public async Task PrecisionEqualityOnUint_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(1)]
                public uint Value { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.PrecisionEqualityOnUnsupportedType)
                .WithSpan(6, 6, 6, 26)
                .WithArguments("Value", "uint"));
    }

    [Fact]
    public async Task PrecisionEqualityOnDouble_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.001)]
                public double Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnFloat_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.01)]
                public float Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnDecimal_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.0001)]
                public decimal Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnInt_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(5)]
                public int Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnLong_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(10)]
                public long Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnShort_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(3)]
                public short Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnSbyte_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(2)]
                public sbyte Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnNullableDouble_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.001)]
                public double? Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnNullableFloat_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.01)]
                public float? Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnNullableDecimal_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(0.0001)]
                public decimal? Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrecisionEqualityOnNullableInt_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [PrecisionEquality(5)]
                public int? Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NoEquatableAttribute_ReportsGE004InsteadOfGE010()
    {
        const string source = """
            using Generator.Equals;

            public class Sample
            {
                [PrecisionEquality(0.001)]
                public int Value { get; set; }
            }
            """;

        // Only GE004 is reported when the type doesn't have [Equatable]
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 30)
                .WithArguments("Value", "PrecisionEquality", "Sample"));
    }
}
