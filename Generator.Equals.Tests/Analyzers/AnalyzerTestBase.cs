extern alias GeneratorEquals;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Base class for analyzer tests using Microsoft.CodeAnalysis.Testing.
/// </summary>
public abstract class AnalyzerTestBase<TAnalyzer> where TAnalyzer : DiagnosticAnalyzer, new()
{
    /// <summary>
    /// Verifies that the given source code produces the expected diagnostics.
    /// </summary>
    protected static async Task VerifyDiagnosticAsync(string source, params DiagnosticResult[] expected)
    {
        var test = new AnalyzerTest(source, expected);
        await test.RunAsync();
    }

    /// <summary>
    /// Verifies that the given source code produces no diagnostics.
    /// </summary>
    protected static async Task VerifyNoDiagnosticAsync(string source)
    {
        var test = new AnalyzerTest(source);
        await test.RunAsync();
    }

    /// <summary>
    /// Creates a diagnostic result for the given descriptor and location.
    /// </summary>
    protected static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => CSharpAnalyzerVerifier<TAnalyzer, DefaultVerifier>.Diagnostic(descriptor);

    private sealed class AnalyzerTest : CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
    {
        private static readonly MetadataReference RuntimeReference =
            MetadataReference.CreateFromFile(typeof(Generator.Equals.EquatableAttribute).Assembly.Location);

        public AnalyzerTest(string source, params DiagnosticResult[] expected)
        {
            TestCode = source;
            ExpectedDiagnostics.AddRange(expected);

            // Configure for C# 9+ features
            SolutionTransforms.Add((solution, projectId) =>
            {
                var project = solution.GetProject(projectId);
                if (project == null) return solution;

                var compilationOptions = project.CompilationOptions;
                if (compilationOptions != null)
                {
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(
                        compilationOptions.SpecificDiagnosticOptions.SetItems(
                            new Dictionary<string, ReportDiagnostic>
                            {
                                // Suppress CS8019 (unnecessary using directive)
                                { "CS8019", ReportDiagnostic.Suppress }
                            }));
                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);
                }

                var parseOptions = project.ParseOptions as CSharpParseOptions;
                if (parseOptions != null)
                {
                    solution = solution.WithProjectParseOptions(projectId,
                        parseOptions.WithLanguageVersion(LanguageVersion.CSharp10));
                }

                return solution;
            });

            // Add Generator.Equals.Runtime reference
            TestState.AdditionalReferences.Add(RuntimeReference);
        }
    }
}
