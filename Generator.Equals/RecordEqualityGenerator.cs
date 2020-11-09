using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    static class RecordEqualityGenerator
    {
        static IEnumerable<(IPropertySymbol symbol, string PropertyName)> GetRecordProperties(ITypeSymbol symbol)
        {
            foreach (var property in symbol.GetMembers().OfType<IPropertySymbol>())
            {
                var propertyName = property.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                if (propertyName == "EqualityContract")
                    continue;

                yield return (property, propertyName);
            }
        }

        static AttributeData? GetAttribute(ISymbol symbol, INamedTypeSymbol attribute)
        {
            return symbol.GetAttributes().FirstOrDefault(x =>
                x.AttributeClass?.Equals(attribute, SymbolEqualityComparer.Default) == true);
        }

        static bool HasAttribute(ISymbol symbol, INamedTypeSymbol attribute) =>
            GetAttribute(symbol, attribute) != null;
        
        static IEnumerable<string> GetIEnumerableTypeArguments(IPropertySymbol property)
        {
            // TODO: Find a better way to find the generic type of IEnumerable<T>.
            var enumerableInterface = property.Type.AllInterfaces.FirstOrDefault(x =>
                x.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) ==
                "global::System.Collections.Generic.IEnumerable<T>");

            if (enumerableInterface == null)
                throw new Exception($"Type {property.Type} does not implement IEnumerable<T>.");

            var types = enumerableInterface.TypeArguments.Select(x => x.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
                        SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
                    )
                )
            );
            return types;
        }

        static void BuildEquals(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb)
        {
            var symbolName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            var baseTypeName = symbol.BaseType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            sb.AppendLine("#nullable enable");

            sb.AppendLine(symbol.IsSealed
                ? $"public bool Equals({symbolName}? other) {{"
                : $"public virtual bool Equals({symbolName}? other) {{");

            sb.AppendLine(baseTypeName == "object"
                ? "return other is not null && EqualityContract == other.EqualityContract"
                : $"return base.Equals(({baseTypeName}?)other)");

            foreach (var (property, propertyName) in GetRecordProperties(symbol))
            {
                var typeName = property.Type.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
                        SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier)
                );

                if (HasAttribute(property, attributesMetadata.SequenceEquality))
                {
                    var types = GetIEnumerableTypeArguments(property);
                    sb.AppendLine(
                        $"&& global::Generator.Equals.SequenceEqualityComparer<{string.Join(", ", types)}>.Instance.Equals({propertyName}, other.{propertyName})");
                }
                else
                {
                    sb.AppendLine(
                        $"&& global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals({propertyName}, other.{propertyName})");
                }
            }

            sb.AppendLine(";");
            sb.AppendLine("}");

            sb.AppendLine("#nullable restore");
        }

        static void BuildGetHashCode(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb)
        {
            var baseTypeName = symbol.BaseType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

            sb.AppendLine("#nullable enable");

            sb.AppendLine(@"public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            ");

            sb.AppendLine(baseTypeName == "object"
                ? "hashCode.Add(this.EqualityContract);"
                : "hashCode.Add(base.GetHashCode());");

            foreach (var (property, propertyName) in GetRecordProperties(symbol))
            {
                var typeName = property.Type.ToDisplayString(
                    SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
                        SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier)
                );

                sb.Append($"hashCode.Add(this.{propertyName}, ");

                if (HasAttribute(property, attributesMetadata.SequenceEquality))
                {
                    var types = GetIEnumerableTypeArguments(property);
                    sb.Append($"global::Generator.Equals.SequenceEqualityComparer<{string.Join(", ", types)}>.Instance");
                }
                else
                {
                    sb.Append($"global::System.Collections.Generic.EqualityComparer<{typeName}>.Default");
                }

                sb.AppendLine(");");
            }

            sb.AppendLine("return hashCode.ToHashCode();");
            sb.AppendLine("}");

            sb.AppendLine("#nullable restore");
        }


        public static string Generate(ITypeSymbol symbol, AttributesMetadata attributesMetadata)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: sb =>
            {
                BuildEquals(symbol, attributesMetadata, sb);

                BuildGetHashCode(symbol, attributesMetadata, sb);
            });

            return code;
        }
    }
}