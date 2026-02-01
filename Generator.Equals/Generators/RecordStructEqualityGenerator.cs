using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    sealed class RecordStructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer)
        {
            var symbolName = model.Fullname;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public bool Equals({symbolName} other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return true");

            writer.Indent++;
            BuildMembersEquality(model.BuildEqualityModels, writer, "this", "other");
            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        static void BuildGetHashCode(
            EqualityTypeModel model,
            IndentedTextWriter writer)
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            BuildMembersHashCode(model.BuildEqualityModels, writer, "this");

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");
            writer.AppendCloseBracket();
        }

        static void BuildNestedEqualityComparer(
            EqualityTypeModel model,
            IndentedTextWriter writer)
        {
            var symbolName = model.Fullname;

            writer.WriteLine();
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<{symbolName}>");
            writer.AppendOpenBracket();

            // Default instance
            writer.WriteLine("public static EqualityComparer Default { get; } = new EqualityComparer();");
            writer.WriteLine();

            // Equals(T, T) - delegates to the type's Equals method
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public bool Equals({symbolName} x, {symbolName} y)");
            writer.AppendOpenBracket();

            writer.WriteLine("return x.Equals(y);");

            writer.AppendCloseBracket();

            writer.WriteLine();

            // GetHashCode(T) - delegates to the type's GetHashCode method
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public int GetHashCode({symbolName} obj)");
            writer.AppendOpenBracket();

            writer.WriteLine("return obj.GetHashCode();");

            writer.AppendCloseBracket();

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
                    BuildNestedEqualityComparer(model, writer);
                });

            return code;
        }
    }
}
