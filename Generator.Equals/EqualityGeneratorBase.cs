using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class EqualityGeneratorBase
    {
        protected const string GeneratedCodeAttributeDeclaration = "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Generator.Equals\", \"1.0.0.0\")]";

        // Obsolete with no comment + obsolete with comment
        protected const string SuppressObsoleteWarningsPragma = "#pragma warning disable CS0612,CS0618";
        protected const string RestoreObsoleteWarningsPragma = "#pragma warning restore CS0612,CS0618";

        protected const string EqualsOperatorCodeComment = @"/// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name=""left"">The left object</param>
        /// <param name=""right"">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>";

        protected const string NotEqualsOperatorCodeComment = @"/// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name=""left"">The left object</param>
        /// <param name=""right"">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>";

        protected const string InheritDocComment = "/// <inheritdoc/>";

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
            else if (property.HasAttribute(attributesMetadata.CustomEquality))
            {
                var attribute = property.GetAttribute(attributesMetadata.CustomEquality);
                var comparerType = (INamedTypeSymbol) attribute?.ConstructorArguments[0].Value!;
                var comparerMemberName = (string) attribute?.ConstructorArguments[1].Value!;

                if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType.GetProperties().Any(x => x.Name == comparerMemberName && x.IsStatic))
                {
                    sb.AppendLine(
                        $"&& {comparerType.ToFQF()}.{comparerMemberName}.Equals({propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    sb.AppendLine(
                        $"&& new {comparerType.ToFQF()}().Equals({propertyName}!, other.{propertyName}!)");
                }
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
            else if (property.HasAttribute(attributesMetadata.CustomEquality))
            {
                var attribute = property.GetAttribute(attributesMetadata.CustomEquality);
                var comparerType = (INamedTypeSymbol) attribute?.ConstructorArguments[0].Value!;
                var comparerMemberName = (string) attribute?.ConstructorArguments[1].Value!;

                if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType.GetProperties().Any(x => x.Name == comparerMemberName && x.IsStatic))
                {
                    sb.AppendLine($"{comparerType.ToFQF()}.{comparerMemberName}");
                }
                else
                {
                    sb.AppendLine($"new {comparerType.ToFQF()}()");
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
