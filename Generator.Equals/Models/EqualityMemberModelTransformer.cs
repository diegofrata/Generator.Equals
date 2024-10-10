using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.Models;

internal static class EqualityMemberModelTransformer
{
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
            // return new EqualityMemberModel(propertyName, typeName, EqualityType.IgnoreEquality)
            // {
            //     Ignored = true
            // };

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = typeName,
                EqualityType = EqualityType.IgnoreEquality,
                Ignored = true,
            };
        }


        if (memberSymbol.HasAttribute(attributesMetadata.StringEquality))
        {
            var attribute = memberSymbol.GetAttribute(attributesMetadata.StringEquality)!;
            var stringComparisonValue = Convert.ToInt64(attribute.ConstructorArguments[0].Value);

            if (!attributesMetadata.StringComparisonLookup.TryGetValue(stringComparisonValue, out var enumMemberName))
            {
                throw new Exception("Unexpected StringComparison value.");
            }


            // Special case: We do this comparison through either OrderedEquality or UnorderedEquality
            if (typeSymbol.IsStringArray() && typeSymbol.GetIEnumerableTypeArguments() is { } args)
            {
                var equalityType = memberSymbol.HasAttribute(attributesMetadata.UnorderedEquality)
                    ? EqualityType.UnorderedEquality
                    : EqualityType.OrderedEquality;

                // return new EqualityMemberModel(propertyName, args.Name, equalityType, stringComparer: enumMemberName);
                return new EqualityMemberModel
                {
                    PropertyName = propertyName,
                    TypeName = args.Name,
                    EqualityType = equalityType,
                    StringComparer = enumMemberName
                };
            }

            // return new EqualityMemberModel(propertyName, typeName, EqualityType.StringEquality, stringComparer: enumMemberName);
            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = typeName,
                EqualityType = EqualityType.StringEquality,
                StringComparer = enumMemberName
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.UnorderedEquality))
        {
            var args = typeSymbol.GetIDictionaryTypeArguments()
                       ?? typeSymbol.GetIEnumerableTypeArguments()!;

            // return new EqualityMemberModel(propertyName, args.Name, EqualityType.UnorderedEquality)
            // {
            //     IsDictionary = args is DictionaryArgumentsResult
            // };

            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = args.Name,
                EqualityType = EqualityType.UnorderedEquality,
                IsDictionary = args is DictionaryArgumentsResult
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.OrderedEquality))
        {
            var types = typeSymbol.GetIEnumerableTypeArguments()!;
            // return new EqualityMemberModel(propertyName, types.Name, EqualityType.OrderedEquality);
            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = types.Name,
                EqualityType = EqualityType.OrderedEquality
            };
        }
        else if (memberSymbol.HasAttribute(attributesMetadata.ReferenceEquality))
        {
            // return new EqualityMemberModel(propertyName, typeName, EqualityType.ReferenceEquality);
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
            // return new EqualityMemberModel(propertyName, types.Name, EqualityType.SetEquality);
            return new EqualityMemberModel
            {
                PropertyName = propertyName,
                TypeName = types.Name,
                EqualityType = EqualityType.SetEquality
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