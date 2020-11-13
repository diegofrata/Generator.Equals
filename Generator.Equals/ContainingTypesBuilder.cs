using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Equals
{
    internal static class ContainingTypesBuilder
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

                if (symbol is not INamespaceOrTypeSymbol namespaceOrTypeSymbol)
                    yield break;

                yield return namespaceOrTypeSymbol;
            }
        }

        public static string Build(ISymbol symbol, Action<StringBuilder> content, bool includeSelf = false)
        {
            var sb = new StringBuilder();
            var symbols = ContainingNamespaceAndTypes(symbol, includeSelf).ToList();

            for (var i = symbols.Count - 1; i >= 0; i--)
            {
                var s = symbols[i];

                if (s.IsNamespace)
                {
                    var namespaceName = s.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
                    sb.AppendLine($"namespace {namespaceName} {{");
                }
                else
                {
                    var keyword = s.DeclaringSyntaxReferences
                        .Select(x => x.GetSyntax())
                        .OfType<TypeDeclarationSyntax>()
                        .First()
                        .Keyword
                        .ValueText;
                 
                    var typeName = s.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                    sb.AppendLine($"partial {keyword} {typeName} {{");
                }
            }

            content(sb);
            sb.AppendLine();

            symbols.Aggregate(sb, (builder, _) => sb.AppendLine("}"));

            return sb.ToString();
        }
    }
}