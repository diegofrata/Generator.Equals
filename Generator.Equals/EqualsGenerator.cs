using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[assembly: InternalsVisibleTo("Generator.Equals.SnapshotTests")]

namespace Generator.Equals
{
    [Generator(LanguageNames.CSharp)]
    public class EqualsGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var provider = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "Generator.Equals.EquatableAttribute",
                    (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax or RecordDeclarationSyntax or StructDeclarationSyntax,
                    (syntaxContext, ct) => syntaxContext);

            context.RegisterSourceOutput(provider, (spc, context) => Execute(spc, context));
        }

        private void Execute(
            SourceProductionContext productionContext,
            GeneratorAttributeSyntaxContext syntax
        )
        {
            var attributesMetadata = AttributesMetadata.Instance;

            var handledSymbols = new HashSet<string>();

            var node = syntax.TargetNode;
            var model = syntax.SemanticModel;

            var symbol = model.GetDeclaredSymbol(node, productionContext.CancellationToken) as ITypeSymbol;

            var equatableAttributeData = symbol?.GetAttributes().FirstOrDefault(x =>
                attributesMetadata.Equatable.Equals(x.AttributeClass));

            if (equatableAttributeData == null)
                return;

            var symbolDisplayString = symbol!.ToDisplayString();

            if (handledSymbols.Contains(symbolDisplayString))
                return;

            handledSymbols.Add(symbolDisplayString);

            var explicitMode = equatableAttributeData.NamedArguments
                .FirstOrDefault(pair => pair.Key == "Explicit")
                .Value.Value is true;

            var ignoreInheritedMembers = equatableAttributeData.NamedArguments
                .FirstOrDefault(pair => pair.Key == "IgnoreInheritedMembers")
                .Value.Value is true;

            var source = node switch
            {
                StructDeclarationSyntax _
                    => StructEqualityGenerator.Generate(symbol!, attributesMetadata, explicitMode),

                RecordDeclarationSyntax _ when node.IsKind(SyntaxKind.RecordStructDeclaration)
                    => RecordStructEqualityGenerator.Generate(symbol!, attributesMetadata, explicitMode),

                RecordDeclarationSyntax _
                    => RecordEqualityGenerator.Generate(symbol!, attributesMetadata, explicitMode, ignoreInheritedMembers),

                ClassDeclarationSyntax _
                    => ClassEqualityGenerator.Generate(symbol!, attributesMetadata, explicitMode, ignoreInheritedMembers),

                _ => throw new Exception("should not have gotten here.")
            };

            var fileName = $"{EscapeFileName(symbolDisplayString)}.Generator.Equals.g.cs"!;

            productionContext.AddSource(fileName, source);

            static string EscapeFileName(string fileName) => new[] { '<', '>', ',' }
                .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_')).ToString();
        }
    }
}