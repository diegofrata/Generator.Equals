extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE011: [Equatable(GenerateClassEqualityOperators)] on non-class types.
/// </summary>
public sealed class GE011GenerateClassEqualityOperatorsOnNonClassTypesTests : AnalyzerTestBase<EquatableAnalyzer>
{
    const string RecordReason = DiagnosticDescriptors.RecordOperatorsAlwaysEmittedReason;
    const string StructReason = DiagnosticDescriptors.StructOperatorsAlwaysGeneratedReason;

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnClass_False_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Sample
            {
                public double Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnClass_True_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = true)]
            public partial class Sample
            {
                public double Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task RecordWithoutTheOption_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial record Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StructWithoutTheOption_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task RecordStructWithoutTheOption_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial record struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnRecord_False_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial record Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 51)
                .WithArguments("Sample", RecordReason));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnRecord_True_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = true)]
            public partial record Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 50)
                .WithArguments("Sample", RecordReason));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnRecordStruct_False_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial record struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 51)
                .WithArguments("Sample", RecordReason));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnStruct_False_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 51)
                .WithArguments("Sample", StructReason));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnStruct_True_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = true)]
            public partial struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 50)
                .WithArguments("Sample", StructReason));
    }
}
