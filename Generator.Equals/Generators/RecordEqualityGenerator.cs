using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    class RecordEqualityGenerator : EqualityGeneratorBase
    {
        /// <summary>
        /// The <c>EqualityContract</c> guard, enforced by the generated <c>Equals</c> itself when it does not
        /// chain to a base (a root record, or <c>IgnoreInheritedMembers</c>); a base record's <c>Equals</c>
        /// enforces it on the real runtime types otherwise. <c>Inequalities</c> mirrors it so a contract
        /// mismatch is reported rather than silently yielding nothing.
        /// </summary>
        static TypeIdentityGuard? TypeIdentity(EqualityTypeModel model) =>
            DelegatesToBase(model)
                ? null
                : new TypeIdentityGuard("EqualityContract == other.EqualityContract", "x.EqualityContract != y.EqualityContract");

        static void BuildEquals(
            EqualityTypeModel model,
            IndentedTextWriter writer
        )
        {
            var symbolName = model.Fullname;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine(model.IsSealed
                ? $"public bool Equals({symbolName}? other)"
                : $"public virtual bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("return");

            writer.Indent++;

            // A record base always owns equality (compiler-synthesized or hand-written), so chain through
            // base.Equals(); only a root (or IgnoreInheritedMembers) enforces the contract guard itself.
            if (TypeIdentity(model) is { } guard)
            {
                writer.WriteLine($"!ReferenceEquals(other, null) && {guard.EqualsCondition}");
            }
            else
            {
                writer.WriteLine($"base.Equals({model.BaseEqualsArgument})");
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

            // Mirror BuildEquals: chain through base.GetHashCode() when delegating, otherwise seed from
            // the contract. Kept in lock-step with Equals so the Equals/GetHashCode contract holds.
            writer.WriteLine(DelegatesToBase(model)
                ? "hashCode.Add(base.GetHashCode());"
                : "hashCode.Add(this.EqualityContract);");

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
            writer.WriteLine("return x.Equals(y);");

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
                BuildEquals(model, writer);

                writer.WriteLine();

                BuildGetHashCode(model, writer);

                // A record base without its own comparer is opaque to the nested comparer's Inequalities;
                // the bridge lets it ask base.Equals instead. A record binds to its typed Equals directly.
                if (UsesBaseEqualityBridge(model))
                    BuildBaseEqualityBridge(model, writer, "other");

                BuildNestedEqualityComparer(model, writer);
            });

            return code;
        }
    }
}
