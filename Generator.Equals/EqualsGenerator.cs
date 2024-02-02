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
                    (syntaxNode, _) =>
                    {
                        return syntaxNode switch
                        {
                            ClassDeclarationSyntax => true,
                            RecordDeclarationSyntax => true,
                            StructDeclarationSyntax => true,
                            _ => false
                        };
                    },
                    (syntaxContext, ct) => syntaxContext);

            var combined = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(combined, (spc, pair) => Execute(spc, pair.Left, pair.Right));
        }


        void Execute(SourceProductionContext productionContext, Compilation compilation,
            ImmutableArray<GeneratorAttributeSyntaxContext> syntaxArr)
        {
            // Build a lookup for the System.StringComparison enum based on the compilation unit
            INamedTypeSymbol typeSymbol = compilation.GetTypeByMetadataName("System.StringComparison")!;

            if (typeSymbol is not { TypeKind: TypeKind.Enum })
            {
                throw new Exception("should not have gotten here. System.StringComparison is not an enum.");
            }

            // Assume: Underlying type of enum is always `long`
            var stringComparisonLookup = typeSymbol
                .GetMembers()
                .OfType<IFieldSymbol>()
                .ToImmutableDictionary(key => Convert.ToInt64(key.ConstantValue), elem => elem.Name);
            

            var attributesMetadata = new AttributesMetadata(
                compilation.GetTypeByMetadataName("Generator.Equals.EquatableAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.DefaultEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.OrderedEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.IgnoreEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.UnorderedEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.ReferenceEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.SetEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.StringEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.CustomEqualityAttribute")!,
                stringComparisonLookup
            );
            
            var handledSymbols = new HashSet<string>();

            foreach (var item in syntaxArr)
            {
                var node = item.TargetNode;
                var model = item.SemanticModel;

                var symbol = model.GetDeclaredSymbol(node, productionContext.CancellationToken) as ITypeSymbol;

                var equatableAttributeData = symbol?.GetAttributes().FirstOrDefault(x =>
                    x.AttributeClass?.Equals(attributesMetadata.Equatable, SymbolEqualityComparer.Default) ==
                    true);

                if (equatableAttributeData == null)
                    continue;

                var symbolDisplayString = symbol!.ToDisplayString();

                if (handledSymbols.Contains(symbolDisplayString))
                    continue;

                handledSymbols.Add(symbolDisplayString);

                var explicitMode = equatableAttributeData.NamedArguments
                    .FirstOrDefault(pair => pair.Key == "Explicit")
                    .Value.Value is true;

                var ignoreInheritedMembers = equatableAttributeData.NamedArguments
                    .FirstOrDefault(pair => pair.Key == "IgnoreInheritedMembers")
                    .Value.Value is true;

                var source = node switch
                {
                    StructDeclarationSyntax _ => StructEqualityGenerator.Generate(symbol!, attributesMetadata,
                        explicitMode),
                    RecordDeclarationSyntax _ when node.IsKind(SyntaxKind.RecordStructDeclaration) =>
                        RecordStructEqualityGenerator.Generate(symbol!, attributesMetadata, explicitMode),
                    RecordDeclarationSyntax _ => RecordEqualityGenerator.Generate(symbol!, attributesMetadata,
                        explicitMode, ignoreInheritedMembers),
                    ClassDeclarationSyntax _ => ClassEqualityGenerator.Generate(symbol!, attributesMetadata,
                        explicitMode, ignoreInheritedMembers),
                    _ => throw new Exception("should not have gotten here.")
                };

                var fileName = $"{EscapeFileName(symbolDisplayString)}.Generator.Equals.g.cs"!;

                productionContext.AddSource(fileName, source);
            }

            static string EscapeFileName(string fileName) => new[] { '<', '>', ',' }
                .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_')).ToString();
        }
    }
}