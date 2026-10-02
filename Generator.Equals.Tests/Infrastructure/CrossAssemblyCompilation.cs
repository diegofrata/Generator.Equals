extern alias GeneratorEquals;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Tests.Infrastructure;

/// <summary>
/// Helpers for tests that compile an "external" assembly and reference it from a "main" compilation, so
/// the generator/analyzer sees the external types only through metadata.
/// </summary>
public static class CrossAssemblyCompilation
{
    public static readonly MetadataReference[] CoreReferences = GetCoreReferences();

    static MetadataReference[] GetCoreReferences()
    {
        var refs = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location),
        };

        var runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        foreach (var name in new[] { "System.Runtime.dll", "netstandard.dll" })
        {
            var path = Path.Combine(runtimeDir, name);
            if (File.Exists(path))
                refs.Add(MetadataReference.CreateFromFile(path));
        }

        return refs.ToArray();
    }

    public static Compilation Compile(string assemblyName, string source, params MetadataReference[] extra) =>
        CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken)],
            CoreReferences.Concat(extra).ToArray(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    static CSharpGeneratorDriver CreateDriver() =>
        CSharpGeneratorDriver.Create(new GeneratorEquals::Generator.Equals.EqualsGenerator().AsSourceGenerator());

    /// <summary>Runs the generator on <paramref name="compilation"/> and returns it with the generated trees added.</summary>
    public static Compilation WithGenerated(Compilation compilation)
    {
        CreateDriver().RunGeneratorsAndUpdateCompilation(compilation, out var updated, out _, TestContext.Current.CancellationToken);
        return updated;
    }

    /// <summary>Runs the generator on <paramref name="compilation"/> and returns the generated source whose hint name contains <paramref name="hintNameContains"/>.</summary>
    public static string GeneratedSourceFor(Compilation compilation, string hintNameContains) =>
        CreateDriver()
            .RunGenerators(compilation, TestContext.Current.CancellationToken)
            .GetRunResult()
            .Results
            .SelectMany(r => r.GeneratedSources)
            .Single(s => s.HintName.Contains(hintNameContains))
            .SourceText.ToString();
}
