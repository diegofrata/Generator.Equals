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
    
    
    /// <summary>
    /// OrderedEquality vs UnorderedEquality.
    /// Specifies that the equality type is not explicitly set and can be overridden by global settings.
    /// </summary>
    public bool IsDefaultEqualityType { get; init; }
    public bool IsDefaultStringComparer { get; init; }
}