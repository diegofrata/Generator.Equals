using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class EqualityGeneratorBase
    {
        protected const string GeneratedCodeAttributeDeclaration = "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Generator.Equals\", \"1.0.0.0\")]";

        public static void BuildPropertyEquality(AttributesMetadata attributesMetadata, StringBuilder sb,
            IPropertySymbol property)
        {
            if (property.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            var propertyName = property.ToFQF();

            var typeName = property.Type.ToNullableFQF();

            if (property.HasAttribute(attributesMetadata.UnorderedEquality))
            {
                var types = property.GetIDictionaryTypeArguments();

                if (types != null)
                {
                    sb.AppendLine(
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    types = property.GetIEnumerableTypeArguments()!;
                    sb.AppendLine(
                        $"&& global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
                }
            }
            else if (property.HasAttribute(attributesMetadata.OrderedEquality))
            {
                var types = property.GetIEnumerableTypeArguments()!;

                sb.AppendLine(
                    $"&& global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                sb.AppendLine(
                    $"&& global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.SetEquality))
            {
                var types = property.GetIEnumerableTypeArguments()!;

                sb.AppendLine(
                    $"&& global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else
            {
                sb.AppendLine(
                    $"&& global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
        }

        public static void BuildPropertyHashCode(IPropertySymbol property, AttributesMetadata attributesMetadata,
            StringBuilder sb)
        {
            if (property.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            var propertyName = property.ToFQF();

            var typeName = property.Type.ToNullableFQF();

            sb.Append($"hashCode.Add(this.{propertyName}!, ");

            if (property.HasAttribute(attributesMetadata.UnorderedEquality))
            {
                var types = property.GetIDictionaryTypeArguments();

                if (types != null)
                {
                    sb.Append(
                        $"global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                }
                else
                {
                    types = property.GetIEnumerableTypeArguments()!;
                    sb.Append(
                        $"global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                }
            }
            else if (property.HasAttribute(attributesMetadata.OrderedEquality))
            {
                var types = property.GetIEnumerableTypeArguments()!;
                sb.Append(
                    $"global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default");
            }
            else if (property.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                sb.Append($"global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default");
            }
            else if (property.HasAttribute(attributesMetadata.SetEquality))
            {
                var types = property.GetIEnumerableTypeArguments()!;
                sb.Append(
                    $"global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default");
            }
            else
            {
                sb.Append($"global::System.Collections.Generic.EqualityComparer<{typeName}>.Default");
            }

            sb.AppendLine(");");
        }
    }
}
