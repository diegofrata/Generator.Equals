extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE011: [GenerateClassEqualityOperators] on non-class types.
/// </summary>
public sealed class GE011GenerateClassEqualityOperatorsOnNonClassTypesTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task GenerateClassEqualityOperatorsOnClass_False_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable (GenerateClassEqualityOperators = false)]
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

            [Equatable (GenerateClassEqualityOperators = true)]
            public partial class Sample
            {
                public double Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnRecord_False_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable (GenerateClassEqualityOperators = false)]
            public partial record Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 52)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnRecord_True_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable (GenerateClassEqualityOperators = true)]
            public partial record Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 51)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnStruct_False_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable (GenerateClassEqualityOperators = false)]
            public partial struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 52)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task GenerateClassEqualityOperatorsOnStruct_True_ReportDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable (GenerateClassEqualityOperators = true)]
            public partial struct Sample
            {
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsIgnored)
                .WithSpan(3, 2, 3, 51)
                .WithArguments("Sample"));
    }
}