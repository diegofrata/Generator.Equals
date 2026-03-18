using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    class RecordEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            bool ignoreInheritedMembers = model.IgnoreInheritedMembers;
            var symbolName = model.Fullname;
            var baseTypeName = model.BaseTypeName;
            var baseTypeFullname = model.BaseTypeFullname;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(model.IsSealed
                ? $"public bool Equals({symbolName}? other)"
                : $"public virtual bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return");

            writer.Indent++;

            // For records, use base.Equals() to properly chain through the inheritance hierarchy
            // This ensures that intermediate types with manual Equals overrides are called
            if (baseTypeName == "object" || ignoreInheritedMembers)
            {
                writer.WriteLine("!ReferenceEquals(other, null) && EqualityContract == other.EqualityContract");
            }
            else
            {
                writer.WriteLine($"base.Equals(({baseTypeFullname}?)other)");
            }

            // Include inherited members (when no ancestor has [Equatable])
            BuildMembersEquality(model.InheritedEqualityModels, writer, "this", "other");
            BuildMembersEquality(model.BuildEqualityModels, writer, "this", "other");

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

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            // For records, use base.GetHashCode() to properly chain through the inheritance hierarchy
            if (baseTypeName == "object" || ignoreInheritedMembers)
            {
                writer.WriteLine("hashCode.Add(this.EqualityContract);");
            }
            else
            {
                writer.WriteLine("hashCode.Add(base.GetHashCode());");
            }

            // Include inherited members (when no ancestor has [Equatable])
            BuildMembersHashCode(model.InheritedEqualityModels, writer, "this");
            BuildMembersHashCode(model.BuildEqualityModels, writer, "this");

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");

            writer.AppendCloseBracket();
        }

        static void BuildNestedEqualityComparer(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var symbolName = model.Fullname;
            // Use 'new' to suppress CS0108 warning when hiding base class's EqualityComparer
            var newKeyword = model.BaseHasEquatable ? "new " : "";

            writer.WriteLine();
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public {newKeyword}sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<{symbolName}>");
            writer.AppendOpenBracket();

            // Default instance
            writer.WriteLine("public static EqualityComparer Default { get; } = new EqualityComparer();");
            writer.WriteLine();

            // Equals(T?, T?) - delegates to the type's Equals method
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public bool Equals({symbolName}? x, {symbolName}? y)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(x, y)) return true;");
            writer.WriteLine("if (x is null || y is null) return false;");
            writer.WriteLine();
            writer.WriteLine("return x.Equals(y);");

            writer.AppendCloseBracket();

            writer.WriteLine();

            // GetHashCode(T) - delegates to the type's GetHashCode method
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public int GetHashCode({symbolName} obj)");
            writer.AppendOpenBracket();

            writer.WriteLine("return obj.GetHashCode();");

            writer.AppendCloseBracket();

            writer.WriteLine();

            // Inequalities method
            BuildInequalitiesMethod(model, writer, symbolName);

            writer.AppendCloseBracket();
        }

        static void BuildInequalitiesMethod(EqualityTypeModel model, IndentedTextWriter writer, string symbolName)
        {
            var baseTypeName = model.BaseTypeName;
            var baseTypeFullname = model.BaseTypeFullname;
            var isRootRecord = baseTypeName == "object";

            writer.WriteLines(InequalitiesMethodComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities({symbolName}? x, {symbolName}? y, global::Generator.Equals.MemberPath path = default)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(x, y)) yield break;");
            writer.WriteLine("if (x is null || y is null)");
            writer.AppendOpenBracket();
            writer.WriteLine("yield return new global::Generator.Equals.Inequality(path, x, y);");
            writer.WriteLine("yield break;");
            writer.AppendCloseBracket();
            writer.WriteLine();

            // For records with [Equatable] base, delegate to base's Inequalities
            if (!isRootRecord && !model.IgnoreInheritedMembers && model.BaseHasEquatable)
            {
                writer.WriteLine($"foreach (var __ineq in {baseTypeFullname}.EqualityComparer.Default.Inequalities(x, y, path))");
                writer.WriteLine(1, "yield return __ineq;");
                writer.WriteLine();
            }

            // Include inherited members (when no ancestor has [Equatable])
            BuildMembersInequalities(model.InheritedEqualityModels, writer, "x", "y");
            BuildMembersInequalities(model.BuildEqualityModels, writer, "x", "y");

            writer.AppendCloseBracket();
        }

        public static string Generate(EqualityTypeModel model)
        {
            var code = ContainingTypesBuilder.Build(model.ContainingSymbols, content: writer =>
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
