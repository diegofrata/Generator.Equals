using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    internal sealed class RecordStructEqualityGenerator : EqualityGeneratorBase
    {
        private static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer)
        {
            var symbolName = model.TypeName;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public bool Equals({symbolName} other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return true");

            writer.Indent++;
            // Assuming you update BuildMembersEquality to accept the new model
            BuildMembersEquality(model.BuildEqualityModels, writer);
            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        private static void BuildGetHashCode(
            EqualityTypeModel model,
            IndentedTextWriter writer)
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            // Assuming you update BuildMembersHashCode to accept the new model
            BuildMembersHashCode(model.BuildEqualityModels, writer);

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");
            writer.AppendCloseBracket();
        }

        public static string Generate(EqualityTypeModel model)
        {
            var code = ContainingTypesBuilder.Build(
                model.ContainingSymbols,
                content: writer =>
                {
                    BuildEquals(model, writer);
                    writer.WriteLine();
                    BuildGetHashCode(model, writer);
                });

            return code;
        }
    }
}