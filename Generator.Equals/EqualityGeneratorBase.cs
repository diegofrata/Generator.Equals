using System.CodeDom.Compiler;
using System.Collections.Immutable;

using Generator.Equals.Models;

using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class EqualityGeneratorBase
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

        static string GetCollectionComparerExpression(
            string comparerClassName,
            string elementTypeName,
            EqualityMemberModel memberModel)
        {
            // Check if element comparer is specified
            if (memberModel.ElementComparerType != null)
            {
                string elementComparerExpr;
                if (memberModel.ElementComparerHasStaticInstance)
                {
                    elementComparerExpr = $"{memberModel.ElementComparerType}.{memberModel.ElementComparerMemberName}";
                }
                else
                {
                    elementComparerExpr = $"new {memberModel.ElementComparerType}()";
                }

                return $"new global::Generator.Equals.{comparerClassName}<{elementTypeName}>({elementComparerExpr})";
            }

            // No element comparer, use default
            return $"global::Generator.Equals.{comparerClassName}<{elementTypeName}>.Default";
        }

        static void BuildEquality(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right)
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
                {
                    var comparer = GetCollectionComparerExpression("UnorderedEqualityComparer", memberModel.TypeName, memberModel);
                    writer.WriteLine(
                        $"&& {comparer}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;
                }

                case EqualityType.UnorderedEquality when memberModel.IsDictionary:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.OrderedEquality:
                {
                    var comparer = GetCollectionComparerExpression("OrderedEqualityComparer", memberModel.TypeName, memberModel);
                    writer.WriteLine(
                        $"&& {comparer}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;
                }

                case EqualityType.ReferenceEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.SetEquality:
                {
                    var comparer = GetCollectionComparerExpression("SetEqualityComparer", memberModel.TypeName, memberModel);
                    writer.WriteLine(
                        $"&& {comparer}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;
                }

                case EqualityType.StringEquality:
                    writer.WriteLine(
                        $"&& global::System.StringComparer.{memberModel.StringComparer}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;

                case EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance:
                    writer.WriteLine(
                        $"&& {memberModel.ComparerType}.{memberModel.ComparerMemberName}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");

                    break;

                case EqualityType.CustomEquality when !memberModel.ComparerHasStaticInstance:
                    writer.WriteLine(
                        $"&& new {memberModel.ComparerType}().Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");

                    break;

                case EqualityType.DefaultEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DefaultEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)");
                    break;
            }
        }

        internal static void BuildMembersEquality(
            ImmutableArray<EqualityMemberModel> models,
            IndentedTextWriter writer,
            string left = "x",
            string right = "y"
        )
        {
            foreach (var model in models)
            {
                BuildEquality(model, writer, left, right);
            }
        }

        static void BuildHashCode(
            ISymbol memberSymbol,
            ITypeSymbol typeSymbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode,
            string obj = "obj"
        )
        {
            var model = EqualityMemberModelTransformer
                .BuildEqualityModel(memberSymbol, typeSymbol, attributesMetadata, explicitMode);

            BuildHashCode(model, writer, obj);
        }

        static void BuildHashCode(EqualityMemberModel memberModel, IndentedTextWriter writer, string obj)
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
                    BuildHashCodeAdd(GetCollectionComparerExpression("UnorderedEqualityComparer", memberModel.TypeName, memberModel));
                    break;

                case EqualityType.OrderedEquality:
                    BuildHashCodeAdd(GetCollectionComparerExpression("OrderedEqualityComparer", memberModel.TypeName, memberModel));
                    break;

                case EqualityType.ReferenceEquality:
                    BuildHashCodeAdd($"global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default");
                    break;

                case EqualityType.SetEquality:
                    BuildHashCodeAdd(GetCollectionComparerExpression("SetEqualityComparer", memberModel.TypeName, memberModel));
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
                writer.WriteLine($"{obj}.{memberModel.PropertyName}!,");
                writer.WriteLine(comparer);
                writer.Indent--;
                writer.WriteLine(");");
            }
        }

        public static void BuildMembersHashCode(ImmutableArray<EqualityMemberModel> models, IndentedTextWriter writer, string obj = "obj")
        {
            foreach (var model in models)
            {
                BuildHashCode(model, writer, obj);
            }
        }
    }
}