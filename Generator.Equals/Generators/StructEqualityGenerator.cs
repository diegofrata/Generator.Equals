using System.CodeDom.Compiler;
using System.Text;
using Generator.Equals.Generators.Core;
using Generator.Equals.Models;

namespace Generator.Equals.Generators;

internal sealed class StructGenerator
{
    private static void BuildEquals(EqualityTypeModel model, IndentedTextWriter writer)
    {
        var symbolName = model.TypeName;

        writer.WriteLines(GeneratorConstants.EqualsOperatorCodeComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine("public static bool operator ==(");
        writer.WriteLine(1, $"{symbolName} left,");
        writer.WriteLine(1, $"{symbolName} right) =>");
        writer.WriteLine(1, $"global::Generator.Equals.DefaultEqualityComparer<{symbolName}>.Default");
        writer.WriteLine(2, $".Equals(left, right);");
        writer.WriteLine();

        writer.WriteLines(GeneratorConstants.NotEqualsOperatorCodeComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine($"public static bool operator !=({symbolName} left, {symbolName} right) =>");
        writer.WriteLine(1, "!(left == right);");
        writer.WriteLine();

        writer.WriteLine(GeneratorConstants.InheritDocComment);
        writer.WriteLine(GeneratorConstants.GeneratedCodeAttributeDeclaration);
        writer.WriteLine("public override bool Equals(object? obj) =>");
        writer.WriteLine(1, $"obj is {symbolName} o && Equals(o);");
        writer.WriteLine();

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

    private static void BuildGetHashCode(EqualityTypeModel model, IndentedTextWriter writer)
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
            writer.WriteLine($"partial struct {model.TypeName} : global::System.IEquatable<{model.TypeName}>");
            writer.AppendOpenBracket();

            BuildEquals(model, writer);

            writer.WriteLine();

            BuildGetHashCode(model, writer);
        });
}