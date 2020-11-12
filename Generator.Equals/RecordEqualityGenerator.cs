﻿using System.Text;
using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class RecordEqualityGenerator : EqualityGeneratorBase
    {
        static void BuildEquals(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb)
        {
            var symbolName = symbol.ToFQF();
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine("#nullable enable");

            sb.AppendLine(symbol.IsSealed
                ? $"public bool Equals({symbolName}? other) {{"
                : $"public virtual bool Equals({symbolName}? other) {{");

            sb.AppendLine(baseTypeName == "object"
                ? "return other is not null && EqualityContract == other.EqualityContract"
                : $"return base.Equals(({baseTypeName}?)other)");

            foreach (var property in symbol.GetProperties())
            {
                var propertyName = property.ToFQF();

                if (propertyName == "EqualityContract")
                    continue;

                BuildPropertyEquality(attributesMetadata, sb, property);
            }

            sb.AppendLine(";");
            sb.AppendLine("}");

            sb.AppendLine("#nullable restore");
        }

        static void BuildGetHashCode(ITypeSymbol symbol, AttributesMetadata attributesMetadata, StringBuilder sb)
        {
            var baseTypeName = symbol.BaseType?.ToFQF();

            sb.AppendLine("#nullable enable");

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

                BuildPropertyHashCode(property, attributesMetadata, sb);
            }

            sb.AppendLine("return hashCode.ToHashCode();");
            sb.AppendLine("}");

            sb.AppendLine("#nullable restore");
        }

        public static string Generate(ITypeSymbol symbol, AttributesMetadata attributesMetadata)
        {
            var code = ContainingTypesBuilder.Build(symbol, includeSelf: true, content: sb =>
            {
                BuildEquals(symbol, attributesMetadata, sb);

                BuildGetHashCode(symbol, attributesMetadata, sb);
            });

            return code;
        }
    }
}