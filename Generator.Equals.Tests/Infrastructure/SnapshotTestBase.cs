extern alias GeneratorEquals;

using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.Tests.Infrastructure;

public abstract class SnapshotTestBase
{
    static readonly MetadataReference RuntimeReference = MetadataReference.CreateFromFile(
        typeof(Generator.Equals.EquatableAttribute).Assembly.Location);

    public enum TargetFramework
    {
        Net60,
        NetFramework48
    }

    /// <summary>
    /// Provides TargetFramework values for use with [Theory, MemberData].
    /// </summary>
    public static TheoryData<TargetFramework> TargetFrameworks =>
    [
        TargetFramework.Net60,
        TargetFramework.NetFramework48
    ];

    /// <summary>
    /// Gets the source code from the nested sample type in the test class.
    /// Override this method to provide custom source code.
    /// </summary>
    protected virtual string GetSampleSource([CallerFilePath] string? callerFilePath = null)
    {
        // Default implementation reads the sample type from the same class
        // Derived classes should override this to provide their source code
        throw new NotImplementedException(
            "Override GetSampleSource() in your test class or use VerifyGeneratedSource with explicit source.");
    }

    /// <summary>
    /// Verifies generated source code against snapshots.
    /// </summary>
    protected async Task VerifyGeneratedSource(
        string source,
        TargetFramework targetFramework,
        OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary,
        [CallerFilePath] string? callerFilePath = null,
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
                referenceAssemblies = referenceAssemblies.AddPackages(
                    ImmutableArray<PackageIdentity>.Empty.Add(new PackageIdentity("Microsoft.Bcl.HashCode", "1.1.1")));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(targetFramework), targetFramework, null);
        }

        var references = await referenceAssemblies.ResolveAsync(null, ct);

        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees:
            [
                CSharpSyntaxTree.ParseText(
                    source,
                    options: parseOptions,
                    cancellationToken: ct)
            ],
            references: references.Add(RuntimeReference),
            options: new CSharpCompilationOptions(outputKind, nullableContextOptions: NullableContextOptions.Enable));

        var driver = CSharpGeneratorDriver
            .Create([new GeneratorEquals::Generator.Equals.EqualsGenerator().AsSourceGenerator()], parseOptions: parseOptions)
            .RunGeneratorsAndUpdateCompilation(compilation, out _, out var diagnostics, ct);

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

        // Determine snapshot directory based on caller file path
        var callerDir = Path.GetDirectoryName(callerFilePath)!;
        var callerFileName = Path.GetFileNameWithoutExtension(callerFilePath);
        var snapshotDir = Path.Combine(callerDir, "..", "Snapshots", Path.GetFileName(callerDir));
        var targetFileName = $"{callerFileName}.{targetFramework}";

        await Task.WhenAll(
            Verify(diagnostics)
                .UseDirectory(snapshotDir)
                .UseFileName($"{targetFileName}.Diagnostics")
                .DisableRequireUniquePrefix(),
            Verify(targets)
                .UseDirectory(snapshotDir)
                .UseFileName(targetFileName)
                .DisableRequireUniquePrefix()
        );
    }
}
