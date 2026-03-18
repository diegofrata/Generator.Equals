using System.CodeDom.Compiler;
using System.Collections.Immutable;
using System.Globalization;

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

        static string FormatPrecisionLiteral(double precision, string typeName)
        {
            var formatted = precision.ToString("R", CultureInfo.InvariantCulture);

            return typeName switch
            {
                "global::System.Single" => formatted + "f",
                "global::System.Decimal" => formatted + "m",
                _ => formatted
            };
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
                        $"&& {comparer}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
                    break;
                }

                case EqualityType.UnorderedEquality when memberModel.IsDictionary:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
                    break;

                case EqualityType.OrderedEquality:
                {
                    var comparer = GetCollectionComparerExpression("OrderedEqualityComparer", memberModel.TypeName, memberModel);
                    writer.WriteLine(
                        $"&& {comparer}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
                    break;
                }

                case EqualityType.ReferenceEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
                    break;

                case EqualityType.SetEquality:
                {
                    var comparer = GetCollectionComparerExpression("SetEqualityComparer", memberModel.TypeName, memberModel);
                    writer.WriteLine(
                        $"&& {comparer}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
                    break;
                }

                case EqualityType.StringEquality:
                    writer.WriteLine(
                        $"&& global::System.StringComparer.{memberModel.StringComparer}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
                    break;

                case EqualityType.PrecisionEquality when memberModel.IsNullable:
                {
                    var literal = FormatPrecisionLiteral(memberModel.Precision!.Value, memberModel.TypeName);
                    writer.WriteLine(
                        $"&& ({left}.{memberModel.MemberName} == {right}.{memberModel.MemberName} || ({left}.{memberModel.MemberName}.HasValue && {right}.{memberModel.MemberName}.HasValue && global::System.Math.Abs({left}.{memberModel.MemberName}.Value - {right}.{memberModel.MemberName}.Value) < {literal}))");
                    break;
                }

                case EqualityType.PrecisionEquality:
                {
                    var literal = FormatPrecisionLiteral(memberModel.Precision!.Value, memberModel.TypeName);
                    writer.WriteLine(
                        $"&& global::System.Math.Abs({left}.{memberModel.MemberName} - {right}.{memberModel.MemberName}) < {literal}");
                    break;
                }

                case EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance:
                    writer.WriteLine(
                        $"&& {memberModel.ComparerType}.{memberModel.ComparerMemberName}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");

                    break;

                case EqualityType.CustomEquality when !memberModel.ComparerHasStaticInstance:
                    writer.WriteLine(
                        $"&& new {memberModel.ComparerType}().Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");

                    break;

                case EqualityType.DefaultEquality:
                    writer.WriteLine(
                        $"&& global::Generator.Equals.DefaultEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)");
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

                case EqualityType.PrecisionEquality:
                    break; // excluded from hash — no stable bucketing under tolerance

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
                writer.WriteLine($"{obj}.{memberModel.MemberName}!,");
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

        protected static readonly string[] InequalitiesMethodComment = @"
/// <summary>
/// Returns the inequalities between two instances.
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
                    $"global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.UnorderedEquality =>
                    $"{GetCollectionComparerExpression("UnorderedEqualityComparer", memberModel.TypeName, memberModel)}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.OrderedEquality =>
                    $"{GetCollectionComparerExpression("OrderedEqualityComparer", memberModel.TypeName, memberModel)}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.ReferenceEquality =>
                    $"global::Generator.Equals.ReferenceEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.SetEquality =>
                    $"{GetCollectionComparerExpression("SetEqualityComparer", memberModel.TypeName, memberModel)}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.PrecisionEquality when memberModel.IsNullable =>
                    $"({left}.{memberModel.MemberName} == {right}.{memberModel.MemberName} || ({left}.{memberModel.MemberName}.HasValue && {right}.{memberModel.MemberName}.HasValue && global::System.Math.Abs({left}.{memberModel.MemberName}.Value - {right}.{memberModel.MemberName}.Value) < {FormatPrecisionLiteral(memberModel.Precision!.Value, memberModel.TypeName)}))",

                EqualityType.PrecisionEquality =>
                    $"(global::System.Math.Abs({left}.{memberModel.MemberName} - {right}.{memberModel.MemberName}) < {FormatPrecisionLiteral(memberModel.Precision!.Value, memberModel.TypeName)})",

                EqualityType.StringEquality =>
                    $"global::System.StringComparer.{memberModel.StringComparer}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance =>
                    $"{memberModel.ComparerType}.{memberModel.ComparerMemberName}.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.CustomEquality =>
                    $"new {memberModel.ComparerType}().Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                EqualityType.DefaultEquality =>
                    $"global::Generator.Equals.DefaultEqualityComparer<{memberModel.TypeName}>.Default.Equals({left}.{memberModel.MemberName}!, {right}.{memberModel.MemberName}!)",

                _ => null!
            };
        }

        static void BuildMemberInequality(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
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
                    BuildOrderedCollectionInequalities(memberModel, writer, left, right, pathExpr);
                    break;

                case EqualityType.UnorderedEquality when !memberModel.IsDictionary:
                case EqualityType.SetEquality:
                    BuildUnorderedCollectionInequalities(memberModel, writer, left, right, pathExpr);
                    break;

                case EqualityType.UnorderedEquality when memberModel.IsDictionary:
                    BuildDictionaryInequalities(memberModel, writer, left, right, pathExpr);
                    break;

                default:
                    // Simple property diff
                    writer.WriteLine($"if (!{comparerEquals})");
                    writer.WriteLine(1, $"yield return new global::Generator.Equals.Inequality({pathExpr}.Append(global::Generator.Equals.MemberPathSegment.{(memberModel.IsField ? "Field" : "Property")}(\"{memberModel.MemberName}\")), {left}.{memberModel.MemberName}, {right}.{memberModel.MemberName});");
                    break;
            }
        }

        static void BuildOrderedCollectionInequalities(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            var comparerEquals = GetComparerEquals(memberModel, left, right);

            writer.WriteLine($"if (!{comparerEquals})");
            writer.AppendOpenBracket();

            writer.WriteLine($"var __propPath = {pathExpr}.Append(global::Generator.Equals.MemberPathSegment.{(memberModel.IsField ? "Field" : "Property")}(\"{memberModel.MemberName}\"));");
            if (memberModel.IsNonNullableCollection)
            {
                writer.WriteLine($"var __xList = new global::System.Collections.Generic.List<{memberModel.TypeName}>({left}.{memberModel.MemberName});");
                writer.WriteLine($"var __yList = new global::System.Collections.Generic.List<{memberModel.TypeName}>({right}.{memberModel.MemberName});");
            }
            else
            {
                writer.WriteLine($"var __xList = {left}.{memberModel.MemberName} is null ? new global::System.Collections.Generic.List<{memberModel.TypeName}>() : new global::System.Collections.Generic.List<{memberModel.TypeName}>({left}.{memberModel.MemberName});");
                writer.WriteLine($"var __yList = {right}.{memberModel.MemberName} is null ? new global::System.Collections.Generic.List<{memberModel.TypeName}>() : new global::System.Collections.Generic.List<{memberModel.TypeName}>({right}.{memberModel.MemberName});");
            }
            writer.WriteLine("var __maxLen = global::System.Math.Max(__xList.Count, __yList.Count);");
            writer.WriteLine();
            writer.WriteLine("for (var __i = 0; __i < __maxLen; __i++)");
            writer.AppendOpenBracket();

            if (memberModel.EquatableElementTypeName != null)
            {
                // Element type is [Equatable] — drill down into per-property diffs
                writer.WriteLine("if (__i < __xList.Count && __i < __yList.Count)");
                writer.AppendOpenBracket();
                writer.WriteLine($"foreach (var __ineq in {memberModel.EquatableElementTypeName}.EqualityComparer.Default.Inequalities(__xList[__i], __yList[__i], __propPath.Append(global::Generator.Equals.MemberPathSegment.Index(__i))))");
                writer.WriteLine(1, "yield return __ineq;");
                writer.AppendCloseBracket();
                writer.WriteLine("else");
                writer.AppendOpenBracket();
                writer.WriteLine("var __xVal = __i < __xList.Count ? (object?)__xList[__i] : null;");
                writer.WriteLine("var __yVal = __i < __yList.Count ? (object?)__yList[__i] : null;");
                writer.WriteLine("yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Index(__i)), __xVal, __yVal);");
                writer.AppendCloseBracket();
            }
            else
            {
                // Non-equatable element type — shallow diff
                writer.WriteLine("var __xVal = __i < __xList.Count ? (object?)__xList[__i] : null;");
                writer.WriteLine("var __yVal = __i < __yList.Count ? (object?)__yList[__i] : null;");
                writer.WriteLine("if (!global::System.Object.Equals(__xVal, __yVal))");
                writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Index(__i)), __xVal, __yVal);");
            }

            writer.AppendCloseBracket();

            writer.AppendCloseBracket();
        }

        static void BuildUnorderedCollectionInequalities(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            var comparerEquals = GetComparerEquals(memberModel, left, right);

            writer.WriteLine($"if (!{comparerEquals})");
            writer.AppendOpenBracket();

            writer.WriteLine($"var __propPath = {pathExpr}.Append(global::Generator.Equals.MemberPathSegment.{(memberModel.IsField ? "Field" : "Property")}(\"{memberModel.MemberName}\"));");
            if (memberModel.IsNonNullableCollection)
            {
                writer.WriteLine($"var __xSet = new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>({left}.{memberModel.MemberName});");
                writer.WriteLine($"var __ySet = new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>({right}.{memberModel.MemberName});");
            }
            else
            {
                writer.WriteLine($"var __xSet = {left}.{memberModel.MemberName} is null ? new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>() : new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>({left}.{memberModel.MemberName});");
                writer.WriteLine($"var __ySet = {right}.{memberModel.MemberName} is null ? new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>() : new global::System.Collections.Generic.HashSet<{memberModel.TypeName}>({right}.{memberModel.MemberName});");
            }
            writer.WriteLine();
            writer.WriteLine("foreach (var __removed in global::System.Linq.Enumerable.Except(__xSet, __ySet))");
            writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Removed()), __removed, null);");
            writer.WriteLine();
            writer.WriteLine("foreach (var __added in global::System.Linq.Enumerable.Except(__ySet, __xSet))");
            writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Added()), null, __added);");

            writer.AppendCloseBracket();
        }

        static void BuildDictionaryInequalities(EqualityMemberModel memberModel, IndentedTextWriter writer, string left, string right, string pathExpr)
        {
            var comparerEquals = GetComparerEquals(memberModel, left, right);

            writer.WriteLine($"if (!{comparerEquals})");
            writer.AppendOpenBracket();

            writer.WriteLine($"var __propPath = {pathExpr}.Append(global::Generator.Equals.MemberPathSegment.{(memberModel.IsField ? "Field" : "Property")}(\"{memberModel.MemberName}\"));");
            writer.WriteLine($"var __xDict = {left}.{memberModel.MemberName};");
            writer.WriteLine($"var __yDict = {right}.{memberModel.MemberName};");
            writer.WriteLine();

            if (memberModel.IsNonNullableCollection)
            {
                // Value type collection (e.g., ImmutableDictionary) — no null checks needed
                writer.AppendOpenBracket();
            }
            else
            {
                writer.WriteLine("if (__xDict is null && __yDict is not null)");
                writer.AppendOpenBracket();
                writer.WriteLine("foreach (var __kvp in __yDict)");
                writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), null, __kvp.Value);");
                writer.AppendCloseBracket();
                writer.WriteLine("else if (__xDict is not null && __yDict is null)");
                writer.AppendOpenBracket();
                writer.WriteLine("foreach (var __kvp in __xDict)");
                writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), __kvp.Value, null);");
                writer.AppendCloseBracket();
                writer.WriteLine("else if (__xDict is not null && __yDict is not null)");
                writer.AppendOpenBracket();
            }

            // Check for removed/changed keys from x
            writer.WriteLine("foreach (var __kvp in __xDict)");
            writer.AppendOpenBracket();
            writer.WriteLine("if (!__yDict.TryGetValue(__kvp.Key, out var __yVal))");
            writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), __kvp.Value, null);");

            if (memberModel.EquatableElementTypeName != null)
            {
                // Value type is [Equatable] — drill down into per-property diffs
                writer.WriteLine("else");
                writer.AppendOpenBracket();
                writer.WriteLine($"foreach (var __ineq in {memberModel.EquatableElementTypeName}.EqualityComparer.Default.Inequalities(__kvp.Value, __yVal, __propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key))))");
                writer.WriteLine(1, "yield return __ineq;");
                writer.AppendCloseBracket();
            }
            else
            {
                writer.WriteLine("else if (!global::System.Object.Equals(__kvp.Value, __yVal))");
                writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), __kvp.Value, __yVal);");
            }

            writer.AppendCloseBracket();

            // Check for added keys in y
            writer.WriteLine("foreach (var __kvp in __yDict)");
            writer.AppendOpenBracket();
            writer.WriteLine("if (!__xDict.ContainsKey(__kvp.Key))");
            writer.WriteLine(1, "yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), null, __kvp.Value);");
            writer.AppendCloseBracket();

            writer.AppendCloseBracket();

            writer.AppendCloseBracket();
        }

        internal static void BuildMembersInequalities(
            ImmutableArray<EqualityMemberModel> models,
            IndentedTextWriter writer,
            string left = "x",
            string right = "y",
            string pathExpr = "path"
        )
        {
            foreach (var model in models)
            {
                BuildMemberInequality(model, writer, left, right, pathExpr);
            }
        }
    }
}