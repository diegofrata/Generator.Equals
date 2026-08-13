using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

record ContainingSymbol
{
    public required string Name { get; init; }
}

sealed record NamespaceContainingSymbol : ContainingSymbol
{
}

sealed record TypeContainingSymbol : ContainingSymbol
{
    public SyntaxKind? Kind { get; init; }
}