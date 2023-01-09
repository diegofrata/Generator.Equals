using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
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
            context.RegisterPostInitializationOutput(c =>
            {
                var assembly = GetType().Assembly;

                foreach (var name in assembly.GetManifestResourceNames())
                {
                    if (!name.EndsWith(".cs") || !name.StartsWith("Generator.Equals.Runtime")) continue;

                    using var stream = assembly.GetManifestResourceStream(name);
                    using var reader = new StreamReader(stream!);
                    var contents = new StringBuilder();
                    contents.AppendLine(EqualityGeneratorBase.EnableNullableContext);
                    contents.AppendLine(EqualityGeneratorBase.SuppressTypeConflictsWarningsPragma);
                    contents.Append(reader.ReadToEnd());
                    c.AddSource(name, contents.ToString());
                }
            });

            bool HasEquatableAttribute(SyntaxList<AttributeListSyntax> c)
            {
                return c.Any(list =>
                    list.Attributes.Any(
                        syntax =>
                            syntax.Name.ToString().EndsWith("Equatable") ||
                            syntax.Name.ToString().EndsWith("EquatableAttribute")
                    )
                );
            }

            var provider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    (syntaxNode, ct) =>
                    {
                        return syntaxNode switch
                        {
                            ClassDeclarationSyntax a when HasEquatableAttribute(a.AttributeLists) => true,
                            RecordDeclarationSyntax b when HasEquatableAttribute(b.AttributeLists) => true,
                            StructDeclarationSyntax c when HasEquatableAttribute(c.AttributeLists) => true,
                            _ => false
                        };
                    },
                    (syntaxContext, ct) => syntaxContext);

            var combined = context.CompilationProvider.Combine(provider.Collect());

            context.RegisterSourceOutput(combined, (spc, pair) => Execute(spc, pair.Left, pair.Right));
        }

        void Validate(SourceProductionContext productionContext, Compilation compilation)
        {
            if (compilation.GetTypeByMetadataName("System.HashCode") is null)
            {
                productionContext.ReportDiagnostic(
                    Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "GENEQ001",
                            "Missing System.HashCode",
                            "[Generator.Equals] System.HashCode does not seem to exist. To fix this error, add package Microsoft.Bcl.HashCode.",
                            "Generator.Equals",
                            DiagnosticSeverity.Error,
                            true
                        ),
                        Location.None
                    )
                );
            }
        }

        void Execute(SourceProductionContext productionContext, Compilation compilation,
            ImmutableArray<GeneratorSyntaxContext> syntaxArr)
        {
            Validate(productionContext, compilation);

            var attributesMetadata = new AttributesMetadata(
                compilation.GetTypeByMetadataName("Generator.Equals.EquatableAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.DefaultEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.OrderedEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.IgnoreEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.UnorderedEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.ReferenceEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.SetEqualityAttribute")!,
                compilation.GetTypeByMetadataName("Generator.Equals.CustomEqualityAttribute")!
            );

            var handledSymbols = new HashSet<string>();

            foreach (var item in syntaxArr)
            {
                var node = item.Node;
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