using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

internal sealed record class EqualityTypeModel
{
    public SyntaxKind SyntaxKind { get; init; }

    public string TypeName { get; }
    public string? BaseTypeName { get; init; }
    public bool IsSealed { get; init; }
    public ImmutableArray<ContainingSymbol> ContainingSymbols { get; }
    public AttributesMetadata AttributesMetadata { get; }
    public bool ExplicitMode { get; }
    public bool IgnoreInheritedMembers { get; }
    public ImmutableArray<EqualityMemberModel> BuildEqualityModels { get; init; }
    public string Fullname { get; init; }

    public EqualityTypeModel(
        string typeName,
        ImmutableArray<ContainingSymbol> containingSymbols,
        AttributesMetadata attributesMetadata,
        bool explicitMode,
        bool ignoreInheritedMembers = false
    )
    {
        TypeName = typeName;
        ContainingSymbols = containingSymbols;
        AttributesMetadata = attributesMetadata;
        ExplicitMode = explicitMode;
        IgnoreInheritedMembers = ignoreInheritedMembers;
    }
}