using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

internal record ContainingSymbol
{
    public string Name { get; init; }
}

internal sealed record NamespaceContainingSymbol : ContainingSymbol
{
}

internal sealed record TypeContainingSymbol : ContainingSymbol
{
    public SyntaxKind? Kind { get; init; }
}