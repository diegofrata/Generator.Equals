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
        
        var comparerFieldName = LocalFieldGenerator.GetFieldName(memberModel);

        switch (memberModel.EqualityType)
        {
            case EqualityType.IgnoreEquality:
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { IsDictionary: false, StringComparer: not null and not "" }:

                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { IsDictionary: false, StringComparer: null }:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { IsDictionary: true, StringComparer: null }:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.OrderedEquality
                when memberModel is { StringComparer: not null and not "" }:

                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.OrderedEquality:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.ReferenceEquality:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.SetEquality:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.StringEquality:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;

            case EqualityType.CustomEquality when memberModel.ComparerHasStaticInstance:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");

                break;

            case EqualityType.CustomEquality when !memberModel.ComparerHasStaticInstance:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");

                break;

            case EqualityType.DefaultEquality:
                writer.WriteLine(
                    $"&& {comparerFieldName}.Equals(this.{memberModel.PropertyName}!, other.{memberModel.PropertyName}!)");
                break;
        }
    }
}