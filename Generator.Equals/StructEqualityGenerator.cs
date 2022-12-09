using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class StructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            int level,
            bool explicitMode)
        {
            var symbolName = symbol.ToFQF();
            
            sb.AppendLine(level, EqualsOperatorCodeComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, "public static bool operator ==(");
            sb.AppendLine(level + 1, $"{symbolName} left,");
            sb.AppendLine(level + 1, $"{symbolName} right) =>");
            sb.AppendLine(level + 1, $"global::System.Collections.Generic.EqualityComparer<{symbolName}>.Default");
            sb.AppendLine(level + 2, $".Equals(left, right);");
            sb.AppendLine(level);

            sb.AppendLine(level, NotEqualsOperatorCodeComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, $"public static bool operator !=({symbolName} left, {symbolName} right) =>");
            sb.AppendLine(level + 1, "!(left == right);");
            sb.AppendLine(level);

            sb.AppendLine(level, InheritDocComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, "public override bool Equals(object? obj) =>");
            sb.AppendLine(level + 1, $"obj is {symbolName} o && Equals(o);");
            sb.AppendLine(level);

            sb.AppendLine(level, InheritDocComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, $"public bool Equals({symbolName} other)");
            sb.AppendOpenBracket(ref level);

            sb.AppendLine(level, "return true");
            level++;

            BuildMembersEquality(symbol, attributesMetadata, sb, level, explicitMode);

            sb.AppendLine(level, ";");
            level--;

            sb.AppendCloseBracket(ref level);
        }

        static void BuildGetHashCode(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            int level,
            bool explicitMode)
        {
            sb.AppendLine(level, InheritDocComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, @"public override int GetHashCode()");
            sb.AppendOpenBracket(ref level);

            sb.AppendLine(level, @"var hashCode = new global::System.HashCode();");
            sb.AppendLine(level);

            BuildMembersHashCode(symbol, attributesMetadata, sb, level, explicitMode);

            sb.AppendLine(level);
            sb.AppendLine(level, "return hashCode.ToHashCode();");

            sb.AppendCloseBracket(ref level);
        }
        
        public static string Generate(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            bool explicitMode)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: false, content: (sb, level) =>
            {
                var typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                sb.AppendLine(level, $"partial struct {typeName} : global::System.IEquatable<{typeName}>");
                sb.AppendOpenBracket(ref level);

                BuildEquals(symbol, attributesMetadata, sb, level, explicitMode);

                sb.AppendLine(level);
                
                BuildGetHashCode(symbol, attributesMetadata, sb, level, explicitMode);

                sb.AppendCloseBracket(ref level);
            });

            return code;
        }
    }
}