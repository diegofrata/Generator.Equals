using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class RecordEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            int level,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var symbolName = symbol.ToFQF();
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(level, InheritDocComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, symbol.IsSealed
                ? $"public bool Equals({symbolName}? other)"
                : $"public virtual bool Equals({symbolName}? other)");
            sb.AppendOpenBracket(ref level);

            sb.AppendLine(level, "return");
            level++;

            sb.AppendLine(level, baseTypeName == "object" || ignoreInheritedMembers
                ? "!ReferenceEquals(other, null) && EqualityContract == other.EqualityContract"
                : $"base.Equals(({baseTypeName}?)other)");

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyEquality(attributesMetadata, sb, level, property, explicitMode);
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
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(level, InheritDocComment);
            sb.AppendLine(level, GeneratedCodeAttributeDeclaration);
            sb.AppendLine(level, @"public override int GetHashCode()");
            sb.AppendOpenBracket(ref level);
            sb.AppendLine(level, @"var hashCode = new global::System.HashCode();");
            sb.AppendLine(level);

            sb.AppendLine(level, baseTypeName == "object" || ignoreInheritedMembers
                ? "hashCode.Add(this.EqualityContract);"
                : "hashCode.Add(base.GetHashCode());");

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyHashCode(property, attributesMetadata, sb, level, explicitMode);
            }

            sb.AppendLine(level);
            sb.AppendLine(level, "return hashCode.ToHashCode();");
            sb.AppendCloseBracket(ref level);
        }

        public static string Generate(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: (sb, level) =>
            {
                BuildEquals(symbol, attributesMetadata, sb, level, explicitMode, ignoreInheritedMembers);

                sb.AppendLine(level);

                BuildGetHashCode(symbol, attributesMetadata, sb, level, explicitMode, ignoreInheritedMembers);
            });

            return code;
        }
    }
}