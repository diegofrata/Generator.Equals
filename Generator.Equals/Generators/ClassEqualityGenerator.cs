using System.CodeDom.Compiler;

using Generator.Equals.Models;

namespace Generator.Equals.Generators
{
    sealed class ClassEqualityGenerator : EqualityGeneratorBase
    {
        /// <summary>
        /// Whether the generated equality should chain through <c>base.Equals()</c>/<c>base.GetHashCode()</c>.
        /// True when the base owns equality — via [Equatable]/a generated comparer, or a hand-written
        /// complete contract — and inherited members are not being ignored.
        /// </summary>
        static bool DelegatesToBase(EqualityTypeModel model) =>
            model.BaseTypeName != "object"
            && !model.IgnoreInheritedMembers
            && (model.BaseHasEquatable || model.BaseHasManualEquality);

        /// <summary>
        /// Whether delegation targets a hand-written base with no comparer — the case that needs the
        /// <c>__BaseEquals</c> bridge. Ties bridge emission and its single use in <c>Inequalities</c>
        /// to one predicate so they cannot drift.
        /// </summary>
        static bool DelegatesToManualBase(EqualityTypeModel model) =>
            model.BaseTypeName != "object"
            && !model.IgnoreInheritedMembers
            && model.BaseHasManualEquality;

        /// <summary>
        /// Whether base delegation must route through <c>Equals(object)</c> (the fallback). True only for a
        /// hand-written base that exposes no public typed <c>Equals(TSelf)</c> to bind to. When false,
        /// delegation casts to the base type and binds to its typed <c>Equals</c> — the base's
        /// <c>IEquatable&lt;TSelf&gt;</c>, or (on the comparer path) the generated typed <c>Equals</c>.
        /// </summary>
        static bool DelegatesWithObjectCast(EqualityTypeModel model) =>
            model.BaseHasManualEquality && !model.ImmediateBaseHasTypedEquals;

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
            var baseTypeFullname = model.BaseTypeFullname;

            writer.WriteLine(InheritDocComment);
            writer.WriteLine(GeneratedCodeAttributeDeclaration);
            writer.WriteLine($"{(model.IsSealed ? "private" : "protected")} bool Equals({symbolName}? other)");
            writer.AppendOpenBracket();

            writer.WriteLine("if (ReferenceEquals(null, other)) return false;");
            writer.WriteLine("if (ReferenceEquals(this, other)) return true;");
            writer.WriteLine();

            // When the base owns equality (via [Equatable]/a generated comparer or a hand-written
            // complete contract), chain through base.Equals() so its semantics are honored. Otherwise
            // compare the exact runtime type; inherited members are carried by the collected models.
            //
            // Prefer the base's typed Equals: casting to the base type binds to its IEquatable<TSelf>
            // (or, on the comparer path, the generated typed Equals). Fall back to the object cast only
            // for a hand-written base that has no typed Equals — there, casting to the base type would
            // instead bind to an [Equatable] ancestor's generated protected Equals(TAncestor?) and skip
            // the hand-written type's own members.
            if (DelegatesToBase(model))
            {
                writer.WriteLine(DelegatesWithObjectCast(model)
                    ? "return base.Equals((object?) other)"
                    : $"return base.Equals(other as {baseTypeFullname})");
            }
            else
            {
                writer.WriteLine("return other.GetType() == this.GetType()");
            }

            writer.Indent++;
            // Include inherited members (when no ancestor has [Equatable])
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

            // Include inherited members (when no ancestor has [Equatable])
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

            if (!model.IgnoreInheritedMembers && model.BaseHasEquatable && !model.BaseHasManualEquality)
            {
                // The immediate base owns its comparer (no hand-written contract in between): delegate
                // member-level detail. Excluding BaseHasManualEquality guards the ComparerBehindManual
                // case, where the inherited comparer would silently skip the manual intermediate's members.
                BuildBaseComparerInequalityDelegation(model, writer);
                writer.WriteLine();
            }
            else if (DelegatesToManualBase(model))
            {
                // A hand-written contract in the delegated base chain is opaque to (inherited) comparers,
                // so report one coarse inequality for the whole base portion when base.Equals (via the
                // bridge) disagrees — keeping Inequalities consistent with Equals.
                BuildCoarseBaseInequality(writer);
                writer.WriteLine();
            }

            // Include inherited members (when no ancestor has [Equatable])
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

                // Only a hand-written base (no comparer to inherit) needs the bridge; an [Equatable]
                // base is reached through its (possibly inherited) generated comparer instead.
                if (DelegatesToManualBase(model))
                {
                    // Mirror BuildEquals' cast choice: prefer the base's typed Equals, fall back to
                    // Equals(object) only when the base exposes no typed Equals.
                    BuildBaseEqualityBridge(model, writer, castArgumentToObject: DelegatesWithObjectCast(model));
                }

                BuildNestedEqualityComparer(model, writer);

                writer.AppendCloseBracket();
            });

            return code;
        }
    }
}
