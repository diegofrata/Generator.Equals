using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    sealed class ClassEqualityGenerator : EqualityGeneratorBase
    {
        /// <summary>
        /// The exact-runtime-type guard, enforced by the generated <c>Equals</c> itself unless a generated
        /// comparer root enforces it on the real runtime types even when reached through <c>base.Equals</c>.
        /// A hand-written base offers no such guarantee (a loose <c>obj is Animal a</c> accepts any
        /// subclass), so the check is kept in every other case. <c>Inequalities</c> mirrors it so a
        /// runtime-type mismatch is reported rather than silently yielding nothing.
        /// </summary>
        static TypeIdentityGuard? TypeIdentity(EqualityTypeModel model) =>
            model.IgnoreInheritedMembers || !model.BaseHasEquatable
                ? new TypeIdentityGuard("other.GetType() == this.GetType()", "x.GetType() != y.GetType()")
                : null;

        static void BuildDelegatingMethods(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var symbolName = model.Fullname;

            if (model.GenerateClassEqualityOperators)
            {
                // == operator
                writer.WriteLines(EqualsOperatorCodeComment);
                writer.WriteLine(GeneratedCodeAttributeDeclaration);
                writer.WriteLine($"public static bool operator ==({symbolName}? left, {symbolName}? right) =>");
                writer.WriteLine(1, "EqualityComparer.Default.Equals(left, right);");
                writer.WriteLine();

                // != operator
                writer.WriteLines(NotEqualsOperatorCodeComment);
                writer.WriteLine(GeneratedCodeAttributeDeclaration);
                writer.WriteLine($"public static bool operator !=({symbolName}? left, {symbolName}? right) =>");
                writer.WriteLine(1, "!EqualityComparer.Default.Equals(left, right);");
                writer.WriteLine();
            }

            // Equals(object?)
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine("public override bool Equals(object? obj) =>");
            writer.WriteLine(1, $"Equals(obj as {symbolName});");
            writer.WriteLine();

            // IEquatable<T>.Equals(T?) - explicit interface implementation
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"bool global::System.IEquatable<{symbolName}>.Equals({symbolName}? obj) => Equals((object?) obj);");
            writer.WriteLine();
        }

        static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var symbolName = model.Fullname;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"{(model.IsSealed ? "private" : "protected")} bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(null, other)) return false;");
            writer.WriteLine("if (ReferenceEquals(this, other)) return true;");
            writer.WriteLine();

            if (TypeIdentity(model) is { } guard)
            {
                writer.WriteLine($"return {guard.EqualsCondition}");
                writer.Indent++;
                if (DelegatesToBase(model))
                    writer.WriteLine($"&& base.Equals({model.BaseEqualsArgument})");
            }
            else
            {
                writer.WriteLine($"return base.Equals({model.BaseEqualsArgument})");
                writer.Indent++;
            }

            BuildMembersEquality(model.InheritedEqualityModels, writer, "this", "other");
            BuildMembersEquality(model.BuildEqualityModels, writer, "this", "other");
            writer.WriteLine(";");
            writer.Indent--;

            writer.AppendCloseBracket();
        }

        static void BuildGetHashCode(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(@"public override int GetHashCode()");
            writer.AppendOpenBracket();

            writer.WriteLine(@"var hashCode = new global::System.HashCode();");
            writer.WriteLine();

            // Mirror BuildEquals: chain through base.GetHashCode() when the base owns equality,
            // otherwise seed from the exact runtime type. Kept in lock-step with Equals so the
            // Equals/GetHashCode contract holds.
            writer.WriteLine(DelegatesToBase(model)
                ? "hashCode.Add(base.GetHashCode());"
                : "hashCode.Add(this.GetType());");

            BuildMembersHashCode(model.InheritedEqualityModels, writer, "this");
            BuildMembersHashCode(model.BuildEqualityModels, writer, "this");

            writer.WriteLine();
            writer.WriteLine("return hashCode.ToHashCode();");

            writer.AppendCloseBracket();
        }

        static void BuildNestedEqualityComparer(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var symbolName = model.Fullname;
            // Use 'new' to suppress CS0108 warning when hiding base class's EqualityComparer
            var newKeyword = model.BaseHasEquatable ? "new " : "";

            writer.WriteLine();
            writer.WriteLines(EqualityComparerCodeComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public {newKeyword}sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<{symbolName}>");
            writer.AppendOpenBracket();

            // Default instance
            writer.WriteLines(EqualityComparerDefaultCodeComment);
            writer.WriteLine("public static EqualityComparer Default { get; } = new EqualityComparer();");
            writer.WriteLine();

            // Equals(T?, T?) - delegates to the type's Equals method
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public bool Equals({symbolName}? x, {symbolName}? y)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(x, y)) return true;");
            writer.WriteLine("if (x is null || y is null) return false;");
            writer.WriteLine();
            // For non-sealed classes, dispatch through the virtual Equals(object?) so the runtime
            // (most-derived) type's equality is used when comparing via a base-class reference.
            // Calling x.Equals(y) would bind non-virtually to the protected Equals(T?) on the base
            // type, ignoring members declared on derived types (see issue #77). This also keeps the
            // comparer consistent with GetHashCode, which already dispatches virtually.
            writer.WriteLine($"return x.Equals({(model.IsSealed ? "y" : "(object?) y")});");

            writer.AppendCloseBracket();

            writer.WriteLine();

            // GetHashCode(T) - delegates to the type's GetHashCode method
            writer.WriteLine(InheritDocComment);
            writer.WriteLine($"public int GetHashCode({symbolName} obj)");
            writer.AppendOpenBracket();

            writer.WriteLine("return obj.GetHashCode();");

            writer.AppendCloseBracket();

            writer.WriteLine();

            // Inequalities method
            BuildInequalitiesMethod(model, writer, symbolName);

            writer.AppendCloseBracket();
        }

        static void BuildInequalitiesMethod(EqualityTypeModel model, IndentedTextWriter writer, string symbolName)
        {
            writer.WriteLines(InequalitiesMethodComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities({symbolName}? x, {symbolName}? y, global::Generator.Equals.MemberPath path = default)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(x, y)) yield break;");
            writer.WriteLine("if (x is null || y is null)");
            writer.AppendOpenBracket();
            writer.WriteLine("yield return new global::Generator.Equals.Inequality(path, x, y);");
            writer.WriteLine("yield break;");
            writer.AppendCloseBracket();
            writer.WriteLine();

            BuildTypeMismatchInequality(writer, TypeIdentity(model));

            BuildBaseInequalities(model, writer,
                memberLevel: !model.IgnoreInheritedMembers && model.BaseEquality == BaseEqualityOwnership.Comparer,
                coarse: UsesBaseEqualityBridge(model));

            BuildMembersInequalities(model.InheritedEqualityModels, writer, "x", "y");
            BuildMembersInequalities(model.BuildEqualityModels, writer, "x", "y");

            writer.AppendCloseBracket();
        }

        public static string Generate(EqualityTypeModel model)
        {
            var code = ContainingTypesBuilder.Build(model.ContainingSymbols, content: writer =>
            {
                writer.WriteLine($"partial class {model.TypeName} : global::System.IEquatable<{model.Fullname}>");
                writer.AppendOpenBracket();

                BuildDelegatingMethods(model, writer);

                BuildEquals(model, writer);

                writer.WriteLine();

                BuildGetHashCode(model, writer);

                if (UsesBaseEqualityBridge(model))
                    BuildBaseEqualityBridge(model, writer, model.BaseEqualsArgument!);

                BuildNestedEqualityComparer(model, writer);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}
