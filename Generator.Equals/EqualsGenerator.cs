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
                (syntaxContext, ct) => new EqualityTypeModelTransformer(syntaxContext, ct).Transform()
            );

        context.RegisterSourceOutput(provider, (spc, ctx) => Execute(spc, ctx));
    }

    private static void Execute(SourceProductionContext productionContext, EqualityTypeModel? model)
    {
        if (productionContext.CancellationToken.IsCancellationRequested || model is null)
        {
            return;
        }

        var source = model.SyntaxKind switch
        {
            SyntaxKind.StructDeclaration => StructEqualityGenerator.Generate(model),
            SyntaxKind.RecordStructDeclaration => RecordStructEqualityGenerator.Generate(model),
            SyntaxKind.RecordDeclaration => RecordEqualityGenerator.Generate(model),
            SyntaxKind.ClassDeclaration => ClassEqualityGenerator.Generate(model),
            _ => null
        };

        if (source is null)
        {
            return;
        }

        var fileName = $"{EscapeFileName(model.Fullname)}.Generator.Equals.g.cs"!;
        productionContext.AddSource(fileName, source);
    }

    private static readonly char[] _illegalFilenameChars = new[] { '<', '>', ',', ':' };

    private static string EscapeFileName(string fileName) => _illegalFilenameChars
        .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_'))
        .ToString();
}