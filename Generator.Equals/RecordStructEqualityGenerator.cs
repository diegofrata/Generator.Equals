using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class RecordStructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            int level,
            bool explicitMode)
        {
            var symbolName = symbol.ToFQF();

            sb.AppendLine(level, InheritDocComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level,  $"public bool Equals({symbolName} other)");
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
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: (sb, level) =>
            {
                BuildEquals(symbol, attributesMetadata, sb, level, explicitMode);

                sb.AppendLine(level);

                BuildGetHashCode(symbol, attributesMetadata, sb, level, explicitMode);
            });

            return code;
        }
    }
}