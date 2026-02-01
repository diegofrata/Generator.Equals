using System;
using System.Collections.Generic;
using System.Linq;
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
        // Not a complex type if it's a primitive (includes enums)
        if (IsPrimitiveType(typeSymbol))
            return false;

        // Not a complex type if it's nullable of a primitive
        if (typeSymbol is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } namedType)
        {
            if (IsPrimitiveType(namedType.TypeArguments[0]))
                return false;
        }

        // Not complex if it's a collection
        if (typeSymbol.IsCollection())
            return false;

        // Not complex if it's a well-known System type
        if (IsWellKnownSystemType(typeSymbol))
            return false;

        // Not complex if it's an interface or abstract type
        if (typeSymbol.TypeKind == TypeKind.Interface)
            return false;

        // Not complex if it has proper equality (attribute or IEquatable<T>)
        if (typeSymbol.HasProperEquality())
            return false;

        // Records and structs with only value-equatable members are not complex
        if (typeSymbol is INamedTypeSymbol nts &&
            (nts.IsRecord || typeSymbol.TypeKind == TypeKind.Struct))
        {
            if (HasDeepValueEquality(nts, new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default)))
                return false;
        }

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

    /// <summary>
    /// Determines if a record or struct has deep value equality by checking all its members recursively.
    /// A type has deep value equality if all its properties/fields are primitives, enums, strings,
    /// or other types that themselves have deep value equality.
    /// </summary>
    private static bool HasDeepValueEquality(INamedTypeSymbol typeSymbol, HashSet<ITypeSymbol> visited)
    {
        // Prevent infinite recursion for circular references
        if (!visited.Add(typeSymbol))
            return true; // Already being checked, assume OK to break cycle

        // Check all instance properties and fields
        foreach (var member in typeSymbol.GetMembers())
        {
            ITypeSymbol? memberType = member switch
            {
                IPropertySymbol { IsStatic: false, IsIndexer: false } prop => prop.Type,
                IFieldSymbol { IsStatic: false, IsImplicitlyDeclared: false } field => field.Type,
                _ => null
            };

            if (memberType == null)
                continue;

            if (!IsDeepValueEquatable(memberType, visited))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if a type is inherently value-equatable (doesn't need [Equatable] for correct equality).
    /// </summary>
    private static bool IsDeepValueEquatable(ITypeSymbol typeSymbol, HashSet<ITypeSymbol> visited)
    {
        // Primitives (including enums) are value-equatable
        if (IsPrimitiveType(typeSymbol))
            return true;

        // Handle nullable types
        if (typeSymbol is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullableType)
        {
            var underlyingType = nullableType.TypeArguments[0];
            return IsDeepValueEquatable(underlyingType, visited);
        }

        // Collections are NOT value-equatable (they use reference equality by default)
        if (typeSymbol.IsCollection())
            return false;

        // Well-known System types that we consider value-equatable
        if (IsWellKnownSystemType(typeSymbol))
            return true;

        // Types with proper equality (attribute or IEquatable<T>) are value-equatable
        if (typeSymbol.HasProperEquality())
            return true;

        // Records and structs - recursively check their members
        if (typeSymbol is INamedTypeSymbol nts &&
            (nts.IsRecord || typeSymbol.TypeKind == TypeKind.Struct))
        {
            return HasDeepValueEquality(nts, visited);
        }

        // Classes without IEquatable<T> are not value-equatable (they use reference equality)
        return false;
    }

    private static bool IsPrimitiveType(ITypeSymbol typeSymbol)
    {
        // Enums are value types with built-in equality
        if (typeSymbol.TypeKind == TypeKind.Enum)
            return true;

        if (typeSymbol.SpecialType != SpecialType.None)
            return true;

        var fullName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            .Replace("global::", "");

        return PrimitiveTypeNames.Contains(fullName);
    }

    /// <summary>
    /// Checks if a type implements IEquatable&lt;T&gt; for itself.
    /// Used as a fallback for cross-assembly detection when [Equatable] attribute is not visible.
    /// </summary>
    public static bool ImplementsIEquatable(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        // Check if the type implements IEquatable<T> where T is the type itself
        foreach (var iface in namedType.AllInterfaces)
        {
            // Check if this is IEquatable<T>
            if (iface is not { Name: "IEquatable", TypeArguments.Length: 1, ContainingNamespace: { } ns })
                continue;

            // Check namespace is System
            if (ns.ToDisplayString() != "System")
                continue;

            // Check the type argument is the type itself
            if (SymbolEqualityComparer.Default.Equals(iface.TypeArguments[0], namedType))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a type has proper equality (either via [Equatable] attribute or IEquatable&lt;T&gt;).
    /// First checks for [Equatable] attribute (works for same-assembly types).
    /// For cross-assembly types (from metadata), falls back to IEquatable&lt;T&gt; check.
    /// </summary>
    public static bool HasProperEquality(this ITypeSymbol typeSymbol)
    {
        // First try to detect via attribute (works for same-assembly)
        if (typeSymbol.HasEquatableAttribute())
            return true;

        // Only fall back to IEquatable<T> check for types from other assemblies (metadata)
        // For same-assembly types, we can see the attribute, so no need for fallback
        var isFromMetadata = typeSymbol.Locations.All(l => l.Kind == LocationKind.MetadataFile);
        if (isFromMetadata && typeSymbol.ImplementsIEquatable())
            return true;

        return false;
    }

    /// <summary>
    /// Checks if a type has the [Equatable] attribute (works across assemblies).
    /// Note: Due to [Conditional] on the attribute, this may not work for types from
    /// referenced assemblies. Prefer <see cref="ImplementsIEquatable"/> for cross-assembly checks.
    /// </summary>
    public static bool HasEquatableAttribute(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol is not INamedTypeSymbol namedType)
            return false;

        foreach (var attribute in namedType.GetAttributes())
        {
            var attrClass = attribute.AttributeClass;
            if (attrClass == null)
                continue;

            // Check by name - attribute class is typically "EquatableAttribute"
            if (attrClass.Name is not ("EquatableAttribute" or "Equatable"))
                continue;

            // Check namespace - could be via ToDisplayString or by walking the namespace chain
            var ns = attrClass.ContainingNamespace;
            if (ns == null || ns.IsGlobalNamespace)
                continue;

            // Check the full namespace path
            var nsName = ns.ToDisplayString();
            if (nsName == "Generator.Equals")
                return true;

            // Also try the fully qualified format in case ToDisplayString differs
            var fullName = attrClass.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (fullName is "global::Generator.Equals.EquatableAttribute" or "global::Generator.Equals.Equatable")
                return true;
        }

        return false;
    }

    private static bool IsWellKnownSystemType(ITypeSymbol typeSymbol)
    {
        var containingNamespace = typeSymbol.ContainingNamespace?.ToDisplayString();
        if (containingNamespace == null)
            return false;

        // Allow user types in non-System namespaces
        if (!containingNamespace.StartsWith("System", StringComparison.Ordinal))
            return false;

        // System types are typically in System, System.Collections, etc.
        // But we want to flag user types that happen to be in the System namespace
        // So we check for common BCL types
        var fullName = typeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)
            .Replace("global::", "");

        // Common well-known BCL types that are not "complex" for our purposes
        return fullName.StartsWith("System.Collections.", StringComparison.Ordinal) ||
               fullName.StartsWith("System.Threading.", StringComparison.Ordinal) ||
               fullName.StartsWith("System.IO.", StringComparison.Ordinal) ||
               fullName.StartsWith("System.Net.", StringComparison.Ordinal) ||
               fullName.StartsWith("System.Text.", StringComparison.Ordinal) ||
               fullName.StartsWith("System.Tuple", StringComparison.Ordinal) ||
               fullName.StartsWith("System.ValueTuple", StringComparison.Ordinal) ||
               fullName == "System.Type" ||
               fullName == "System.Version" ||
               fullName == "System.Exception";
    }
}
