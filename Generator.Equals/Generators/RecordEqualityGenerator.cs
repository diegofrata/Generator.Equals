using System.CodeDom.Compiler;
using Generator.Equals.Generators.Core;
using Generator.Equals.Models;

namespace Generator.Equals.Generators;

internal static class RecordGenerator
{
    static void BuildEquals(
        EqualityTypeModel model,
        IndentedTextWriter writer
    )
    {
        bool ignoreInheritedMembers = model.IgnoreInheritedMembers;
        var symbolName = model.TypeName;
        var baseTypeName = model.BaseTypeName;

        writer.WriteLine(GeneratorConstants.InheritDocComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine(model.IsSealed
            ? $"public bool Equals({symbolName}? other)"
            : $"public virtual bool Equals({symbolName}? other)");
        writer.AppendOpenBracket();

        writer.WriteLine("return");

        writer.Indent++;

        writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
            ? "!ReferenceEquals(other, null) && EqualityContract == other.EqualityContract"
            : $"base.Equals(({baseTypeName}?)other)");

        EqualityMethodGenerator.BuildMembersEquality(model.BuildEqualityModels, writer);

        writer.WriteLine(";");
        writer.Indent--;

        writer.AppendCloseBracket();
    }

    static void BuildGetHashCode(
        EqualityTypeModel model,
        IndentedTextWriter writer
    )
    {
        bool ignoreInheritedMembers = model.IgnoreInheritedMembers;
        var baseTypeName = model.BaseTypeName;

        writer.WriteLine(GeneratorConstants.InheritDocComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine(@"public override int GetHashCode()");
        writer.AppendOpenBracket();

        writer.WriteLine(@"var hashCode = new global::System.HashCode();");
        writer.WriteLine();

        writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
            ? "hashCode.Add(this.EqualityContract);"
            : "hashCode.Add(base.GetHashCode());");

        HashCodeMethodGenerator.BuildMembersHashCode(model.BuildEqualityModels, writer);

        writer.WriteLine();
        writer.WriteLine("return hashCode.ToHashCode();");

        writer.AppendCloseBracket();
    }

    public static string Generate(EqualityTypeModel model) => 
        ContainingTypesBuilder.Build(model, static (model, writer) =>
        {
            BuildEquals(model, writer);

            writer.WriteLine();

            BuildGetHashCode(model, writer);
        });
}