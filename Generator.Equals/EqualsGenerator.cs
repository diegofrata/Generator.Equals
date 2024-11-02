using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Generator.Equals.Generators;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[assembly: InternalsVisibleTo("Generator.Equals.SnapshotTests")]
[assembly: InternalsVisibleTo("Generator.Equals.Tests.DynamicGeneration")]

namespace Generator.Equals;

[Generator(LanguageNames.CSharp)]
public class EqualsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "Generator.Equals.EquatableAttribute",
                (syntaxNode, ct) => syntaxNode is ClassDeclarationSyntax or RecordDeclarationSyntax or StructDeclarationSyntax,
                (syntaxContext, ct) => new EqualityTypeModelTransformer(syntaxContext).Transform(ct)
            );

        var config = context.AnalyzerConfigOptionsProvider
            .Select((options, _) => new GeneratorOptions(options.GlobalOptions));

        var combinedProvider = provider.Combine(config);


        // context.RegisterSourceOutput(provider, (spc, ctx) => Execute(spc, ctx));
        context.RegisterSourceOutput(combinedProvider, (spc, ctx) => Execute2(spc, ctx));
    }


    private void Execute2(SourceProductionContext productionContext, (EqualityTypeModel? model, GeneratorOptions options) ctx)
    {
        if (productionContext.CancellationToken.IsCancellationRequested || ctx.model is null)
        {
            return;
        }

        var model = ctx.model
            .WithGeneratorOptions(ctx.options);


        var source = model.SyntaxKind switch
        {
            SyntaxKind.StructDeclaration => StructGenerator.Generate(model),
            SyntaxKind.RecordStructDeclaration => RecordStructGenerator.Generate(model),
            SyntaxKind.RecordDeclaration => RecordGenerator.Generate(model),
            SyntaxKind.ClassDeclaration => ClassGenerator.Generate(model),
            _ => null
        };

        if (source is null)
        {
            return;
        }

        //Test: prepend  "CounterEnabled: {options.CounterEnabled}" to the generated source
        source = $"// CounterEnabled: {ctx.options.DefaultStringComparison}\n" +
                 $"// ArrayCompare: {ctx.options.ArrayCompare}\n" +
                 $"{source}";

        var fileName = $"{EscapeFileName(model.Fullname)}.Generator.Equals.g.cs"!;
        productionContext.AddSource(fileName, source);
    }

    private static readonly char[] _illegalFilenameChars = new[] { '<', '>', ',', ':' };

    private static string EscapeFileName(string fileName) => _illegalFilenameChars
        .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_'))
        .ToString();
}