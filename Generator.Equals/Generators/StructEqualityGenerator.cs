using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    internal sealed class StructEqualityGenerator : EqualityGeneratorBase
    {
        private static void BuildEquals(EqualityTypeModel model, IndentedTextWriter writer)
        {
            var symbolName = model.TypeName;

            writer.WriteLines(EqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public static bool operator ==(");
            writer.WriteLine(1, $"{symbolName} left,");
            writer.WriteLine(1, $"{symbolName} right) =>");
            writer.WriteLine(1, $"global::Generator.Equals.DefaultEqualityComparer<{symbolName}>.Default");
            writer.WriteLine(2, $".Equals(left, right);");
            writer.WriteLine();

            writer.WriteLines(NotEqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator !=({symbolName} left, {symbolName} right) =>");
            writer.WriteLine(1, "!(left == right);");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override bool Equals(object? obj) =>");
            writer.WriteLine(1, $"obj is {symbolName} o && Equals(o);");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public bool Equals({symbolName} other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return true");

            writer.Indent++;
            BuildMembersEquality(model.BuildEqualityModels, writer);

            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        private static void BuildGetHashCode(EqualityTypeModel model, IndentedTextWriter writer)
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            BuildMembersHashCode(model.BuildEqualityModels, writer);

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");

            writer.AppendCloseBracket();
        }

        public static string Generate(EqualityTypeModel model)
        {
            // Generate the code using the custom model instead of Roslyn types
            var code = ContainingTypesBuilder.Build(model.ContainingSymbols, content: writer =>
            {
                writer.WriteLine($"partial struct {model.TypeName} : global::System.IEquatable<{model.TypeName}>");
                writer.AppendOpenBracket();

                // BuildEquals and BuildGetHashCode are adjusted to accept the custom model instead of Roslyn symbols
                BuildEquals(model, writer);

                writer.WriteLine();

                BuildGetHashCode(model, writer);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}