using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class StructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            IndentedTextWriter writer,
            bool explicitMode)
        {
            var symbolName = symbol.ToFQF();
            
            writer.WriteLines(EqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public static bool operator ==(");
            writer.WriteLine(1, $"{symbolName} left,");
            writer.WriteLine(1, $"{symbolName} right) =>");
            writer.WriteLine(1, $"global::Generator.Equals.DefaultEqualityComparer<{symbolName}>.Default");
            writer.WriteLine(2, $".Equals(left, right);");
            writer.WriteLine();

            writer.WriteLines(NotEqualsOperatorCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public static bool operator !=({symbolName} left, {symbolName} right) =>");
            writer.WriteLine(1, "!(left == right);");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override bool Equals(object? obj) =>");
            writer.WriteLine(1, $"obj is {symbolName} o && Equals(o);");
            writer.WriteLine();

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public bool Equals({symbolName} other)");
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
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: false, content: writer =>
            {
                var typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                writer.WriteLine($"partial struct {typeName} : global::System.IEquatable<{typeName}>");
                writer.AppendOpenBracket();

                BuildEquals(symbol, attributesMetadata, writer, explicitMode);

                writer.WriteLine();
                
                BuildGetHashCode(symbol, attributesMetadata, writer, explicitMode);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}