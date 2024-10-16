using System;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace Generator.Equals.Models;

internal sealed record AttributeMetadata(string FullName, string Namespace, string LongName, string ShortName)
{
    // Returns an AttributeMetadata instance for a full name
    public static AttributeMetadata FromFullName(string fullName)
    {
        var parts = fullName.Split('.');

        var name = parts.Last();

        var shortName = name.EndsWith("Attribute", StringComparison.Ordinal) ? name[..^"Attribute".Length] : name;
        var longName = name.EndsWith("Attribute", StringComparison.Ordinal) ? name : name + "Attribute";

        var metadata = new AttributeMetadata
        (
            FullName: fullName,
            Namespace: parts.Length == 1 ? string.Empty : string.Join(".", parts.Take(parts.Length - 1)),
            LongName: longName,
            ShortName: shortName
        );

        return metadata;
    }

    public bool Equals(INamedTypeSymbol? symbol)
    {
        if (symbol is null)
        {
            return false;
        }

        var nameEquals = string.Equals(symbol.Name, LongName, StringComparison.Ordinal)
                         || string.Equals(symbol.Name, ShortName, StringComparison.Ordinal);

        var containingNamespace = symbol.ContainingNamespace.ToDisplayString();
        var namespaceEquals = string.Equals(containingNamespace, Namespace, StringComparison.Ordinal);

        return nameEquals && namespaceEquals;
    }
}