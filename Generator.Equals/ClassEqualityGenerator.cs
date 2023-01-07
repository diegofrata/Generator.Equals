using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class ClassEqualityGenerator : EqualityGeneratorBase
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
            writer.WriteLine($"{(symbol.IsSealed ? "private" : "protected")} bool Equals({symbolName}? other)");
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
            BuildMembersEquality(symbol, attributesMetadata, writer, explicitMode);
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
                ? "hashCode.Add(this.GetType());"
                : "hashCode.Add(base.GetHashCode());");

            BuildMembersHashCode(symbol, attributesMetadata, writer, explicitMode);

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
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: false, content: writer =>
            {
                var typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                writer.WriteLine($"partial class {typeName} : global::System.IEquatable<{typeName}>");
                writer.AppendOpenBracket();

                BuildEquals(symbol, attributesMetadata, writer, explicitMode, ignoreInheritedMembers);

                writer.WriteLine();
                
                BuildGetHashCode(symbol, attributesMetadata, writer, explicitMode, ignoreInheritedMembers);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}