using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class EqualityGeneratorBase
    {
        protected const string GeneratedCodeAttributeDeclaration = "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Generator.Equals\", \"1.0.0.0\")]";

        internal const string EnableNullableContext = "#nullable enable";

        // CS0612: Obsolete with no comment
        // CS0618: obsolete with comment
        internal const string SuppressObsoleteWarningsPragma = "#pragma warning disable CS0612,CS0618";

        protected static readonly string[] EqualsOperatorCodeComment = @"
/// <summary>
/// Indicates whether the object on the left is equal to the object on the right.
/// </summary>
/// <param name=""left"">The left object</param>
/// <param name=""right"">The right object</param>
/// <returns>true if the objects are equal; otherwise, false.</returns>".ToLines();

        protected static readonly string[] NotEqualsOperatorCodeComment = @"
/// <summary>
/// Indicates whether the object on the left is not equal to the object on the right.
/// </summary>
/// <param name=""left"">The left object</param>
/// <param name=""right"">The right object</param>
/// <returns>true if the objects are not equal; otherwise, false.</returns>".ToLines();

        protected const string InheritDocComment = "/// <inheritdoc/>";

        public static void BuildPropertyEquality(AttributesMetadata attributesMetadata, StringBuilder sb, int level,
            IPropertySymbol property, bool explicitMode)
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
                    sb.AppendLine(level,
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    types = property.GetIEnumerableTypeArguments()!;
                    sb.AppendLine(level,
                        $"&& global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
                }
            }
            else if (property.HasAttribute(attributesMetadata.OrderedEquality))
            {
                var types = property.GetIEnumerableTypeArguments()!;

                sb.AppendLine(level,
                    $"&& global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                sb.AppendLine(level,
                    $"&& global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.SetEquality))
            {
                var types = property.GetIEnumerableTypeArguments()!;

                sb.AppendLine(level,
                    $"&& global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
            else if (property.HasAttribute(attributesMetadata.CustomEquality))
            {
                var attribute = property.GetAttribute(attributesMetadata.CustomEquality);
                var comparerType = (INamedTypeSymbol) attribute?.ConstructorArguments[0].Value!;
                var comparerMemberName = (string) attribute?.ConstructorArguments[1].Value!;

                if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType.GetProperties().Any(x => x.Name == comparerMemberName && x.IsStatic))
                {
                    sb.AppendLine(level,
                        $"&& {comparerType.ToFQF()}.{comparerMemberName}.Equals({propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    sb.AppendLine(level,
                        $"&& new {comparerType.ToFQF()}().Equals({propertyName}!, other.{propertyName}!)");
                }
            }
            else if (
                !explicitMode ||
                property.HasAttribute(attributesMetadata.DefaultEquality))
            {
                sb.AppendLine(level,
                    $"&& global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals({propertyName}!, other.{propertyName}!)");
            }
        }

        public static void BuildPropertyHashCode(
            IPropertySymbol property,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            int level,
            bool explicitMode)
        {
            if (property.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            if (explicitMode &&
                !property.HasAttribute(attributesMetadata.DefaultEquality) &&
                !property.HasAttribute(attributesMetadata.UnorderedEquality) &&
                !property.HasAttribute(attributesMetadata.OrderedEquality) &&
                !property.HasAttribute(attributesMetadata.ReferenceEquality) &&
                !property.HasAttribute(attributesMetadata.SetEquality) &&
                !property.HasAttribute(attributesMetadata.CustomEquality))
                return;

            var propertyName = property.ToFQF();

            var typeName = property.Type.ToNullableFQF();

            sb.AppendLine(level, $"hashCode.Add(");
            level++;
            sb.AppendLine(level, $"this.{propertyName}!,");
            sb.AppendMargin(level);

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
            else if (property.HasAttribute(attributesMetadata.CustomEquality))
            {
                var attribute = property.GetAttribute(attributesMetadata.CustomEquality);
                var comparerType = (INamedTypeSymbol) attribute?.ConstructorArguments[0].Value!;
                var comparerMemberName = (string) attribute?.ConstructorArguments[1].Value!;

                if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType.GetProperties().Any(x => x.Name == comparerMemberName && x.IsStatic))
                {
                    sb.Append($"{comparerType.ToFQF()}.{comparerMemberName}");
                }
                else
                {
                    sb.Append($"new {comparerType.ToFQF()}()");
                }
            }
            else
            {
                sb.Append($"global::System.Collections.Generic.EqualityComparer<{typeName}>.Default");
            }

            sb.AppendLine(");");
        }
    }
}
