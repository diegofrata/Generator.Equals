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

                if (!(symbol is INamespaceOrTypeSymbol namespaceOrTypeSymbol))
                    yield break;

                yield return namespaceOrTypeSymbol;
            }
        }

        public static string Build(ISymbol symbol, Action<StringBuilder, int> content, bool includeSelf = false)
        {
            // The test cases use 3000 characters on average, and these are the minimum classes.
            // It is also recommended to select a power of two as the initial value.
            var sb = new StringBuilder(capacity: 4096);
            var symbols = ContainingNamespaceAndTypes(symbol, includeSelf).ToList();
            var level = 0;

            for (var i = symbols.Count - 1; i >= 0; i--)
            {
                var s = symbols[i];

                if (s.IsNamespace)
                {
                    sb.AppendLine();
                    sb.AppendLine(EqualityGeneratorBase.EnableNullableContext);
                    sb.AppendLine(EqualityGeneratorBase.SuppressObsoleteWarningsPragma);
                    sb.AppendLine();

                    var namespaceName = s.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
                    sb.AppendLine($"namespace {namespaceName}");
                    sb.AppendOpenBracket(ref level);
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
                    sb.AppendLine(level, $"partial {keyword} {typeName}");
                    sb.AppendOpenBracket(ref level);
                }
            }

            content(sb, level);

            symbols.Aggregate(sb, (builder, _) => sb.AppendCloseBracket(ref level));

            return sb.ToString();
        }
    }
}
