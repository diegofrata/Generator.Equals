using Microsoft.CodeAnalysis;

namespace Generator.Equals.Analyzers;

/// <summary>
/// Contains all diagnostic descriptors for the Generator.Equals analyzer.
/// </summary>
public static class DiagnosticDescriptors
{
    private const string Category = "Generator.Equals";

    /// <summary>
    /// GE001: Collection property missing equality attribute.
    /// </summary>
    public static readonly DiagnosticDescriptor CollectionMissingAttribute = new(
        id: "GE001",
        title: "Collection property missing equality attribute",
        messageFormat: "Collection property '{0}' should have an equality attribute like [OrderedEquality], [UnorderedEquality], or [SetEquality]",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Collection properties in an [Equatable] type should specify how elements are compared using [OrderedEquality], [UnorderedEquality], or [SetEquality] attributes.");

    /// <summary>
    /// GE002: Complex object property type lacks [Equatable].
    /// </summary>
    public static readonly DiagnosticDescriptor ComplexTypeMissingEquatable = new(
        id: "GE002",
        title: "Complex object property type lacks [Equatable]",
        messageFormat: "Property '{0}' has type '{1}' which is a complex type without [Equatable] attribute",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "User-defined classes and structs used in [Equatable] types should also have the [Equatable] attribute for proper deep equality comparison.");

    /// <summary>
    /// GE003: Collection element type lacks [Equatable].
    /// </summary>
    public static readonly DiagnosticDescriptor CollectionElementMissingEquatable = new(
        id: "GE003",
        title: "Collection element type lacks [Equatable]",
        messageFormat: "Collection property '{0}' has element type '{1}' which is a complex type without [Equatable] attribute",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        description: "Complex types used as collection elements in [Equatable] types should also have the [Equatable] attribute for proper deep equality comparison.");

    /// <summary>
    /// GE004: Equality attribute used but containing type lacks [Equatable].
    /// </summary>
    public static readonly DiagnosticDescriptor OrphanedEqualityAttribute = new(
        id: "GE004",
        title: "Equality attribute used but containing type lacks [Equatable]",
        messageFormat: "Property '{0}' has [{1}] attribute but containing type '{2}' is not marked with [Equatable]",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Equality attributes like [OrderedEquality], [IgnoreEquality], etc. have no effect unless the containing type is marked with [Equatable].");

    /// <summary>
    /// GE005: Type with [Equatable] has manual Equals/GetHashCode implementation.
    /// </summary>
    public static readonly DiagnosticDescriptor ManualEqualsImplementation = new(
        id: "GE005",
        title: "Type with [Equatable] has manual Equals/GetHashCode",
        messageFormat: "Type '{0}' has [Equatable] attribute but also has a manual {1} implementation",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Types with [Equatable] attribute have Equals and GetHashCode generated automatically. Manual implementations may conflict or be overwritten.");

    /// <summary>
    /// GE006: [Equatable] on non-partial type.
    /// </summary>
    public static readonly DiagnosticDescriptor NonPartialType = new(
        id: "GE006",
        title: "[Equatable] on non-partial type",
        messageFormat: "Type '{0}' has [Equatable] attribute but is not declared as partial",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Types with [Equatable] attribute must be declared as partial so that the generator can add the Equals and GetHashCode methods.");

    /// <summary>
    /// GE007: Conflicting attributes on same property.
    /// </summary>
    public static readonly DiagnosticDescriptor ConflictingAttributes = new(
        id: "GE007",
        title: "Conflicting attributes on same property",
        messageFormat: "Property '{0}' has conflicting equality attributes [{1}] and [{2}]",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "A property can only have one equality attribute. Remove duplicate or conflicting attributes.");

    /// <summary>
    /// GE008: [StringEquality] on non-string type.
    /// </summary>
    public static readonly DiagnosticDescriptor StringEqualityOnNonString = new(
        id: "GE008",
        title: "[StringEquality] on non-string type",
        messageFormat: "Property '{0}' has [StringEquality] attribute but its type is '{1}', not string",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "[StringEquality] attribute can only be used on properties of type string.");

    /// <summary>
    /// GE009: Collection equality attribute on non-collection type.
    /// </summary>
    public static readonly DiagnosticDescriptor CollectionAttributeOnNonCollection = new(
        id: "GE009",
        title: "Collection equality attribute on non-collection type",
        messageFormat: "Property '{0}' has [{1}] attribute but its type '{2}' is not a collection",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Collection equality attributes like [OrderedEquality], [UnorderedEquality], and [SetEquality] can only be used on collection types (IEnumerable<T>).");

    /// <summary>
    /// GE010: [PrecisionEquality] on unsupported type.
    /// </summary>
    public static readonly DiagnosticDescriptor PrecisionEqualityOnUnsupportedType = new(
        id: "GE010",
        title: "[PrecisionEquality] on unsupported type",
        messageFormat: "Property '{0}' has [PrecisionEquality] attribute but its type is '{1}', which is not a supported numeric type (float, double, decimal, int, long, short, sbyte, or their nullable variants)",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "[PrecisionEquality] attribute can only be used on properties of type float, double, decimal, int, long, short, sbyte, or their nullable variants.");
}
