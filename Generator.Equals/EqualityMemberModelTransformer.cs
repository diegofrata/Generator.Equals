using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Generator.Equals.Extensions;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.Models;

record ElementComparerInfo(
    string? ComparerType,
    string? MemberName,
    bool HasStaticInstance
);

static class EqualityMemberModelTransformer
{
    static ElementComparerInfo? ExtractElementComparerInfo(
        AttributeData attribute,
        AttributesMetadata attributesMetadata
    )
    {
        var args = attribute.ConstructorArguments;

        if (args.Length == 0)
        {
            return null;
        }

        // Check for StringComparison argument (first constructor overload)
        if (args[0].Type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::System.StringComparison")
        {
            var stringComparisonValue = Convert.ToInt64(args[0].Value, CultureInfo.InvariantCulture);
            if (attributesMetadata.StringComparisonLookup.TryGetValue(stringComparisonValue, out var enumMemberName))
            {
                return new ElementComparerInfo(
                    "global::System.StringComparer",
                    enumMemberName,
                    true
                );
            }
            return null;
        }

        // Check for Type argument (second constructor overload)
        if (args[0].Value is INamedTypeSymbol comparerType)
        {
            var comparerTypeName = comparerType.ToFQF();
            var comparerMemberName = args.Length > 1 && args[1].Value is string memberName
                ? memberName
                : "Default";

            var hasStaticInstance = comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic)
                                    || comparerType.GetPropertiesAndFields().Any(x => x.Name == comparerMemberName && x.IsStatic);

            return new ElementComparerInfo(
                comparerTypeName,
                comparerMemberName,
                hasStaticInstance
            );
        }

        return null;
    }

    public static ImmutableArray<EqualityMemberModel> BuildEqualityModels(
        ITypeSymbol symbol,
        AttributesMetadata attributesMetadata,
        bool explicitMode,
        Predicate<ISymbol>? filter = null
    )
    {
        var isRecord = symbol.IsRecord;

        var members = symbol.GetPropertiesAndFields();
        var models = members
            .Where(member => filter == null || filter(member))

            // ignore equalitycontract if the type is a record
            .Where(member => !isRecord || member.Name != "EqualityContract")
            .Select(member => member switch
            {
                IPropertySymbol propertySymbol
                    => BuildEqualityModel(propertySymbol, propertySymbol.Type, attributesMetadata, explicitMode),
                IFieldSymbol fieldSymbol => BuildEqualityModel(fieldSymbol, fieldSymbol.Type, attributesMetadata, explicitMode),
                _ => throw new NotSupportedException($"Member of type {member.GetType()} not supported")
            })
            .ToImmutableArray();

        return models;
    }

    public static EqualityMemberModel BuildEqualityModel(
        ISymbol memberSymbol,
        ITypeSymbol typeSymbol,
        AttributesMetadata attributesMetadata,
        bool explicitMode
    )
    {
        var propertyName = memberSymbol.ToFQF();
        var typeName = typeSymbol.ToNullableFQF();

        // IgnoreEquality
        if (memberSymbol.HasAttribute(attributesMetadata.IgnoreEquality))
        {
            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = typeName,
                EqualityType = EqualityType.IgnoreEquality,
                Ignored = true,
            };
        }

        // Check for different equality attributes and map them to the model
        if (memberSymbol.HasAttribute(attributesMetadata.UnorderedEquality))
        {
            var args = typeSymbol.GetIDictionaryTypeArguments()
                       ?? typeSymbol.GetIEnumerableTypeArguments()!;

            var attribute = memberSymbol.GetAttribute(attributesMetadata.UnorderedEquality)!;
            var elementComparer = ExtractElementComparerInfo(attribute, attributesMetadata);

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = args.Name,
                EqualityType = EqualityType.UnorderedEquality,
                IsDictionary = args is DictionaryArgumentsResult,
                ElementComparerType = elementComparer?.ComparerType,
                ElementComparerMemberName = elementComparer?.MemberName,
                ElementComparerHasStaticInstance = elementComparer?.HasStaticInstance ?? false
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.OrderedEquality))
        {
            var types = typeSymbol.GetIEnumerableTypeArguments()!;

            var attribute = memberSymbol.GetAttribute(attributesMetadata.OrderedEquality)!;
            var elementComparer = ExtractElementComparerInfo(attribute, attributesMetadata);

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = types.Name,
                EqualityType = EqualityType.OrderedEquality,
                ElementComparerType = elementComparer?.ComparerType,
                ElementComparerMemberName = elementComparer?.MemberName,
                ElementComparerHasStaticInstance = elementComparer?.HasStaticInstance ?? false
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.ReferenceEquality))
        {
            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = typeName,
                EqualityType = EqualityType.ReferenceEquality
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.SetEquality))
        {
            var types = typeSymbol.GetIEnumerableTypeArguments()!;

            var attribute = memberSymbol.GetAttribute(attributesMetadata.SetEquality)!;
            var elementComparer = ExtractElementComparerInfo(attribute, attributesMetadata);

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = types.Name,
                EqualityType = EqualityType.SetEquality,
                ElementComparerType = elementComparer?.ComparerType,
                ElementComparerMemberName = elementComparer?.MemberName,
                ElementComparerHasStaticInstance = elementComparer?.HasStaticInstance ?? false
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.StringEquality))
        {
            var attribute = memberSymbol.GetAttribute(attributesMetadata.StringEquality)!;
            var stringComparisonValue = Convert.ToInt64(attribute.ConstructorArguments[0].Value, CultureInfo.InvariantCulture);

            if (!attributesMetadata.StringComparisonLookup.TryGetValue(stringComparisonValue, out var enumMemberName))
            {
                throw new InvalidOperationException("Unexpected StringComparison value.");
            }

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = typeName,
                EqualityType = EqualityType.StringEquality,
                StringComparer = enumMemberName
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.CustomEquality))
        {
            var attribute = memberSymbol.GetAttribute(attributesMetadata.CustomEquality);
            var comparerType = ((INamedTypeSymbol)attribute?.ConstructorArguments[0].Value!);

            var comparerTypeName = comparerType.ToFQF();
            var comparerMemberName = (string)attribute?.ConstructorArguments[1].Value!;

            var hasDefault = comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic)
                             || comparerType.GetPropertiesAndFields().Any(x => x.Name == comparerMemberName && x.IsStatic);

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = typeName,
                EqualityType = EqualityType.CustomEquality,
                ComparerType = comparerTypeName,
                ComparerMemberName = comparerMemberName,
                ComparerHasStaticInstance = hasDefault
            };
        }

        var isIgnored = (explicitMode && !memberSymbol.HasAttribute(attributesMetadata.DefaultEquality));

        return new EqualityMemberModel
        {
            PropertyName = propertyName,
            TypeName = typeName,
            EqualityType = isIgnored ? EqualityType.IgnoreEquality : EqualityType.DefaultEquality,
            Ignored = isIgnored
        };
    }
}