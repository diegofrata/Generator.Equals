using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class ClassEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb)
        {
            var symbolName = symbol.ToFQF();
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(EqualsOperatorCodeComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine($"public static bool operator ==({symbolName}? left, {symbolName}? right) => global::System.Collections.Generic.EqualityComparer<{symbolName}?>.Default.Equals(left, right);");

            sb.AppendLine(NotEqualsOperatorCodeComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine($"public static bool operator !=({symbolName}? left, {symbolName}? right) => !(left == right);");

            sb.AppendLine(InheritDocComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine($"public override bool Equals(object? obj) => Equals(obj as {symbolName});");

            sb.AppendLine(InheritDocComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine($"public bool Equals({symbolName}? other) {{");

            sb.AppendLine(baseTypeName == "object"
                ? "return !ReferenceEquals(other, null) && this.GetType() == other.GetType()"
                : $"return base.Equals(other as {baseTypeName})");

            foreach (var property in symbol.GetProperties())
            {
                BuildPropertyEquality(attributesMetadata, sb, property);
            }

            sb.AppendLine(";");
            sb.AppendLine("}");
        }

        static void BuildGetHashCode(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb)
        {
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(InheritDocComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine(@"public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            ");

            sb.AppendLine(baseTypeName == "object"
                ? "hashCode.Add(this.GetType());"
                : "hashCode.Add(base.GetHashCode());");

            foreach (var property in symbol.GetProperties())
            {
                BuildPropertyHashCode(property, attributesMetadata, sb);
            }

            sb.AppendLine("return hashCode.ToHashCode();");
            sb.AppendLine("}");
        }
        
        public static string Generate(ITypeSymbol symbol, AttributesMetadata attributesMetadata)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: false, content: sb =>
            {
                var typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                sb.AppendLine($"partial class {typeName} : global::System.IEquatable<{typeName}> {{");

                sb.AppendLine(EnableNullableContext);
                sb.AppendLine(SuppressObsoleteWarningsPragma);

                BuildEquals(symbol, attributesMetadata, sb);
                BuildGetHashCode(symbol, attributesMetadata, sb);

                sb.AppendLine(RestoreObsoleteWarningsPragma);
                sb.AppendLine(RestoreNullableContext);

                sb.AppendLine("}");
            });

            return code;
        }
    }
}
