using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals;

internal static class ContainingTypesBuilder
{
    public static string Build(EqualityTypeModel model, Action<EqualityTypeModel, IndentedTextWriter> content)
    {
        var sb = new StringBuilder(capacity: 4096);
        using (var writer = CreateWriter(sb, model.ContainingSymbols))
        {
            content(model, writer);
        }

        return sb.ToString();
    }

    public static IndentedTextWriter CreateWriter(StringBuilder sb, ImmutableArray<ContainingSymbol> containingSymbols)
    {
        var writer = new UnwindingTextWriter(sb);
        foreach (var parentSymbol in containingSymbols.Reverse())
        {
            switch (parentSymbol)
            {
                case NamespaceContainingSymbol namespaceSymbol:
                {
                    writer.WriteLine();
                    writer.WriteLine(GeneratorConstants.EnableNullableContext);
                    writer.WriteLine(GeneratorConstants.SuppressObsoleteWarningsPragma);
                    writer.WriteLine(GeneratorConstants.SuppressTypeConflictsWarningsPragma);
                    writer.WriteLine();

                    if (!string.IsNullOrEmpty(namespaceSymbol.Name))
                    {
                        writer.WriteLine($"namespace {namespaceSymbol.Name}");
                        writer.AppendOpenBracket();
                    }

                    break;
                }
                case TypeContainingSymbol typeContainingSymbol:
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
                    break;
                }
            }
        }

        return writer;
    }

    internal class UnwindingTextWriter(StringBuilder sb)
        : IndentedTextWriter(new StringWriter(sb))
    {
        protected override void Dispose(bool disposing)
        {
            this.UnwindOpenedBrackets();
            base.Dispose(disposing);
        }
    }
}