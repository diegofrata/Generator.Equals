extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE005: Type with [Equatable] has manual Equals/GetHashCode implementation.
/// </summary>
public sealed class GE005ManualEqualsImplementationTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task ManualEqualsOverride_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }

                public override bool Equals(object obj)
                {
                    return obj is Sample other && Name == other.Name;
                }
            }
            """;

        // "Equals" method name = 6 chars, starts at col 26, ends at col 32
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ManualEqualsImplementation)
                .WithSpan(8, 26, 8, 32)
                .WithArguments("Sample", "Equals(object)"));
    }

    [Fact]
    public async Task ManualGetHashCodeOverride_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }

                public override int GetHashCode()
                {
                    return Name?.GetHashCode() ?? 0;
                }
            }
            """;

        // "GetHashCode" method name = 11 chars, starts at col 25, ends at col 36
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ManualEqualsImplementation)
                .WithSpan(8, 25, 8, 36)
                .WithArguments("Sample", "GetHashCode()"));
    }

    [Fact]
    public async Task BothEqualsAndGetHashCode_ReportsMultipleDiagnostics()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }

                public override bool Equals(object obj)
                {
                    return obj is Sample other && Name == other.Name;
                }

                public override int GetHashCode()
                {
                    return Name?.GetHashCode() ?? 0;
                }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ManualEqualsImplementation)
                .WithSpan(8, 26, 8, 32)
                .WithArguments("Sample", "Equals(object)"),
            Diagnostic(DiagnosticDescriptors.ManualEqualsImplementation)
                .WithSpan(13, 25, 13, 36)
                .WithArguments("Sample", "GetHashCode()"));
    }

    [Fact]
    public async Task NoManualOverrides_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task TypedEqualsMethod_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }

                public bool Equals(Sample other)
                {
                    return Name == other?.Name;
                }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NonOverrideEqualsMethod_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }

                public bool Equals(string name)
                {
                    return Name == name;
                }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StaticEqualsMethod_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }

                public static bool Equals(Sample a, Sample b)
                {
                    return a?.Name == b?.Name;
                }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StructWithManualEqualsOverride_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial struct Sample
            {
                public string Name { get; set; }

                public override bool Equals(object obj)
                {
                    return obj is Sample other && Name == other.Name;
                }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ManualEqualsImplementation)
                .WithSpan(8, 26, 8, 32)
                .WithArguments("Sample", "Equals(object)"));
    }

    [Fact]
    public async Task WithoutEquatable_NoManualOverrideDiagnostic()
    {
        const string source = """
            public class Sample
            {
                public string Name { get; set; }

                public override bool Equals(object obj)
                {
                    return obj is Sample other && Name == other.Name;
                }

                public override int GetHashCode()
                {
                    return Name?.GetHashCode() ?? 0;
                }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }
}
