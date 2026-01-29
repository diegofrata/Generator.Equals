using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

sealed record EqualityTypeModel
{
    public required SyntaxKind SyntaxKind { get; init; }

    public required string TypeName { get; init; }
    public required string? BaseTypeName { get; init; }
    public required bool IsSealed { get; init; }
    public required EquatableImmutableArray<ContainingSymbol> ContainingSymbols { get; init; }
    public required AttributesMetadata AttributesMetadata { get; init; }
    public required bool ExplicitMode { get; init; }
    public required bool IgnoreInheritedMembers { get; init; }
    public required EquatableImmutableArray<EqualityMemberModel> BuildEqualityModels { get; init; }

    /// <summary>
    /// Properties collected from ancestor types that don't have [Equatable].
    /// These are only populated when IgnoreInheritedMembers=false and BaseHasEquatable=false.
    /// </summary>
    public EquatableImmutableArray<EqualityMemberModel> InheritedEqualityModels { get; init; }
    public required string Fullname { get; init; }

    /// <summary>
    /// For classes, indicates whether the base type has [Equatable].
    /// If false, base.Equals() would use object reference equality,
    /// so the class should be treated as a "root class" for equality purposes.
    /// </summary>
    public bool BaseHasEquatable { get; init; }
}