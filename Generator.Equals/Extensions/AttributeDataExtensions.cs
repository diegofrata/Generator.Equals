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

    /// <summary>
    /// Whether an [Equatable] application asks the generator to emit == and != for a class.
    /// Single source of truth for the rule, shared by the generator and the analyzer so the two
    /// cannot drift apart.
    /// </summary>
    public static bool GeneratesClassEqualityOperators(this AttributeData equatableAttributeData) =>
        equatableAttributeData.GetNamedArgumentValue(nameof(EquatableAttribute.GenerateClassEqualityOperators)) is not false;
}