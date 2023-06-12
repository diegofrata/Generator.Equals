using System.Collections.Immutable;
using DiffEngine;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.SnapshotTests
{
    public abstract class SnapshotTestBase
    {
        static readonly Lazy<bool> Init = new(DoInit, LazyThreadSafetyMode.ExecutionAndPublication);

        static bool DoInit()
        {
            DiffTools.UseOrder(DiffTool.VisualStudioCode);
            VerifySourceGenerators.Enable();
            VerifySourceGeneratorsPatch.Patch();
            ClipboardAccept.Enable();
            return true;
        }

        protected SnapshotTestBase()
        {
            if (!Init.Value)
                throw new InvalidOperationException();
        }

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
                references: references,
                options: new CSharpCompilationOptions(outputKind, nullableContextOptions: NullableContextOptions.Enable));

            var driver = CSharpGeneratorDriver
                .Create(new[] { new EqualsGenerator().AsSourceGenerator() }, parseOptions: parseOptions)
                .RunGeneratorsAndUpdateCompilation(compilation, out _, out var diagnostics, ct);

            var targetFileName = $"{fileName}.{targetFramework}";

            await Task.WhenAll(
                Verify(diagnostics).UseDirectory(directory).UseFileName($"{targetFileName}.Diagnostics"),
                Verify(driver).UseDirectory(directory).UseFileName(targetFileName)
            );
        }
    }
}