namespace Generator.Equals.Models;

internal sealed class EqualityMemberModel
{
    public string PropertyName { get; }
    public string TypeName { get; }
    public EqualityType EqualityType { get; }
    public string? ComparerType { get; }
    public string? ComparerMemberName { get; }
    public string? StringComparer { get; }
    public bool IsDictionary { get; init; }

    public bool Ignored { get; set; }
    public bool ComparerHasStaticInstance { get; init; }

    public EqualityMemberModel(
        string propertyName,
        string typeName,
        EqualityType equalityType,
        string? comparerType = null,
        string? comparerMemberName = null,
        string? stringComparer = null
    )
    {
        PropertyName = propertyName;
        TypeName = typeName;
        EqualityType = equalityType;
        ComparerType = comparerType;
        ComparerMemberName = comparerMemberName;
        StringComparer = stringComparer;
    }
}