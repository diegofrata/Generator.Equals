using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    sealed class StructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildDelegatingMethods(EqualityTypeModel model, IndentedTextWriter writer)
        {
            var symbolName = model.Fullname;

            // == operator
            writer.WriteLines(EqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator ==({symbolName} left, {symbolName} right) =>");
            writer.WriteLine(1, "EqualityComparer.Default.Equals(left, right);");
            writer.WriteLine();

            // != operator
            writer.WriteLines(NotEqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator !=({symbolName} left, {symbolName} right) =>");
            writer.WriteLine(1, "!EqualityComparer.Default.Equals(left, right);");
            writer.WriteLine();

            // Equals(object?)
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override bool Equals(object? obj) =>");
            writer.WriteLine(1, $"obj is {symbolName} o && EqualityComparer.Default.Equals(this, o);");
            writer.WriteLine();

            // Equals(T)
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public bool Equals({symbolName} other) =>");
            writer.WriteLine(1, "EqualityComparer.Default.Equals(this, other);");
            writer.WriteLine();

            // GetHashCode()
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override int GetHashCode() =>");
            writer.WriteLine(1, "EqualityComparer.Default.GetHashCode(this);");
        }

        static void BuildNestedEqualityComparer(EqualityTypeModel model, IndentedTextWriter writer)
        {
            var symbolName = model.Fullname;

            writer.WriteLine();
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<{symbolName}>");
            writer.AppendOpenBracket();

            // Default instance
            writer.WriteLine("public static EqualityComparer Default { get; } = new EqualityComparer();");
            writer.WriteLine();

            // Equals(T, T)
            BuildComparerEquals(model, writer, symbolName);

            writer.WriteLine();

            // GetHashCode(T)
            BuildComparerGetHashCode(model, writer, symbolName);

            writer.WriteLine();

            // Inequalities method
            BuildInequalitiesMethod(model, writer, symbolName);

            writer.AppendCloseBracket();
        }

        static void BuildInequalitiesMethod(EqualityTypeModel model, IndentedTextWriter writer, string symbolName)
        {
            writer.WriteLines(InequalitiesMethodComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities({symbolName} x, {symbolName} y, global::Generator.Equals.MemberPath path = default)");
            writer.AppendOpenBracket();

            BuildMembersInequalities(model.BuildEqualityModels, writer, "x", "y");

            // Need to add yield break if there are no members to ensure it's an iterator
            if (model.BuildEqualityModels.Items.Length == 0)
            {
                writer.WriteLine("yield break;");
            }

            writer.AppendCloseBracket();
        }

        static void BuildComparerEquals(EqualityTypeModel model, IndentedTextWriter writer, string symbolName)
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public bool Equals({symbolName} x, {symbolName} y)");
            writer.AppendOpenBracket();

            writer.WriteLine("return true");

            writer.Indent++;
            BuildMembersEquality(model.BuildEqualityModels, writer, "x", "y");
            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        static void BuildComparerGetHashCode(EqualityTypeModel model, IndentedTextWriter writer, string symbolName)
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public int GetHashCode({symbolName} obj)");
            writer.AppendOpenBracket();

            writer.WriteLine("var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            BuildMembersHashCode(model.BuildEqualityModels, writer, "obj");

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");

            writer.AppendCloseBracket();
        }

        public static string Generate(EqualityTypeModel model)
        {
            var code = ContainingTypesBuilder.Build(model.ContainingSymbols, content: writer =>
            {
                writer.WriteLine($"partial struct {model.TypeName} : global::System.IEquatable<{model.Fullname}>");
                writer.AppendOpenBracket();

                BuildDelegatingMethods(model, writer);

                BuildNestedEqualityComparer(model, writer);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}
