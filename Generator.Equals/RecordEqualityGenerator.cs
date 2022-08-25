using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class RecordEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb, bool explicitMode)
        {
            var symbolName = symbol.ToFQF();
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(InheritDocComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine(symbol.IsSealed
                ? $"public bool Equals({symbolName}? other) {{"
                : $"public virtual bool Equals({symbolName}? other) {{");

            sb.AppendLine(baseTypeName == "object"
                ? "return !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract"
                : $"return base.Equals(({baseTypeName}?)other)");

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyEquality(attributesMetadata, sb, property, explicitMode);
            }

            sb.AppendLine(";");
            sb.AppendLine("}");
        }

        static void BuildGetHashCode(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb, bool explicitMode)
        {
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(InheritDocComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine(@"public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            ");

            sb.AppendLine(baseTypeName == "object"
                ? "hashCode.Add(this.EqualityContract);"
                : "hashCode.Add(base.GetHashCode());");

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyHashCode(property, attributesMetadata, sb, explicitMode);
            }

            sb.AppendLine("return hashCode.ToHashCode();");
            sb.AppendLine("}");
        }

        public static string Generate(ITypeSymbol symbol, AttributesMetadata attributesMetadata, bool explicitMode)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: sb =>
            {
                sb.AppendLine(EnableNullableContext);
                sb.AppendLine(SuppressObsoleteWarningsPragma);
                
                BuildEquals(symbol, attributesMetadata, sb, explicitMode);
                BuildGetHashCode(symbol, attributesMetadata, sb, explicitMode);

                sb.AppendLine(RestoreObsoleteWarningsPragma);
                sb.AppendLine(RestoreNullableContext);
            });

            return code;
        }
    }
}
