using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    internal class ClassEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            bool explicitMode,
            bool ignoreInheritedMembers)
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

            sb.AppendLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "return !ReferenceEquals(other, null) && this.GetType() == other.GetType()"
                : $"return base.Equals(other as {baseTypeName})");

            foreach (var property in symbol.GetProperties())
            {
                BuildPropertyEquality(attributesMetadata, sb, property, explicitMode);
            }

            sb.AppendLine(";");
            sb.AppendLine("}");
        }

        static void BuildGetHashCode(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            StringBuilder sb,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine(InheritDocComment);
            sb.AppendLine(GeneratedCodeAttributeDeclaration);
            sb.AppendLine(@"public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            ");

            sb.AppendLine(baseTypeName == "object" || ignoreInheritedMembers
                ? "hashCode.Add(this.GetType());"
                : "hashCode.Add(base.GetHashCode());");

            foreach (var property in symbol.GetProperties())
            {
                BuildPropertyHashCode(property, attributesMetadata, sb, explicitMode);
            }

            sb.AppendLine("return hashCode.ToHashCode();");
            sb.AppendLine("}");
        }
        
        public static string Generate(
            ITypeSymbol symbol,
            AttributesMetadata attributesMetadata,
            bool explicitMode,
            bool ignoreInheritedMembers)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: false, content: sb =>
            {
                var typeName = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                sb.AppendLine($"partial class {typeName} : global::System.IEquatable<{typeName}> {{");

                sb.AppendLine(EnableNullableContext);
                sb.AppendLine(SuppressObsoleteWarningsPragma);

                BuildEquals(symbol, attributesMetadata, sb, explicitMode, ignoreInheritedMembers);
                BuildGetHashCode(symbol, attributesMetadata, sb, explicitMode, ignoreInheritedMembers);

                sb.AppendLine(RestoreObsoleteWarningsPragma);
                sb.AppendLine(RestoreNullableContext);

                sb.AppendLine("}");
            });

            return code;
        }
    }
}
