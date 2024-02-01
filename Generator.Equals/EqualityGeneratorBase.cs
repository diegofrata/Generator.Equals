using System;
using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Equals
{
    class EqualityGeneratorBase
    {
        protected const string GeneratedCodeAttributeDeclaration = "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Generator.Equals\", \"1.0.0.0\")]";

        internal const string EnableNullableContext = "#nullable enable";

        // CS0612: Obsolete with no comment
        // CS0618: obsolete with comment
        internal const string SuppressObsoleteWarningsPragma = "#pragma warning disable CS0612,CS0618";
        internal const string SuppressTypeConflictsWarningsPragma = "#pragma warning disable CS0436";

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

        static void BuildEquality(AttributesMetadata attributesMetadata, IndentedTextWriter writer,
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
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    types = typeSymbol.GetIEnumerableTypeArguments()!;
                    writer.WriteLine(
                        $"&& global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
                }
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.OrderedEquality))
            {
                var types = typeSymbol.GetIEnumerableTypeArguments()!;

                writer.WriteLine(
                    $"&& global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                writer.WriteLine(
                    $"&& global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.SetEquality))
            {
                var types = typeSymbol.GetIEnumerableTypeArguments()!;

                writer.WriteLine(
                    $"&& global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.StringEquality))
            {
                var attribute = memberSymbol.GetAttribute(attributesMetadata.StringEquality)!;

                // `System.StringComparison` is an enum, so we need to fetch the value as a string
                // since compilation unit might differ from analyzer unit.
                // We solve this by fetching the last identifier name in the syntax tree in the for the attribute
                // and assume that is the enum member name.

                var enumValueAsString = attribute.ApplicationSyntaxReference!.GetSyntax()
                    .DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Last()
                    .GetText();

                writer.WriteLine($"&& global::System.StringComparer.{enumValueAsString}.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.CustomEquality))
            {
                var attribute = memberSymbol.GetAttribute(attributesMetadata.CustomEquality);
                var comparerType = (INamedTypeSymbol) attribute?.ConstructorArguments[0].Value!;
                var comparerMemberName = (string) attribute?.ConstructorArguments[1].Value!;

                if (comparerType.GetMembers().Any(x => x.Name == comparerMemberName && x.IsStatic) || comparerType.GetPropertiesAndFields().Any(x => x.Name == comparerMemberName && x.IsStatic))
                {
                    writer.WriteLine(
                        $"&& {comparerType.ToFQF()}.{comparerMemberName}.Equals(this.{propertyName}!, other.{propertyName}!)");
                }
                else
                {
                    writer.WriteLine(
                        $"&& new {comparerType.ToFQF()}().Equals(this.{propertyName}!, other.{propertyName}!)");
                }
            }
            else if (
                memberSymbol.HasAttribute(attributesMetadata.DefaultEquality) ||
                (!explicitMode && memberSymbol is IPropertySymbol)
                )
            {
                writer.WriteLine(
                    $"&& global::Generator.Equals.DefaultEqualityComparer<{typeName}>.Default.Equals(this.{propertyName}!, other.{propertyName}!)");
            }
        }
        
        public static void BuildMembersEquality(ITypeSymbol symbol, AttributesMetadata attributesMetadata, IndentedTextWriter writer,
            bool explicitMode, Predicate<ISymbol>? filter = null)
        {
            foreach (var member in symbol.GetPropertiesAndFields())
            {
                if (filter != null && !filter(member))
                    continue;
                
                switch (member)
                {
                    case IPropertySymbol propertySymbol:
                        BuildEquality(attributesMetadata, writer, propertySymbol, propertySymbol.Type, explicitMode);
                        break;
                    case IFieldSymbol fieldSymbol:
                        BuildEquality(attributesMetadata, writer, fieldSymbol, fieldSymbol.Type, explicitMode);
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
            IndentedTextWriter writer,
            bool explicitMode)
        {
            if (memberSymbol.HasAttribute(attributesMetadata.IgnoreEquality))
                return;

            var propertyName = memberSymbol.ToFQF();

            var typeName = typeSymbol.ToNullableFQF();

            void BuildHashCodeAdd(Action action)
            {
                writer.WriteLine($"hashCode.Add(");

                writer.Indent++;
                writer.WriteLine($"this.{propertyName}!,");

                action();
                writer.Indent--;

                writer.WriteLine(");");
            }

            if (memberSymbol.HasAttribute(attributesMetadata.UnorderedEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var types = typeSymbol.GetIDictionaryTypeArguments();

                    if (types != null)
                    {
                        writer.Write(
                            $"global::Generator.Equals.DictionaryEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                    }
                    else
                    {
                        types = typeSymbol.GetIEnumerableTypeArguments()!;
                        writer.Write(
                            $"global::Generator.Equals.UnorderedEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                    }
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.OrderedEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var types = typeSymbol.GetIEnumerableTypeArguments()!;
                    writer.Write(
                        $"global::Generator.Equals.OrderedEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.ReferenceEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    writer.Write($"global::Generator.Equals.ReferenceEqualityComparer<{typeName}>.Default");
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.SetEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var types = typeSymbol.GetIEnumerableTypeArguments()!;
                    writer.Write(
                        $"global::Generator.Equals.SetEqualityComparer<{string.Join(", ", types.Value)}>.Default");
                });
            }
            else if (memberSymbol.HasAttribute(attributesMetadata.StringEquality))
            {
                BuildHashCodeAdd(() =>
                {
                    var attribute = memberSymbol.GetAttribute(attributesMetadata.StringEquality);
                    var comparerType = (StringComparison)attribute?.ConstructorArguments[0].Value!;
                    writer.Write($"global::System.StringComparer.{comparerType}");
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
                        writer.Write($"{comparerType.ToFQF()}.{comparerMemberName}");
                    }
                    else
                    {
                        writer.Write($"new {comparerType.ToFQF()}()");
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
                    writer.Write($"global::Generator.Equals.DefaultEqualityComparer<{typeName}>.Default");
                });
            }
        }
        
        public static void BuildMembersHashCode(ITypeSymbol symbol, AttributesMetadata attributesMetadata, IndentedTextWriter writer,
            bool explicitMode, Predicate<ISymbol>? filter = null)
        {
            foreach (var member in symbol.GetPropertiesAndFields())
            {
                if (filter != null && !filter(member))
                    continue;
                
                switch (member)
                {
                    case IPropertySymbol propertySymbol:
                        BuildHashCode(propertySymbol, propertySymbol.Type, attributesMetadata, writer, explicitMode);
                        break;
                    case IFieldSymbol fieldSymbol:
                        BuildHashCode(fieldSymbol, fieldSymbol.Type, attributesMetadata, writer, explicitMode);
                        break;
                    default:
                        throw new NotSupportedException($"Member of type {member.GetType()} not supported");
                }
            }
        }
    }
}