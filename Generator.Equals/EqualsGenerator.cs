using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Equals
{
    [Generator]
    class EqualsGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            Debugger.Launch();
#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not SyntaxReceiver s) return;

            var attributesMetadata = new AttributesMetadata(
                context.Compilation.GetTypeByMetadataName("Generator.Equals.EquatableAttribute")!,
                context.Compilation.GetTypeByMetadataName("Generator.Equals.SequenceEqualityAttribute")!
            );

            foreach (var node in s.CandidateSyntaxes)
            {
                var model = context.Compilation.GetSemanticModel(node.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(node, context.CancellationToken) as ITypeSymbol;

                var equatableAttributeData = symbol?.GetAttributes().FirstOrDefault(x =>
                    x.AttributeClass?.Equals(attributesMetadata.Equatable, SymbolEqualityComparer.Default) ==
                    true);

                if (equatableAttributeData == null)
                    continue;

                var source = node switch
                {
                    RecordDeclarationSyntax => RecordEqualityGenerator.Generate(symbol!, attributesMetadata),
                    ClassDeclarationSyntax => ClassEqualityGenerator.Generate(symbol!, attributesMetadata),
                    _ => throw new Exception("should not have gotten here.")
                };

                var fileName = $"{symbol!.ToDisplayString()}.Generator.Equals.g.cs"!;
                context.AddSource(fileName, source);
            }
        }

        class SyntaxReceiver : ISyntaxReceiver
        {
            private readonly List<SyntaxNode> _candidateSyntaxes = new List<SyntaxNode>();

            public IReadOnlyList<SyntaxNode> CandidateSyntaxes => _candidateSyntaxes;

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is not ClassDeclarationSyntax c && syntaxNode is not RecordDeclarationSyntax)
                    return;

                _candidateSyntaxes.Add(syntaxNode);
            }
        }
    }
}