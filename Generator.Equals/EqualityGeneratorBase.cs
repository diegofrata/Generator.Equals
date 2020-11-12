using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class EqualityGeneratorBase
    {
        public static void BuildPropertyEquality(AttributesMetadata attributesMetadata, StringBuilder sb,
            IPropertySymbol property)
        {
            var propertyName = property.ToFQF();

            var typeName = property.Type.ToNullableFQF();

            if (property.HasAttribute(attributesMetadata.SequenceEquality))
            {
                var types = property.GetIEnumerableTypeArguments();

                sb.AppendLine(
                    $"&& global::Generator.Equals.SequenceEqualityComparer<{string.Join(", ", types)}>.Instance.Equals({propertyName}, other.{propertyName})");
            }
            else
            {
                sb.AppendLine(
                    $"&& global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals({propertyName}, other.{propertyName})");
            }
        }
        
        public static void BuildPropertyHashCode(IPropertySymbol property, AttributesMetadata attributesMetadata,
            StringBuilder sb)
        {
            var propertyName = property.ToFQF();

            var typeName = property.Type.ToNullableFQF();

            sb.Append($"hashCode.Add(this.{propertyName}, ");

            if (property.HasAttribute(attributesMetadata.SequenceEquality))
            {
                var types = property.GetIEnumerableTypeArguments();
                sb.Append(
                    $"global::Generator.Equals.SequenceEqualityComparer<{string.Join(", ", types)}>.Instance");
            }
            else
            {
                sb.Append($"global::System.Collections.Generic.EqualityComparer<{typeName}>.Default");
            }
            
            sb.AppendLine(");");
        }
    }
}