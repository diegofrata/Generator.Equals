using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class EqualityGeneratorBase
    {
        public static void BuildPropertyEquality(AttributesMetadata attributesMetadata, StringBuilder sb,
            IPropertySymbol property)
        {
            if (property.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            var propertyName = property.ToFQF();

            var typeName = property.Type.ToNullableFQF();

            if (property.HasAttribute(attributesMetadata.DictionaryEquality))
            {
                var types = property.GetIDictionaryTypeArguments();

                sb.AppendLine(
                    $"&& global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.SequenceEquality))
            {
                var types = property.GetIEnumerableTypeArguments();

                sb.AppendLine(
                    $"&& global::Generator.Equals.SequenceEqualityComparer<{string.Join(", ", types)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.UnorderedSequenceEquality))
            {
                var types = property.GetIEnumerableTypeArguments();

                sb.AppendLine(
                    $"&& global::Generator.Equals.UnorderedSequenceEqualityComparer<{string.Join(", ", types)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
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

            if (property.HasAttribute(attributesMetadata.DictionaryEquality))
            {
                var types = property.GetIDictionaryTypeArguments();
                sb.Append(
                    $"global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types)}>.Default");
            }
            else if (property.HasAttribute(attributesMetadata.SequenceEquality))
            {
                var types = property.GetIEnumerableTypeArguments();
                sb.Append(
                    $"global::Generator.Equals.SequenceEqualityComparer<{string.Join(", ", types)}>.Default");
            }
            else if (property.HasAttribute(attributesMetadata.UnorderedSequenceEquality))
            {
                var types = property.GetIEnumerableTypeArguments();
                sb.Append(
                    $"global::Generator.Equals.UnorderedSequenceEqualityComparer<{string.Join(", ", types)}>.Default");
            }
            else
            {
                sb.Append($"global::System.Collections.Generic.EqualityComparer<{typeName}>.Default");
            }

            sb.AppendLine(");");
        }
    }
}
