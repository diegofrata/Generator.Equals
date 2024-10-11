using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;

namespace Generator.Equals.Models;

internal static class AttributeMetadataExtensions
{
    [SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator", Justification = "Performance")]
    public static bool HasAttribute(this ISymbol symbol, AttributeMetadata metadata)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (metadata.Equals(attribute.AttributeClass))
            {
                return true;
            }
        }

        return false;
    }

    //GetAttribute
    public static AttributeData? GetAttribute(this ISymbol symbol, AttributeMetadata metadata)
    {
        foreach (var attribute in symbol.GetAttributes())
        {
            if (metadata.Equals(attribute.AttributeClass))
            {
                return attribute;
            }
        }

        return null;
    }
}