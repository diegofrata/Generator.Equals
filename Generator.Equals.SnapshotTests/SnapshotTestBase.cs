extern alias GeneratorEquals;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.SnapshotTests
{
    public abstract class SnapshotTestBase
    {
        // Reference to the Generator.Equals.Runtime assembly for the [Equatable] attribute
        private static readonly MetadataReference RuntimeReference = MetadataReference.CreateFromFile(
            typeof(Generator.Equals.EquatableAttribute).Assembly.Location);

        public enum TargetFramework
        {
            Net60,
            NetFramework48
        }

        protected async Task VerifyGeneratedSource(
            string directory,
            string fileName,
            string source,
            OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
            TargetFramework targetFramework = TargetFramework.Net60,
            CancellationToken ct = default)
        {
            var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp9);
            ReferenceAssemblies? referenceAssemblies;
            switch (targetFramework)
            {
                case TargetFramework.Net60:
                    referenceAssemblies = ReferenceAssemblies.Net.Net60;
                    break;
                case TargetFramework.NetFramework48:
                    referenceAssemblies = ReferenceAssemblies.NetFramework.Net48.Default;
                    referenceAssemblies = referenceAssemblies.AddPackages(ImmutableArray<PackageIdentity>.Empty.Add(new PackageIdentity("Microsoft.Bcl.HashCode", "1.1.1")));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetFramework), targetFramework, null);
            }

            var references = await referenceAssemblies.ResolveAsync(null, ct);

            var compilation = (Compilation)CSharpCompilation.Create(
                assemblyName: "MyAssembly",
                syntaxTrees: new[]
                {
                    CSharpSyntaxTree.ParseText(
                        source,
                        options: parseOptions,
                        cancellationToken: ct),
                },
                references: references.Add(RuntimeReference),
                options: new CSharpCompilationOptions(outputKind, nullableContextOptions: NullableContextOptions.Enable));

            var driver = CSharpGeneratorDriver
                .Create(new[] { new GeneratorEquals::Generator.Equals.EqualsGenerator().AsSourceGenerator() }, parseOptions: parseOptions)
                .RunGeneratorsAndUpdateCompilation(compilation, out _, out var diagnostics, ct);

            var targetFileName = $"{fileName}.{targetFramework}";

            var runResult = driver.GetRunResult();

            // Filter and order generated sources
            var generatedSources = runResult.Results
                .SelectMany(r => r.GeneratedSources)
                .Where(s => !s.HintName.StartsWith("Generator.Equals.Runtime"))
                .OrderBy(s => s.HintName)
                .ToArray();

            // Create targets for each generated source file
            var targets = generatedSources.Select(s =>
            {
                var name = Path.GetFileNameWithoutExtension(s.HintName);
                var content = $"//HintName: {s.HintName}\n{s.SourceText}";
                return new Target("cs", content, name);
            });

            await Task.WhenAll(
                Verify(diagnostics).UseDirectory(directory).UseFileName($"{targetFileName}.Diagnostics"),
                Verify(targets)
                    .UseDirectory(directory)
                    .UseFileName(targetFileName)
            );
        }
    }
}