using System;
using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Linq;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class EqualityGeneratorBase
    {
        protected const string GeneratedCodeAttributeDeclaration =
            "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Generator.Equals\", \"1.0.0.0\")]";

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

        private static void BuildEquality(EqualityMemberModel memberModel, IndentedTextWriter writer)
        {
            if (memberModel.Ignored)
            {
                return;
            }

            switch (memberModel.EqualityType)
            {
                case EqualityType.IgnoreEquality:
                    break;

                case EqualityType.UnorderedEquality when !memberModel.IsDictionary:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.UnorderedEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.UnorderedEquality when memberModel.IsDictionary:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.OrderedEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.OrderedEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.ReferenceEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.SetEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.SetEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.StringEquality:
                    writer.WriteLine(
                        $"&& global::System.StringComparer.{memberModel.StringComparer}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance:
                    writer.WriteLine(
                        $"&& {memberModel.ComparerType}.{memberModel.ComparerMemberName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");

                    break;

                case EqualityType.CustomEquality when !memberModel.ComparerHasStaticInstance:
                    writer.WriteLine(
                        $"&& new {memberModel.ComparerType}().Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");

                    break;

                case EqualityType.DefaultEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DefaultEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                    break;
            }
        }


        internal static void BuildMembersEquality(
            ImmutableArray<EqualityMemberModel> models,
            IndentedTextWriter writer
        )
        {
            foreach (var model in models)
            {
                BuildEquality(model, writer);
            }
        }

        private static void BuildHashCode(
            ISymbol memberSymbol,
            ITypeSymbol typeSymbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode
        )
        {
            var model = EqualityMemberModelTransformer
                .BuildEqualityModel(memberSymbol, typeSymbol, attributesMetadata, explicitMode);

            BuildHashCode(model, writer);
        }

        private static void BuildHashCode(EqualityMemberModel memberModel, IndentedTextWriter writer)
        {
            if (memberModel.Ignored)
            {
                return;
            }

            switch (memberModel.EqualityType)
            {
                case EqualityType.IgnoreEquality:
                    break;

                case EqualityType.UnorderedEquality when memberModel.IsDictionary:
                    BuildHashCodeAdd($"global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default");
                    break;

                case EqualityType.UnorderedEquality when !memberModel.IsDictionary:
                    BuildHashCodeAdd($"global::Generator.Equals.UnorderedEqualityComparer<{memberModel.TypeName}>.Default");
                    break;

                case EqualityType.OrderedEquality:
                    BuildHashCodeAdd($"global::Generator.Equals.OrderedEqualityComparer<{memberModel.TypeName}>.Default");
                    break;

                case EqualityType.ReferenceEquality:
                    BuildHashCodeAdd($"global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default");
                    break;

                case EqualityType.SetEquality:
                    BuildHashCodeAdd($"global::Generator.Equals.SetEqualityComparer<{memberModel.TypeName}>.Default");
                    break;

                case EqualityType.StringEquality:
                    BuildHashCodeAdd($"global::System.StringComparer.{memberModel.StringComparer}");
                    break;

                case EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance:
                    BuildHashCodeAdd($"{memberModel.ComparerType}.{memberModel.ComparerMemberName}");
                    break;

                case EqualityType.CustomEquality when !memberModel.ComparerHasStaticInstance:
                    BuildHashCodeAdd($"new {memberModel.ComparerType}()");
                    break;

                case EqualityType.DefaultEquality:
                    BuildHashCodeAdd($"global::Generator.Equals.DefaultEqualityComparer<{memberModel.TypeName}>.Default");
                    break;
            }

            void BuildHashCodeAdd(string comparer)
            {
                writer.WriteLine("hashCode.Add(");
                writer.Indent++;
                writer.WriteLine($"this.{memberModel.PropertyName}!,");
                writer.WriteLine(comparer);
                writer.Indent--;
                writer.WriteLine(");");
            }
        }

        public static void BuildMembersHashCode(ImmutableArray<EqualityMemberModel> models, IndentedTextWriter writer)
        {
            foreach (var model in models)
            {
                BuildHashCode(model, writer);
            }
        }
    }
}