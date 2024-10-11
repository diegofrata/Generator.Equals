using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    internal sealed class ClassEqualityGenerator : EqualityGeneratorBase
    {
        private static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var ignoreInheritedMembers = model.IgnoreInheritedMembers;
            var symbolName = model.TypeName;
            var baseTypeName = model.BaseTypeName;
            var isRootClass = baseTypeName == "object";

            writer.WriteLines(EqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public static bool operator ==(");
            writer.WriteLine(1, $"{symbolName}? left,");
            writer.WriteLine(1, $"{symbolName}? right) =>");
            writer.WriteLine(1, $"global::Generator.Equals.DefaultEqualityComparer<{symbolName}?>.Default");
            writer.WriteLine(2, $".Equals(left, right);");
            writer.WriteLine();

            writer.WriteLines(NotEqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator !=({symbolName}? left, {symbolName}? right) =>");
            writer.WriteLine(1, "!(left == right);");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override bool Equals(object? obj) =>");
            writer.WriteLine(1, $"Equals(obj as {symbolName});");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"bool global::System.IEquatable<{symbolName}>.Equals({symbolName}? obj) => Equals((object?) obj);");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"{(model.IsSealed ? "private" : "protected")} bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(null, other)) return false;");
            writer.WriteLine("if (ReferenceEquals(this, other)) return true;");
            writer.WriteLine();

            if (isRootClass || ignoreInheritedMembers)
            {
                writer.WriteLine("return other.GetType() == this.GetType()");
            }
            else
            {
                writer.WriteLine($"return base.Equals(other as {baseTypeName})");
            }

            writer.Indent++;
            BuildMembersEquality(model.BuildEqualityModels, writer);
            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        private static void BuildGetHashCode(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var ignoreInheritedMembers = model.IgnoreInheritedMembers;
            var baseTypeName = model.BaseTypeName;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "hashCode.Add(this.GetType());"
                : "hashCode.Add(base.GetHashCode());");

            BuildMembersHashCode(model.BuildEqualityModels, writer);

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");

            writer.AppendCloseBracket();
        }

        public static string Generate(EqualityTypeModel model)
        {
            var code = ContainingTypesBuilder.Build(model.ContainingSymbols, content: writer =>
            {
                writer.WriteLine($"partial class {model.TypeName} : global::System.IEquatable<{model.TypeName}>");
                writer.AppendOpenBracket();

                BuildEquals(model, writer);

                writer.WriteLine();

                BuildGetHashCode(model, writer);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}