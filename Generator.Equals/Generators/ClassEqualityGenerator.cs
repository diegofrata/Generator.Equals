using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    sealed class ClassEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildDelegatingMethods(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var symbolName = model.Fullname;

            // == operator
            writer.WriteLines(EqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator ==({symbolName}? left, {symbolName}? right) =>");
            writer.WriteLine(1, "EqualityComparer.Default.Equals(left, right);");
            writer.WriteLine();

            // != operator
            writer.WriteLines(NotEqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator !=({symbolName}? left, {symbolName}? right) =>");
            writer.WriteLine(1, "!EqualityComparer.Default.Equals(left, right);");
            writer.WriteLine();

            // Equals(object?)
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override bool Equals(object? obj) =>");
            writer.WriteLine(1, $"Equals(obj as {symbolName});");
            writer.WriteLine();

            // IEquatable<T>.Equals(T?) - explicit interface implementation
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"bool global::System.IEquatable<{symbolName}>.Equals({symbolName}? obj) => Equals((object?) obj);");
            writer.WriteLine();
        }

        static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var ignoreInheritedMembers = model.IgnoreInheritedMembers;
            var symbolName = model.Fullname;
            var baseTypeName = model.BaseTypeName;
            var baseTypeFullname = model.BaseTypeFullname;
            // Treat as root class if base is object OR if base doesn't have [Equatable]
            // (since base.Equals() would just use object reference equality)
            var isRootClass = baseTypeName == "object" || !model.BaseHasEquatable;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"{(model.IsSealed ? "private" : "protected")} bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(null, other)) return false;");
            writer.WriteLine("if (ReferenceEquals(this, other)) return true;");
            writer.WriteLine();

            // For classes, use base.Equals() to properly chain through the inheritance hierarchy
            // This ensures that intermediate types with manual Equals overrides are called
            if (isRootClass || ignoreInheritedMembers)
            {
                writer.WriteLine("return other.GetType() == this.GetType()");
            }
            else
            {
                writer.WriteLine($"return base.Equals(other as {baseTypeFullname})");
            }

            writer.Indent++;
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
            var ignoreInheritedMembers = model.IgnoreInheritedMembers;
            var baseTypeName = model.BaseTypeName;

            var isRootClass = baseTypeName == "object" || !model.BaseHasEquatable;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            // For classes, use base.GetHashCode() to properly chain through the inheritance hierarchy
            writer.WriteLine(isRootClass || ignoreInheritedMembers
                ? "hashCode.Add(this.GetType());"
                : "hashCode.Add(base.GetHashCode());");

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
                writer.WriteLine($"partial class {model.TypeName} : global::System.IEquatable<{model.Fullname}>");
                writer.AppendOpenBracket();

                BuildDelegatingMethods(model, writer);

                BuildEquals(model, writer);

                writer.WriteLine();

                BuildGetHashCode(model, writer);

                BuildNestedEqualityComparer(model, writer);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}
