using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class RecordStructEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildNullableEquals(
            ITypeSymbol symbol,
            StringBuilder sb,
            int level)
        {
            var symbolName = symbol.ToFQF();

            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level,  $"public bool Equals({symbolName}? other)");
            sb.AppendOpenBracket(ref level);

            sb.AppendLine(level, "return other.HasValue && Equals(other.Value);");

            sb.AppendCloseBracket(ref level);
        }
        
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

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyEquality(attributesMetadata, sb, level, property, explicitMode,  false);
            }

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
            

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyHashCode(property, attributesMetadata, sb, level, explicitMode, false);
            }

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
                
                BuildNullableEquals(symbol, sb, level);

                sb.AppendLine(level);

                BuildGetHashCode(symbol, attributesMetadata, sb, level, explicitMode);
            });

            return code;
        }
    }
}