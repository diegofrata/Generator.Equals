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
