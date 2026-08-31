using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

sealed record EqualityTypeModel
{
    public required SyntaxKind SyntaxKind { get; init; }

    public required string TypeName { get; init; }
    public required string? BaseTypeName { get; init; }

    /// <summary>
    /// Fully qualified name of the base type (e.g., global::Namespace.BaseClass).
    /// Used for generating base comparer calls like BaseClass.EqualityComparer.Default.
    /// </summary>
    public string? BaseTypeFullname { get; init; }

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
    /// For classes, indicates whether the base type has [Equatable] or a generated EqualityComparer.
    /// If true, the generated equality delegates to the base's generated comparer / <c>base.Equals()</c>.
    /// </summary>
    public bool BaseHasEquatable { get; init; }

    /// <summary>
    /// For classes, indicates that the delegated base portion includes a hand-written complete equality
    /// contract — some ancestor overrides BOTH <c>Equals(object)</c> and <c>GetHashCode()</c> on a single
    /// type. Such a contract is honored by <c>base.Equals()</c>/<c>base.GetHashCode()</c> but is invisible
    /// to member-level comparer delegation, so <c>Inequalities</c> reports the base portion coarsely via the
    /// <c>__BaseEquals</c> bridge. This is true both when no comparer exists anywhere (pure manual base) and
    /// when a comparer ancestor sits above a hand-written intermediate (<see cref="BaseHasEquatable"/> is
    /// then also true).
    /// </summary>
    public bool BaseHasManualEquality { get; init; }

    /// <summary>
    /// Whether the IMMEDIATE base type exposes its own generated <c>EqualityComparer</c> (it has
    /// <c>[Equatable]</c> or a generated comparer). When false but <see cref="BaseHasEquatable"/> is true,
    /// a comparer ancestor is reached only through a non-comparer intermediate, so
    /// <c>{immediateBase}.EqualityComparer</c> resolves to the inherited ancestor comparer and skips that
    /// intermediate's members. The record generator uses this to decide between member-level
    /// <c>Inequalities</c> delegation and reporting the base portion coarsely via the bridge.
    /// </summary>
    public bool ImmediateBaseHasComparer { get; init; }

    /// <summary>
    /// For classes, indicates whether the decorated class should generate == and != operators.
    /// <br/>It has no impact on struct (operator always generated) and record types (operator compiler-emitted).
    /// </summary>
    public required bool GenerateClassEqualityOperators { get; init; }
}