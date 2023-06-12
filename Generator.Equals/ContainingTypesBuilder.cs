using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Equals
{
    static class ContainingTypesBuilder
    {
        static IEnumerable<INamespaceOrTypeSymbol> ContainingNamespaceAndTypes(ISymbol symbol, bool includeSelf)
        {
            foreach (var item in AllContainingNamespacesAndTypes(symbol, includeSelf))
            {
                yield return item;

                if (item.IsNamespace)
                    yield break;
            }
        }

        static IEnumerable<INamespaceOrTypeSymbol> AllContainingNamespacesAndTypes(ISymbol symbol, bool includeSelf)
        {
            if (includeSelf && symbol is INamespaceOrTypeSymbol self)
                yield return self;

            while (true)
            {
                symbol = symbol.ContainingSymbol;

                if (!(symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol))
                    yield break;

                yield return namespaceOrTypeSymbol;
            }
        }

        public static string Build(ISymbol symbol, Action<IndentedTextWriter> content, bool includeSelf = false)
        {
            var buffer = new StringWriter(new StringBuilder(capacity: 4096));
            var writer = new IndentedTextWriter(buffer);
            var symbols = ContainingNamespaceAndTypes(symbol, includeSelf).ToList();

            for (var i = symbols.Count - 1; i >= 0; i--)
            {
                var s = symbols[i];

                if (s.IsNamespace)
                {
                    writer.WriteLine();
                    writer.WriteLine(EqualityGeneratorBase.EnableNullableContext);
                    writer.WriteLine(EqualityGeneratorBase.SuppressObsoleteWarningsPragma);
                    writer.WriteLine(EqualityGeneratorBase.SuppressTypeConflictsWarningsPragma);
                    writer.WriteLine();

                    var namespaceName = s.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));

                    if (!string.IsNullOrEmpty(namespaceName))
                    {
                        writer.WriteLine($"namespace {namespaceName}");
                        writer.AppendOpenBracket();
                    }
                }
                else
                {
                    var typeDeclarationSyntax = s.DeclaringSyntaxReferences
                        .Select(x => x.GetSyntax())
                        .OfType<TypeDeclarationSyntax>()
                        .First();

                    var keyword = typeDeclarationSyntax.Kind() switch
                    {
                        SyntaxKind.ClassDeclaration => "class",
                        SyntaxKind.RecordDeclaration => "record",
                        (SyntaxKind)9068 => "record struct", // RecordStructDeclaration
                        SyntaxKind.StructDeclaration => "struct",
                        var x => throw new ArgumentOutOfRangeException($"Syntax kind {x} not supported")
                    };

                    var typeName = s.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                    writer.WriteLine($"partial {keyword} {typeName}");
                    writer.AppendOpenBracket();
                }
            }

            content(writer);

            writer.UnwindOpenedBrackets();

            return buffer.ToString();
        }
    }
}