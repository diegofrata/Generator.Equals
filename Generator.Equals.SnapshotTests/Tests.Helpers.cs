using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using Generator.Equals;

namespace H.Generators.SnapshotTests;

[TestClass]
public partial class Tests : VerifyBase
{
    private static string GetHeader()
    {
        return @$"
#nullable enable

namespace Generator.Equals.SnapshotTests;
";
    }

    public async Task CheckSourceAsync(
        string source,
        CancellationToken cancellationToken = default)
    {
        source = $"{GetHeader()}{source}";

        var referenceAssemblies = ReferenceAssemblies.Net.Net60;
        var references = await referenceAssemblies.ResolveAsync(null, cancellationToken);
        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: new[]
            {
                CSharpSyntaxTree.ParseText(source, options: new CSharpParseOptions(), cancellationToken: cancellationToken),
            },
            references: references
                .Add(MetadataReference.CreateFromFile(typeof(CustomEqualityAttribute).Assembly.Location)),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var generator = new EqualsGenerator();
        var driver = CSharpGeneratorDriver
            .Create(generator)
            .RunGeneratorsAndUpdateCompilation(compilation, out compilation, out _, cancellationToken);
        var diagnostics = compilation.GetDiagnostics(cancellationToken);

        await Task.WhenAll(
            Verify(diagnostics)
                .UseDirectory("Snapshots")
                .UseTextForParameters("Diagnostics"),
            Verify(driver)
                .UseDirectory("Snapshots"));
    }
}