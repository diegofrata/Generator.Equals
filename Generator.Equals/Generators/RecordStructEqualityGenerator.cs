using System.CodeDom.Compiler;
using Generator.Equals.Generators.Core;
using Generator.Equals.Models;

namespace Generator.Equals.Generators;

internal sealed class RecordStructGenerator 
{
    private static void BuildEquals(
        EqualityTypeModel model,
        IndentedTextWriter writer
    )
    {
        var symbolName = model.TypeName;

        writer.WriteLine(GeneratorConstants.InheritDocComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine($"public bool Equals({symbolName} other)");
        writer.AppendOpenBracket();

        writer.WriteLine("return true");

        writer.Indent++;
        EqualityMethodGenerator.BuildMembersEquality(model.BuildEqualityModels, writer);
        writer.WriteLine(";");
        writer.Indent--;

        writer.AppendCloseBracket();
    }

    private static void BuildGetHashCode(
        EqualityTypeModel model,
        IndentedTextWriter writer
    )
    {
        writer.WriteLine(GeneratorConstants.InheritDocComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine(@"public override int GetHashCode()");
        writer.AppendOpenBracket();

        writer.WriteLine(@"var hashCode = new global::System.HashCode();");
        writer.WriteLine();

        HashCodeMethodGenerator.BuildMembersHashCode(model.BuildEqualityModels, writer);

        writer.WriteLine();
        writer.WriteLine("return hashCode.ToHashCode();");
        writer.AppendCloseBracket();
    }

    public static string Generate(EqualityTypeModel model) 
        => ContainingTypesBuilder.Build(model, static (model, writer) =>
        {
            BuildEquals(model, writer);
            writer.WriteLine();
            BuildGetHashCode(model, writer);
        });
}