using System;
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

        static void BuildEquality(AttributesMetadata attributesMetadata, StringBuilder sb, int level,
            ISymbol memberSymbol, ITypeSymbol typeSymbol, bool explicitMode)
        {
            if (memberSymbol.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            var propertyName = memberSymbol.ToFQF();

            var typeName = typeSymbol.ToNullableFQF();

            if (memberSymbol.HasAttribute(attributesMetadata.UnorderedEquality))
            {
                var types = typeSymbol.GetIDictionaryTypeArguments();

                if (types != null)
                {
                    sb.AppendLine(level,
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    types = typeSymbol.GetIEnumerableTypeArguments()!;
                    sb.AppendLine(level,
                        $"&& global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
                }
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.OrderedEquality))
            {
                var types = typeSymbol.GetIEnumerableTypeArguments()!;

                sb.AppendLine(level,
                    $"&& global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                sb.AppendLine(level,
                    $"&& global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.SetEquality))
            {
                var types = typeSymbol.GetIEnumerableTypeArguments()!;

                sb.AppendLine(level,
                    $"&& global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.CustomEquality))
            {
                var attribute = memberSymbol.GetAttribute(attributesMetadata.CustomEquality);
                var comparerType = (INamedTypeSymbol) attribute?.ConstructorArguments[0].Value!;
                var comparerMemberName = (string) attribute?.ConstructorArguments[1].Value!;

                if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType.GetPropertiesAndFields().Any(x => x.Name == comparerMemberName && x.IsStatic))
                {
                    sb.AppendLine(level,
                        $"&& {comparerType.ToFQF()}.{comparerMemberName}.Equals(this.{propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    sb.AppendLine(level,
                        $"&& new {comparerType.ToFQF()}().Equals(this.{propertyName}!, other.{propertyName}!)");
                }
            }
            else if (
                memberSymbol.HasAttribute(attributesMetadata.DefaultEquality) ||
                (!explicitMode && memberSymbol is IPropertySymbol)
                )
            {
                sb.AppendLine(level,
                    $"&& global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
        }
        
        public static void BuildMembersEquality(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb,
            int level, bool explicitMode, Predicate<ISymbol>? filter = null)
        {
            foreach (var member in symbol.GetPropertiesAndFields())
            {
                if (filter != null && !filter(member))
                    continue;
                
                switch (member)
                {
                    case IPropertySymbol propertySymbol:
                        BuildEquality(attributesMetadata, sb, level, propertySymbol, propertySymbol.Type, explicitMode);
                        break;
                    case IFieldSymbol fieldSymbol:
                        BuildEquality(attributesMetadata, sb, level, fieldSymbol, fieldSymbol.Type, explicitMode);
                        break;
                    default:
                        throw new NotSupportedException($"Member of type {member.GetType()} not supported");
                }
            }
        }
        static void BuildHashCode(
            ISymbol memberSymbol,
            ITypeSymbol typeSymbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            int level,
            bool explicitMode)
        {
            if (memberSymbol.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            var propertyName = memberSymbol.ToFQF();

            var typeName = typeSymbol.ToNullableFQF();

            void BuildHashCodeAdd(Action action)
            {
                sb.AppendLine(level, $"hashCode.Add(");
                level++;

                sb.AppendLine(level, $"this.{propertyName}!,");
                sb.AppendMargin(level);

                action();

                sb.AppendLine(");");
            }

            if (memberSymbol.HasAttribute(attributesMetadata.UnorderedEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var types = typeSymbol.GetIDictionaryTypeArguments();

                    if (types != null)
                    {
                        sb.Append(
                            $"global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                    }
                    else
                    {
                        types = typeSymbol.GetIEnumerableTypeArguments()!;
                        sb.Append(
                            $"global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                    }
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.OrderedEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var types = typeSymbol.GetIEnumerableTypeArguments()!;
                    sb.Append(
                        $"global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    sb.Append($"global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default");
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.SetEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var types = typeSymbol.GetIEnumerableTypeArguments()!;
                    sb.Append(
                        $"global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.CustomEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var attribute = memberSymbol.GetAttribute(attributesMetadata.CustomEquality);
                    var comparerType = (INamedTypeSymbol)attribute?.ConstructorArguments[0].Value!;
                    var comparerMemberName = (string)attribute?.ConstructorArguments[1].Value!;

                    if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType
                            .GetPropertiesAndFields().Any(x => x.Name == comparerMemberName && x.IsStatic))
                    {
                        sb.Append($"{comparerType.ToFQF()}.{comparerMemberName}");
                    }
                    else
                    {
                        sb.Append($"new {comparerType.ToFQF()}()");
                    }
                });
            }
            else if (
                memberSymbol.HasAttribute(attributesMetadata.DefaultEquality) ||
                (!explicitMode && memberSymbol is IPropertySymbol)
            )
            {
                BuildHashCodeAdd(() =>
                {
                    sb.Append($"global::System.Collections.Generic.EqualityComparer<{typeName}>.Default");
                });
            }
        }
        
        public static void BuildMembersHashCode(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb,
            int level, bool explicitMode, Predicate<ISymbol>? filter = null)
        {
            foreach (var member in symbol.GetPropertiesAndFields())
            {
                if (filter != null && !filter(member))
                    continue;
                
                switch (member)
                {
                    case IPropertySymbol propertySymbol:
                        BuildHashCode(propertySymbol, propertySymbol.Type, attributesMetadata, sb, level, explicitMode);
                        break;
                    case IFieldSymbol fieldSymbol:
                        BuildHashCode(fieldSymbol, fieldSymbol.Type, attributesMetadata, sb, level, explicitMode);
                        break;
                    default:
                        throw new NotSupportedException($"Member of type {member.GetType()} not supported");
                }
            }
        }
    }
}