// using System.CodeDom.Compiler;
// using System.Collections.Immutable;
// using Generator.Equals.Models;
//
// namespace Generator.Equals.Generators.Core;
//
// internal static class LocalFieldGenerator
// {
//     internal static void BuildEqualityComparerFields(
//         ImmutableArray<EqualityMemberModel> models,
//         IndentedTextWriter writer
//     )
//     {
//         foreach (var model in models)
//         {
//             if (model.Ignored) continue;
//
//             switch (model.EqualityType)
//             {
//                 case EqualityType.UnorderedEquality when model.StringComparer is not null:
//                     line =(
//                         $"private static readonly global::Generator.Equals.UnorderedEqualityComparer<{model.TypeName}> _unorderedEqualityComparer_{model.TypeName}_{model.StringComparer} = " +
//                         $"new(global::System.StringComparer.{model.StringComparer});");
//                     break;
//
//                 case EqualityType.OrderedEquality when model.StringComparer is not null:
//                     line =(
//                         $"private static readonly global::Generator.Equals.OrderedEqualityComparer<{model.TypeName}> _orderedEqualityComparer_{model.TypeName}_{model.StringComparer} = " +
//                         $"new(global::System.StringComparer.{model.StringComparer});");
//                     break;
//
//                 // Add more cases here if you need additional comparer types (SetEquality, ReferenceEquality, etc.)
//             }
//         }
//     }
// }

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Generator.Equals.Models;

namespace Generator.Equals.Generators.Core;

internal static class LocalFieldGenerator
{
    internal static void BuildEqualityComparerFields(
        ImmutableArray<EqualityMemberModel> models,
        IndentedTextWriter writer
    )
    {
        var hashset = new HashSet<string>();

        foreach (var model in models)
        {
            if (model.Ignored) continue;
            
            var fieldName = GetFieldName(model);

            var localFieldDefinition = model.EqualityType switch
            {
                EqualityType.UnorderedEquality when model.IsDictionary && model.StringComparer is null =>
                    $"private static readonly global::Generator.Equals.DictionaryEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"global::Generator.Equals.DictionaryEqualityComparer<{model.TypeName}>.Default;",
                EqualityType.UnorderedEquality when model.StringComparer is not null =>
                    $"private static readonly global::Generator.Equals.UnorderedEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"new(global::System.StringComparer.{model.StringComparer});",
                EqualityType.UnorderedEquality when model.StringComparer is null =>
                    $"private static readonly global::Generator.Equals.UnorderedEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"global::Generator.Equals.UnorderedEqualityComparer<{model.TypeName}>.Default;",
                EqualityType.OrderedEquality when model.StringComparer is not null =>
                    $"private static readonly global::Generator.Equals.OrderedEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"new(global::System.StringComparer.{model.StringComparer});",
                EqualityType.OrderedEquality =>
                    $"private static readonly global::Generator.Equals.OrderedEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"global::Generator.Equals.OrderedEqualityComparer<{model.TypeName}>.Default;",
                EqualityType.ReferenceEquality => (
                    $"private static readonly global::Generator.Equals.ReferenceEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"global::Generator.Equals.ReferenceEqualityComparer<{model.TypeName}>.Default;"),
                EqualityType.SetEquality => (
                    $"private static readonly global::Generator.Equals.SetEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"global::Generator.Equals.SetEqualityComparer<{model.TypeName}>.Default;"),
                EqualityType.StringEquality when model.StringComparer is not null => (
                    $"private static readonly global::System.StringComparer {fieldName} = " +
                    $"global::System.StringComparer.{model.StringComparer};"),
                EqualityType.CustomEquality when model.ComparerHasStaticInstance => (
                    $"private static readonly {model.ComparerType} {fieldName} = " +
                    $"{model.ComparerType}.{model.ComparerMemberName};"),
                EqualityType.CustomEquality when !model.ComparerHasStaticInstance => (
                    $"private static readonly {model.ComparerType} {fieldName} = " +
                    $"new {model.ComparerType}();"),
                EqualityType.DefaultEquality => (
                    $"private static readonly global::Generator.Equals.DefaultEqualityComparer<{model.TypeName}> {fieldName} = " +
                    $"global::Generator.Equals.DefaultEqualityComparer<{model.TypeName}>.Default;"),
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(localFieldDefinition) && hashset.Add(localFieldDefinition))
            {
                writer.WriteLine(localFieldDefinition);
            }
        }
    }
    
    private static readonly Regex _typeNameClean = new Regex(@"[<>:.]", RegexOptions.Compiled);
    
    public static string GetFieldName(EqualityMemberModel model)
    {
        // Replace <>:. with _
        var cleanedTypeName = _typeNameClean.Replace(model.TypeName, "_");
        
        
        return model.EqualityType switch
        {
            EqualityType.UnorderedEquality when model.IsDictionary && model.StringComparer is null => $"_dictionaryEqualityComparer_{cleanedTypeName}",
            EqualityType.UnorderedEquality when model.StringComparer is not null => $"_unorderedEqualityComparer_{cleanedTypeName}_{model.StringComparer}",
            EqualityType.UnorderedEquality when model.StringComparer is null => $"_unorderedEqualityComparer_{cleanedTypeName}",
            EqualityType.OrderedEquality when model.StringComparer is not null => $"_orderedEqualityComparer_{cleanedTypeName}_{model.StringComparer}",
            EqualityType.OrderedEquality => $"_orderedEqualityComparer_{cleanedTypeName}",
            EqualityType.ReferenceEquality => $"_referenceEqualityComparer_{cleanedTypeName}",
            EqualityType.SetEquality => $"_setEqualityComparer_{cleanedTypeName}",
            EqualityType.StringEquality when model.StringComparer is not null => $"_stringComparer_{model.StringComparer}",
            EqualityType.CustomEquality when model.ComparerHasStaticInstance => $"_customComparer_{model.ComparerType}_{model.PropertyName}",
            EqualityType.CustomEquality when !model.ComparerHasStaticInstance => $"_customComparer_{model.ComparerType}_{model.PropertyName}",
            EqualityType.DefaultEquality => $"_defaultEqualityComparer_{cleanedTypeName}",
            _ => string.Empty
        };
    }
}