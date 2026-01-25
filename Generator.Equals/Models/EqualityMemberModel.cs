namespace Generator.Equals.Models;

internal sealed record EqualityMemberModel
{
    public required string PropertyName { get; init; }
    public required string TypeName { get; init; }
    public required EqualityType EqualityType { get; init; }
    public string? ComparerType { get; init; }
    public string? ComparerMemberName { get; init; }
    public string? StringComparer { get; init; }
    public bool IsDictionary { get; init; }

    public bool Ignored { get; init; }
    public bool ComparerHasStaticInstance { get; init; }

    // Element comparer properties for collection equality attributes
    public string? ElementComparerType { get; init; }
    public string? ElementComparerMemberName { get; init; }
    public bool ElementComparerHasStaticInstance { get; init; }
}