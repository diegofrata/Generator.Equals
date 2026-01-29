using System;

using Microsoft.CodeAnalysis;

namespace Generator.Equals.Extensions;

static class AttributeDataExtensions
{
    public static object? GetNamedArgumentValue(this AttributeData attributeData, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }
        
        if (attributeData == null)
        {
            throw new ArgumentNullException(nameof(attributeData));
        }

        foreach (var pair in attributeData.NamedArguments)
        {
            if (pair.Key == name)
            {
                return pair.Value.Value;
            }
        }

        return null;
    }
}