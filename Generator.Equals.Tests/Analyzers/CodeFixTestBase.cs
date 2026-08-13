extern alias GeneratorEquals;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Base class for code fix tests using Microsoft.CodeAnalysis.Testing.
/// </summary>
public abstract class CodeFixTestBase<TAnalyzer, TCodeFix>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    /// <summary>
    /// Verifies that the code fix transforms the source as expected.
    /// </summary>
    protected static async Task VerifyCodeFixAsync(string source, string fixedSource, params DiagnosticResult[] expected)
    {
        var test = new CodeFixTest(source, fixedSource, expected);
        await test.RunAsync();
    }

    /// <summary>
    /// Verifies that the code fix transforms the source as expected, with a specific code fix action index.
    /// </summary>
    protected static async Task VerifyCodeFixAsync(string source, string fixedSource, int codeFixIndex, params DiagnosticResult[] expected)
    {
        var test = new CodeFixTest(source, fixedSource, expected)
        {
            CodeActionIndex = codeFixIndex
        };
        await test.RunAsync();
    }

    /// <summary>
    /// Creates a diagnostic result for the given descriptor.
    /// </summary>
    protected static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
        => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, DefaultVerifier>.Diagnostic(descriptor);

    private sealed class CodeFixTest : CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
    {
        private static readonly MetadataReference RuntimeReference =
            MetadataReference.CreateFromFile(typeof(Generator.Equals.EquatableAttribute).Assembly.Location);

        public CodeFixTest(string source, string fixedSource, params DiagnosticResult[] expected)
        {
            TestCode = source;
            FixedCode = fixedSource;
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
            FixedState.AdditionalReferences.Add(RuntimeReference);
        }
    }
}
