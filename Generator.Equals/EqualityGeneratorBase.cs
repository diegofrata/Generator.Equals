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

        protected static readonly string[] DiffMethodComment = @"
/// <summary>
/// Returns the differences between two instances.
/// </summary>
/// <param name=""x"">The first instance to compare.</param>
/// <param name=""y"">The second instance to compare.</param>
/// <param name=""path"">The base path for difference reporting.</param>
/// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>".ToLines();

        static string GetComparerEquals(EqualityMemberModel memberModel, string left, string right)
        {
            return memberModel.EqualityType switch
            {
                EqualityType.IgnoreEquality => null!,

                EqualityType.UnorderedEquality when memberModel.IsDictionary =>
                    $"global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.UnorderedEquality =>
                    $"{GetCollectionComparerExpression("UnorderedEqualityComparer", memberModel.TypeName, memberModel)}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.OrderedEquality =>
                    $"{GetCollectionComparerExpression("OrderedEqualityComparer", memberModel.TypeName, memberModel)}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.ReferenceEquality =>
                    $"global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.SetEquality =>
                    $"{GetCollectionComparerExpression("SetEqualityComparer", memberModel.TypeName, memberModel)}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.StringEquality =>
                    $"global::System.StringComparer.{memberModel.StringComparer}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance =>
                    $"{memberModel.ComparerType}.{memberModel.ComparerMemberName}.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.CustomEquality =>
                    $"new {memberModel.ComparerType}().Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                EqualityType.DefaultEquality =>
                    $"global::Generator.Equals.DefaultEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.PropertyName}!, {right}.{memberModel.PropertyName}!)",

                _ => null!
            };
        }

        static void BuildMemberDiff(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            if (memberModel.Ignored || memberModel.EqualityType == EqualityType.IgnoreEquality)
            {
                return;
            }

            var comparerEquals = GetComparerEquals(memberModel, left, right);
            if (comparerEquals == null) return;

            switch (memberModel.EqualityType)
            {
                case EqualityType.OrderedEquality:
                    BuildOrderedCollectionDiff(memberModel, writer, left, right, pathExpr);
                    break;

                case EqualityType.UnorderedEquality when !memberModel.IsDictionary:
                case EqualityType.SetEquality:
                    BuildUnorderedCollectionDiff(memberModel, writer, left, right, pathExpr);
                    break;

                case EqualityType.UnorderedEquality when memberModel.IsDictionary:
                    BuildDictionaryDiff(memberModel, writer, left, right, pathExpr);
                    break;

                default:
                    // Simple property diff
                    writer.WriteLine($"if (!{comparerEquals})");
                    writer.WriteLine(1, $"yield return ({pathExpr} + \"{memberModel.PropertyName}\", {left}.{memberModel.PropertyName}, {right}.{memberModel.PropertyName});");
                    break;
            }
        }

        static void BuildOrderedCollectionDiff(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            var comparerEquals = GetComparerEquals(memberModel, left, right);

            writer.WriteLine($"if (!{comparerEquals})");
            writer.AppendOpenBracket();

            writer.WriteLine($"var __xList = {left}.{memberModel.PropertyName} is null ? new global::System.Collections.Generic.List<{memberModel.TypeName}>() : new global::System.Collections.Generic.List<{memberModel.TypeName}>({left}.{memberModel.PropertyName});");
            writer.WriteLine($"var __yList = {right}.{memberModel.PropertyName} is null ? new global::System.Collections.Generic.List<{memberModel.TypeName}>() : new global::System.Collections.Generic.List<{memberModel.TypeName}>({right}.{memberModel.PropertyName});");
            writer.WriteLine("var __maxLen = global::System.Math.Max(__xList.Count, __yList.Count);");
            writer.WriteLine();
            writer.WriteLine("for (var __i = 0; __i < __maxLen; __i++)");
            writer.AppendOpenBracket();
            writer.WriteLine("var __xVal = __i < __xList.Count ? (object?)__xList[__i] : null;");
            writer.WriteLine("var __yVal = __i < __yList.Count ? (object?)__yList[__i] : null;");
            writer.WriteLine("if (!global::System.Object.Equals(__xVal, __yVal))");
            writer.WriteLine(1, $"yield return ({pathExpr} + $\"{memberModel.PropertyName}[{{__i}}]\", __xVal, __yVal);");
            writer.AppendCloseBracket();

            writer.AppendCloseBracket();
        }

        static void BuildUnorderedCollectionDiff(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            var comparerEquals = GetComparerEquals(memberModel, left, right);

            writer.WriteLine($"if (!{comparerEquals})");
            writer.AppendOpenBracket();

            writer.WriteLine($"var __xSet = {left}.{memberModel.PropertyName} is null ? new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>() : new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>({left}.{memberModel.PropertyName});");
            writer.WriteLine($"var __ySet = {right}.{memberModel.PropertyName} is null ? new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>() : new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>({right}.{memberModel.PropertyName});");
            writer.WriteLine();
            writer.WriteLine("foreach (var __removed in global::System.Linq.Enumerable.Except(__xSet, __ySet))");
            writer.WriteLine(1, $"yield return ({pathExpr} + \"{memberModel.PropertyName}[-]\", __removed, null);");
            writer.WriteLine();
            writer.WriteLine("foreach (var __added in global::System.Linq.Enumerable.Except(__ySet, __xSet))");
            writer.WriteLine(1, $"yield return ({pathExpr} + \"{memberModel.PropertyName}[+]\", null, __added);");

            writer.AppendCloseBracket();
        }

        static void BuildDictionaryDiff(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            var comparerEquals = GetComparerEquals(memberModel, left, right);

            writer.WriteLine($"if (!{comparerEquals})");
            writer.AppendOpenBracket();

            writer.WriteLine($"var __xDict = {left}.{memberModel.PropertyName};");
            writer.WriteLine($"var __yDict = {right}.{memberModel.PropertyName};");
            writer.WriteLine();
            writer.WriteLine("if (__xDict is null && __yDict is not null)");
            writer.AppendOpenBracket();
            writer.WriteLine("foreach (var __kvp in __yDict)");
            writer.WriteLine(1, $"yield return ({pathExpr} + $\"{memberModel.PropertyName}[{{__kvp.Key}}]\", null, __kvp.Value);");
            writer.AppendCloseBracket();
            writer.WriteLine("else if (__xDict is not null && __yDict is null)");
            writer.AppendOpenBracket();
            writer.WriteLine("foreach (var __kvp in __xDict)");
            writer.WriteLine(1, $"yield return ({pathExpr} + $\"{memberModel.PropertyName}[{{__kvp.Key}}]\", __kvp.Value, null);");
            writer.AppendCloseBracket();
            writer.WriteLine("else if (__xDict is not null && __yDict is not null)");
            writer.AppendOpenBracket();

            // Check for removed/changed keys from x
            writer.WriteLine("foreach (var __kvp in __xDict)");
            writer.AppendOpenBracket();
            writer.WriteLine("if (!__yDict.TryGetValue(__kvp.Key, out var __yVal))");
            writer.WriteLine(1, $"yield return ({pathExpr} + $\"{memberModel.PropertyName}[{{__kvp.Key}}]\", __kvp.Value, null);");
            writer.WriteLine("else if (!global::System.Object.Equals(__kvp.Value, __yVal))");
            writer.WriteLine(1, $"yield return ({pathExpr} + $\"{memberModel.PropertyName}[{{__kvp.Key}}]\", __kvp.Value, __yVal);");
            writer.AppendCloseBracket();

            // Check for added keys in y
            writer.WriteLine("foreach (var __kvp in __yDict)");
            writer.AppendOpenBracket();
            writer.WriteLine("if (!__xDict.ContainsKey(__kvp.Key))");
            writer.WriteLine(1, $"yield return ({pathExpr} + $\"{memberModel.PropertyName}[{{__kvp.Key}}]\", null, __kvp.Value);");
            writer.AppendCloseBracket();

            writer.AppendCloseBracket();

            writer.AppendCloseBracket();
        }

        internal static void BuildMembersDiff(
            ImmutableArray<EqualityMemberModel> models,
            IndentedTextWriter writer,
            string left = "x",
            string right = "y",
            string pathExpr = "__path"
        )
        {
            foreach (var model in models)
            {
                BuildMemberDiff(model, writer, left, right, pathExpr);
            }
        }
    }
}