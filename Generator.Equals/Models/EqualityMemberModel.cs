namespace Generator.Equals.Models;

sealed record EqualityMemberModel
{
    public required string MemberName { get; init; }
    public bool IsField { get; init; }
    public required string TypeName { get; init; }
    public required EqualityType EqualityType { get; init; }
    public string? ComparerType { get; init; }
    public string? ComparerMemberName { get; init; }
    public string? StringComparer { get; init; }
    public bool IsDictionary { get; init; }
    public bool IsValueTypeCollection { get; init; }
    public double? Precision { get; init; }
    public bool IsNullable { get; init; }

    public bool Ignored => EqualityType == EqualityType.IgnoreEquality;
    public bool ComparerHasStaticInstance { get; init; }

    // Element comparer properties for collection equality attributes
    public string? ElementComparerType { get; init; }
    public string? ElementComparerMemberName { get; init; }
    public bool ElementComparerHasStaticInstance { get; init; }

    /// <summary>
    /// FQF of the element type when it has [Equatable], enabling Inequalities drill-down.
    /// Null when the element type is not equatable.
    /// </summary>
    public string? EquatableElementTypeName { get; init; }
}