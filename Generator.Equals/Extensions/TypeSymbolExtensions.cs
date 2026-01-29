using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.Extensions;

/// <summary>
/// Extension methods for <see cref="ITypeSymbol"/> used by the analyzer.
/// </summary>
internal static class TypeSymbolExtensions
{
    private static readonly HashSet<string> PrimitiveTypeNames = new()
    {
        "System.Boolean",
        "System.Byte",
        "System.SByte",
        "System.Char",
        "System.Decimal",
        "System.Double",
        "System.Single",
        "System.Int32",
        "System.UInt32",
        "System.Int64",
        "System.UInt64",
        "System.Int16",
        "System.UInt16",
        "System.IntPtr",
        "System.UIntPtr",
        "System.Object",
        "System.String",
        "System.DateTime",
        "System.DateTimeOffset",
        "System.TimeSpan",
        "System.Guid",
        "System.Uri"
    };

    /// <summary>
    /// Determines if the type is a collection (implements IEnumerable&lt;T&gt;), excluding string.
    /// </summary>
    public static bool IsCollection(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol.SpecialType == SpecialType.System_String)
            return false;

        // Check if it's an array
        if (typeSymbol is IArrayTypeSymbol)
            return true;

        // Check if it implements IEnumerable<T>
        return typeSymbol.GetIEnumerableTypeArguments() != null;
    }

    /// <summary>
    /// Determines if the type is a complex user-defined type (not primitive, enum, System type, or collection).
    /// </summary>
    public static bool IsComplexType(this ITypeSymbol typeSymbol)
    {
        // Not a complex type if it's a primitive
        if (IsPrimitiveType(typeSymbol))
            return false;

        // Not a complex type if it's nullable of a primitive
        if (typeSymbol is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } namedType)
        {
            var underlyingType = namedType.TypeArguments[0];
            if (IsPrimitiveType(underlyingType))
                return false;
        }

        // Not complex if it's an enum
        if (typeSymbol.TypeKind == TypeKind.Enum)
            return false;

        // Not complex if it's a collection
        if (typeSymbol.IsCollection())
            return false;

        // Not complex if it's a well-known System type
        if (IsWellKnownSystemType(typeSymbol))
            return false;

        // Not complex if it's an interface or abstract type
        if (typeSymbol.TypeKind == TypeKind.Interface)
            return false;

        // It's a complex type if it's a user-defined class, struct, or record
        return typeSymbol.TypeKind is TypeKind.Class or TypeKind.Struct;
    }

    /// <summary>
    /// Gets the element type of a collection (T from IEnumerable&lt;T&gt;).
    /// For dictionaries, returns KeyValuePair&lt;TKey, TValue&gt;.
    /// </summary>
    public static ITypeSymbol? GetCollectionElementType(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol is IArrayTypeSymbol arrayType)
            return arrayType.ElementType;

        var arguments = typeSymbol.GetIEnumerableTypeArguments();
        return arguments?.Arguments?[0];
    }

    /// <summary>
    /// Determines if the type is a dictionary (implements IDictionary&lt;TKey, TValue&gt;).
    /// </summary>
    public static bool IsDictionary(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.GetIDictionaryTypeArguments() != null;
    }

    /// <summary>
    /// Determines if the type is a set (implements ISet&lt;T&gt;).
    /// </summary>
    public static bool IsSet(this ITypeSymbol typeSymbol)
    {
        return typeSymbol.GetInterface("global::System.Collections.Generic.ISet<T>") != null;
    }

    private static bool IsPrimitiveType(ITypeSymbol typeSymbol)
    {
        if (typeSymbol.SpecialType != SpecialType.None)
            return true;

        var fullName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            .Replace("global::", "");

        return PrimitiveTypeNames.Contains(fullName);
    }

    private static bool IsWellKnownSystemType(ITypeSymbol typeSymbol)
    {
        var containingNamespace = typeSymbol.ContainingNamespace?.ToDisplayString();
        if (containingNamespace == null)
            return false;

        // Allow user types in non-System namespaces
        if (!containingNamespace.StartsWith("System"))
            return false;

        // System types are typically in System, System.Collections, etc.
        // But we want to flag user types that happen to be in the System namespace
        // So we check for common BCL types
        var fullName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            .Replace("global::", "");

        // Common well-known BCL types that are not "complex" for our purposes
        return fullName.StartsWith("System.Collections.") ||
               fullName.StartsWith("System.Threading.") ||
               fullName.StartsWith("System.IO.") ||
               fullName.StartsWith("System.Net.") ||
               fullName.StartsWith("System.Text.") ||
               fullName.StartsWith("System.Tuple") ||
               fullName.StartsWith("System.ValueTuple") ||
               fullName == "System.Type" ||
               fullName == "System.Version" ||
               fullName == "System.Exception";
    }
}
