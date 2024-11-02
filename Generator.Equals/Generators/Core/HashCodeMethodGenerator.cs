using Generator.Equals.Models;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Generator.Equals.Generators.Core;

internal class HashCodeMethodGenerator
{
    public static void BuildMembersHashCode(ImmutableArray<EqualityMemberModel> models, IndentedTextWriter writer)
    {
        foreach (var model in models)
        {
            HashCodeMethodGenerator.BuildHashCode(model, writer);
        }
    }

    internal static void BuildHashCode(EqualityMemberModel memberModel, IndentedTextWriter writer)
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
                when memberModel is { StringComparer: null, IsDictionary: true }:

                BuildHashCodeAdd($"global::Generator.Equals.DictionaryEqualityComparer<{memberModel.TypeName}>.Default");
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { StringComparer: null, IsDictionary: false }:

                BuildHashCodeAdd($"global::Generator.Equals.UnorderedEqualityComparer<{memberModel.TypeName}>.Default");
                break;

            case EqualityType.UnorderedEquality
                when memberModel is { StringComparer: not null and not "", IsDictionary: false }:

                BuildHashCodeAdd($"new global::Generator.Equals.UnorderedEqualityComparer<{memberModel.TypeName}>(global::System.StringComparer.{memberModel.StringComparer})");
                break;

            case EqualityType.OrderedEquality
                when memberModel is { StringComparer: not null and not "" }:

                BuildHashCodeAdd($"new global::Generator.Equals.OrderedEqualityComparer<{memberModel.TypeName}>(global::System.StringComparer.{memberModel.StringComparer})");
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
}