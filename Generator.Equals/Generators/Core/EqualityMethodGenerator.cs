using Generator.Equals.Models;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Generator.Equals.Generators.Core;

internal class EqualityMethodGenerator
{
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

    internal static void BuildEquality(EqualityMemberModel memberModel, IndentedTextWriter writer)
    {
        if (memberModel.Ignored)
        {
            return;
        }

        switch (memberModel.EqualityType)
        {
            case EqualityType.IgnoreEquality:
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { IsDictionary: false, StringComparer: not null and not "" }:

                writer.WriteLine(
                    $"&& new global::Generator.Equals.UnorderedEqualityComparer<{memberModel.TypeName}>(global::System.StringComparer.{memberModel.StringComparer}).Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { IsDictionary: false, StringComparer: null }:
                writer.WriteLine(
                    $"&& global::Generator.Equals.UnorderedEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { IsDictionary: true, StringComparer: null }:
                writer.WriteLine(
                    $"&& global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.OrderedEquality
                when memberModel is { StringComparer: not null and not "" }:

                writer.WriteLine(
                    $"&& new global::Generator.Equals.OrderedEqualityComparer<{memberModel.TypeName}>(global::System.StringComparer.{memberModel.StringComparer}).Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
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
}