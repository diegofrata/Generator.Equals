using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.SnapshotTests
{
    [UsesVerify]
    public class GeneratedSourceTests
    {
        static IEnumerable<object[]> FindSampleFiles([CallerFilePath] string? path = null)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            var testProjectPath = Path.GetFullPath(Path.Combine(path, "../../Generator.Equals.Tests"));
            var files = Directory.EnumerateFiles(testProjectPath, "*.Sample.cs", SearchOption.AllDirectories);

            return files.Select(f =>
            {
                var relativePath = Path.GetRelativePath(testProjectPath, f);
                return new object[]
                {
                    Path.GetDirectoryName(relativePath)!,
                    Path.GetFileName(relativePath).Replace(".Sample.cs", ""),
                    f
                };
            });
        }

        public static IEnumerable<object[]> SampleFiles => FindSampleFiles();

        public static async Task VerifyGeneratedSource(string directory, string fileName, string source, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
            CancellationToken ct = default)
        {
            var referenceAssemblies = ReferenceAssemblies.Net.Net60;
            var references = await referenceAssemblies.ResolveAsync(null, ct);
            var compilation = (Compilation)CSharpCompilation.Create(
                assemblyName: "MyAssembly",
                syntaxTrees: new[]
                {
                    CSharpSyntaxTree.ParseText(source, options: new CSharpParseOptions(), cancellationToken: ct),
                },
                references: references
                    .Add(MetadataReference.CreateFromFile(typeof(CustomEqualityAttribute).Assembly.Location)),
                options: new CSharpCompilationOptions(outputKind, nullableContextOptions: NullableContextOptions.Enable));

            var driver = CSharpGeneratorDriver
                .Create(new EqualsGenerator())
                .RunGeneratorsAndUpdateCompilation(compilation, out compilation, out _, ct);

            var diagnostics = compilation.GetDiagnostics(ct);

            await Task.WhenAll(
                Verify(diagnostics).UseDirectory(directory).UseFileName($"{fileName}.Diagnostics"),
                Verify(driver).UseDirectory(directory).UseFileName(fileName)
            );
        }

        [Theory]
        [MemberData(nameof(SampleFiles))]
        public async Task Check(string directory, string fileName, string filePath)
        {
            var sample = await File.ReadAllTextAsync(filePath);
            await VerifyGeneratedSource(directory, fileName, sample);
        }
    }
}