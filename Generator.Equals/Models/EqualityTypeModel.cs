using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

/// <summary>
/// How the base inheritance chain owns equality, if at all. Computed once by the transformer in a single
/// upward walk: a generated comparer <em>anywhere</em> wins; whether a hand-written contract sits below it
/// decides <see cref="Comparer"/> vs <see cref="ComparerBehindManual"/>; with no comparer at all, a
/// hand-written complete contract makes the base <see cref="Manual"/>.
/// </summary>
enum BaseEqualityOwnership
{
    /// <summary>No ancestor owns equality; inherited members are compared explicitly.</summary>
    None,

    /// <summary>
    /// The nearest equality-owning ancestor exposes a generated EqualityComparer ([Equatable] or
    /// cross-assembly) with no hand-written contract in between; delegate to that comparer.
    /// </summary>
    Comparer,

    /// <summary>
    /// A comparer ancestor exists, but a hand-written complete contract sits between it and this type.
    /// base.Equals() honors that contract, but its members are invisible to the (inherited) comparer, so
    /// Inequalities must go through the base-equality bridge instead of delegating member-level.
    /// </summary>
    ComparerBehindManual,

    /// <summary>
    /// An ancestor hand-rolls a complete Equals/GetHashCode contract and no comparer exists anywhere;
    /// delegate via base.Equals()/base.GetHashCode() and the bridge.
    /// </summary>
    Manual,
}

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
    /// Properties of plain ancestors (no equality of their own) below the nearest equality-owning
    /// ancestor, compared explicitly by this type. Empty when IgnoreInheritedMembers=true.
    /// </summary>
    public EquatableImmutableArray<EqualityMemberModel> InheritedEqualityModels { get; init; }
    public required string Fullname { get; init; }

    /// <summary>How the base chain owns equality. See <see cref="BaseEqualityOwnership"/>.</summary>
    public BaseEqualityOwnership BaseEquality { get; init; }

    /// <summary>
    /// Whether a generated EqualityComparer is inherited from an ancestor (so the nested comparer
    /// declared here hides it and needs the <c>new</c> modifier).
    /// </summary>
    public bool BaseHasEquatable => BaseEquality is BaseEqualityOwnership.Comparer or BaseEqualityOwnership.ComparerBehindManual;

    /// <summary>
    /// Whether the IMMEDIATE base type exposes its own generated EqualityComparer. When false but
    /// <see cref="BaseHasEquatable"/> is true, <c>{immediateBase}.EqualityComparer</c> resolves to an
    /// inherited ancestor comparer that knows nothing about the intermediate's members. Records use this
    /// to choose between member-level Inequalities delegation and the coarse bridge.
    /// </summary>
    public bool ImmediateBaseHasComparer { get; init; }

    /// <summary>
    /// For classes, the argument expression of the <c>base.Equals(...)</c> delegation call, cast so
    /// overload resolution binds to the intended base overload (the immediate base's generated typed
    /// Equals, a hand-written ancestor's public typed Equals, or <c>object</c> for a hand-written
    /// <c>Equals(object)</c>). Null when <see cref="BaseEquality"/> is <see cref="BaseEqualityOwnership.None"/>.
    /// </summary>
    public string? BaseEqualsArgument { get; init; }

    /// <summary>
    /// For classes, indicates whether the decorated class should generate == and != operators.
    /// <br/>It has no impact on struct (operator always generated) and record types (operator compiler-emitted).
    /// </summary>
    public required bool GenerateClassEqualityOperators { get; init; }
}