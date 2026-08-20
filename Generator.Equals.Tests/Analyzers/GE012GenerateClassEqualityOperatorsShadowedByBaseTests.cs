extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE012: [Equatable(GenerateClassEqualityOperators = false)] on a derived class whose
/// base type already contributes == and != to overload resolution.
/// </summary>
public sealed class GE012GenerateClassEqualityOperatorsShadowedByBaseTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task OptOutOnRootClass_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Sample
            {
                public int Value { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task OptOutOnBothBaseAndDerived_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Base
            {
                public int Age { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Derived : Base
            {
                public string Department { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task OptOutWithPlainBaseClass_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Base
            {
                public int Age { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Derived : Base
            {
                public string Department { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task OptInOnDerived_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Base
            {
                public int Age { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = true)]
            public partial class Derived : Base
            {
                public string Department { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task OptOutOnEveryClassInThreeLevelChain_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Root
            {
                public int Age { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Middle : Root
            {
                public string Department { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Leaf : Middle
            {
                public string Title { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task OptOutOnDerivedWithEquatableBase_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Base
            {
                public int Age { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Derived : Base
            {
                public string Department { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsShadowedByBase)
                .WithSpan(9, 2, 9, 51)
                .WithArguments("Derived", "Base"));
    }

    [Fact]
    public async Task OptOutSkipsOptedOutParentAndReportsGrandparent()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Root
            {
                public int Age { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Middle : Root
            {
                public string Department { get; set; }
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Leaf : Middle
            {
                public string Title { get; set; }
            }
            """;

        // Middle's own opt-out is defeated by Root, and Leaf's by Root as well.
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsShadowedByBase)
                .WithSpan(9, 2, 9, 51)
                .WithArguments("Middle", "Root"),
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsShadowedByBase)
                .WithSpan(15, 2, 15, 51)
                .WithArguments("Leaf", "Root"));
    }

    [Fact]
    public async Task OptOutWithInapplicableBaseOperators_NoDiagnostic()
    {
        // Base's == cannot bind to two Derived operands, so it does not defeat the opt-out.
        const string source = """
            using Generator.Equals;

            public class Base
            {
                public int Age { get; set; }

                public static bool operator ==(Base left, string right) => false;
                public static bool operator !=(Base left, string right) => true;

                public override bool Equals(object obj) => ReferenceEquals(this, obj);
                public override int GetHashCode() => 0;
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Derived : Base
            {
                public string Department { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task OptOutWithHandWrittenBaseOperators_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Base
            {
                public int Age { get; set; }

                public static bool operator ==(Base left, Base right) => ReferenceEquals(left, right);
                public static bool operator !=(Base left, Base right) => !ReferenceEquals(left, right);

                public override bool Equals(object obj) => ReferenceEquals(this, obj);
                public override int GetHashCode() => 0;
            }

            [Equatable(GenerateClassEqualityOperators = false)]
            public partial class Derived : Base
            {
                public string Department { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.GenerateClassEqualityOperatorsShadowedByBase)
                .WithSpan(14, 2, 14, 51)
                .WithArguments("Derived", "Base"));
    }
}
