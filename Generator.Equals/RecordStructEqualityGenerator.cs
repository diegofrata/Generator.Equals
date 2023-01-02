using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class RecordStructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode)
        {
            var symbolName = symbol.ToFQF();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine( $"public bool Equals({symbolName} other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return true");

            writer.Indent++;
            BuildMembersEquality(symbol, attributesMetadata, writer, explicitMode);
            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        static void BuildGetHashCode(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode)
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();
            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();
            
            BuildMembersHashCode(symbol, attributesMetadata, writer, explicitMode);

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");
            writer.AppendCloseBracket();
        }

        public static string Generate(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            bool explicitMode)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: writer =>
            {
                BuildEquals(symbol, attributesMetadata, writer, explicitMode);

                writer.WriteLine();

                BuildGetHashCode(symbol, attributesMetadata, writer, explicitMode);
            });

            return code;
        }
    }
}