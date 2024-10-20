using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Generator.Equals.Tests.DynamicGeneration.Utils;

public class GeneratorTestHelper
{
    public static readonly List<PortableExecutableReference> References =
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location))
            .Select(x => MetadataReference.CreateFromFile(x.Location))
            .Append(MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location))
            .ToList();

    public static GeneratorDriverRunResult RunGenerator
    (
        [StringSyntax("c#-test")] string source,
        Dictionary<string, string>? options
    )
    {
        // If source has no namespace, add one
        if (!source.Contains("namespace"))
        {
            source = "namespace TestNamespace;\n" + source;
        }

        // using CompositeDto.Generator.Runtime;
        // using System;

        if (!source.Contains("using Generator.Equals;"))
        {
            source = "using Generator.Equals;\n" + source;
        }

        if (!source.Contains("using System;"))
        {
            source = "using System;\n" + source;
        }
        
        if (!source.Contains("using System.Collections.Generic;"))
        {
            source = "using System.Collections.Generic;\n" + source;
        }
        
        
        
        
        var syntaxTree = CSharpSyntaxTree.ParseText(source);

        var compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: References,
            options: new(
                outputKind: OutputKind.DynamicallyLinkedLibrary
            )
        );

        IEnumerable<ISourceGenerator> generator = new[]
        {
            new EqualsGenerator().AsSourceGenerator()
        };

        AnalyzerConfigOptionsProvider? prov = 
            options is null 
                ? null 
                : new MockAnalyzerConfigOptionsProvider(options);

        var driver = CSharpGeneratorDriver
            .Create
            (
                generator
                , optionsProvider: prov
            )
            .RunGeneratorsAndUpdateCompilation(
                compilation,
                out var outputCompilation,
                out var diagnostics
            );

        var diag = outputCompilation.GetDiagnostics();
        var warnings = diag.Where(d => d.Severity == DiagnosticSeverity.Warning).ToList();

        // Assert.Empty(
        //     outputCompilation
        //         .GetDiagnostics()
        //         .Where(d => d.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)
        // );

        // Assert.Empty(diagnostics);
        return driver.GetRunResult();
    }
}

public class MockAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    public override AnalyzerConfigOptions GlobalOptions { get; }

    public MockAnalyzerConfigOptionsProvider(Dictionary<string, string> options)
    {
        GlobalOptions = new MockAnalyzerConfigOptions(options);
    }

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree)
    {
        return GlobalOptions;
    }

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile)
    {
        return GlobalOptions;
    }
}

public class MockAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private readonly Dictionary<string, string> _options;

    public MockAnalyzerConfigOptions(Dictionary<string, string> options)
    {
        _options = options;
    }

    public override bool TryGetValue(string key, [NotNullWhen(true)] out string? value)
    {
        return _options.TryGetValue(key, out value);
    }
}