using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class RecordEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var symbolName = symbol.ToFQF();
            var baseTypeName = symbol.BaseType?.ToFQF();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(symbol.IsSealed
                ? $"public bool Equals({symbolName}? other)"
                : $"public virtual bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return");

            writer.Indent++;

            writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "!ReferenceEquals(other, null) && EqualityContract == other.EqualityContract"
                : $"base.Equals(({baseTypeName}?)other)");

            BuildMembersEquality(symbol, attributesMetadata, writer, explicitMode, m => m.ToFQF() != "EqualityContract");

            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        static void BuildGetHashCode(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var baseTypeName = symbol.BaseType?.ToFQF();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();
            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            writer.WriteLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "hashCode.Add(this.EqualityContract);"
                : "hashCode.Add(base.GetHashCode());");

            BuildMembersHashCode(symbol, attributesMetadata, writer, explicitMode, m => m.ToFQF() != "EqualityContract");

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");
            writer.AppendCloseBracket();
        }

        public static string Generate(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: writer =>
            {
                BuildEquals(symbol, attributesMetadata, writer, explicitMode, ignoreInheritedMembers);

                writer.WriteLine();

                BuildGetHashCode(symbol, attributesMetadata, writer, explicitMode, ignoreInheritedMembers);
            });

            return code;
        }
    }
}