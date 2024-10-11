using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

using Generator.Equals.Models;

using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals
{
    internal static class ContainingTypesBuilder
    {
        public static string Build(ImmutableArray<ContainingSymbol> containingSymbols, Action<IndentedTextWriter> content)
        {
            using var buffer = new StringWriter(new StringBuilder(capacity: 4096));
            using var writer = new IndentedTextWriter(buffer);

            foreach (var parentSymbol in containingSymbols.Reverse())
            {
                if (parentSymbol is NamespaceContainingSymbol namespaceSymbol)
                {
                    writer.WriteLine();
                    writer.WriteLine(EqualityGeneratorBase.EnableNullableContext);
                    writer.WriteLine(EqualityGeneratorBase.SuppressObsoleteWarningsPragma);
                    writer.WriteLine(EqualityGeneratorBase.SuppressTypeConflictsWarningsPragma);
                    writer.WriteLine();

                    if (!string.IsNullOrEmpty(namespaceSymbol.Name))
                    {
                        writer.WriteLine($"namespace {namespaceSymbol.Name}");
                        writer.AppendOpenBracket();
                    }
                }
                else if (parentSymbol is TypeContainingSymbol typeContainingSymbol)
                {
                    var keyword = typeContainingSymbol.Kind switch
                    {
                        SyntaxKind.ClassDeclaration => "class",
                        SyntaxKind.RecordDeclaration => "record",
                        SyntaxKind.RecordStructDeclaration => "record struct",
                        SyntaxKind.StructDeclaration => "struct",
                        var x => throw new ArgumentOutOfRangeException($"Syntax kind {x} not supported")
                    };

                    writer.WriteLine($"partial {keyword} {parentSymbol.Name}");
                    writer.AppendOpenBracket();
                }
            }

            content(writer);

            writer.UnwindOpenedBrackets();

            return buffer.ToString();
        }
    }
}