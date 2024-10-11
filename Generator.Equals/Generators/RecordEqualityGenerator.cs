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
            var symbolName = model.TypeName;
            var baseTypeName = model.BaseTypeName;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(model.IsSealed
                ? $"public bool Equals({symbolName}? other)"
                : $"public virtual bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return");

            writer.Indent++;

            writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "!ReferenceEquals(other, null) && EqualityContract == other.EqualityContract"
                : $"base.Equals(({baseTypeName}?)other)");

            BuildMembersEquality(model.BuildEqualityModels, writer); // , m => m.MemberName != "EqualityContract"

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

            writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "hashCode.Add(this.EqualityContract);"
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
                BuildEquals(model, writer);

                writer.WriteLine();

                BuildGetHashCode(model, writer);
            });

            return code;
        }
    }
}