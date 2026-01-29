extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;
using GeneratorEquals::Generator.Equals.Analyzers.CodeFixes;

namespace Generator.Equals.Tests.Analyzers.CodeFixes;

/// <summary>
/// Tests for MakeTypePartialCodeFix (fixes GE006).
/// </summary>
public sealed class MakeTypePartialCodeFixTests : CodeFixTestBase<EquatableAnalyzer, MakeTypePartialCodeFix>
{
    [Fact]
    public async Task NonPartialClass_AddsPartialModifier()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public class Sample
            {
                public string Name { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        // "Sample" = 6 chars, starts at col 14 (after "public class "), ends at col 20
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 14, 4, 20)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task NonPartialStruct_AddsPartialModifier()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public struct Sample
            {
                public string Name { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            public partial struct Sample
            {
                public string Name { get; set; }
            }
            """;

        // "Sample" = 6 chars, starts at col 15 (after "public struct "), ends at col 21
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 15, 4, 21)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task SealedNonPartialClass_AddsPartialModifier()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public sealed class Sample
            {
                public string Name { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            public sealed partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        // "Sample" = 6 chars, starts at col 21 (after "public sealed class "), ends at col 27
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 21, 4, 27)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task InternalNonPartialClass_AddsPartialModifier()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            internal class Sample
            {
                public string Name { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            internal partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        // "Sample" = 6 chars, starts at col 16 (after "internal class "), ends at col 22
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 16, 4, 22)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task NestedNonPartialClass_AddsPartialModifier()
    {
        const string source = """
            using Generator.Equals;

            public partial class Outer
            {
                [Equatable]
                public class Inner
                {
                    public string Name { get; set; }
                }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            public partial class Outer
            {
                [Equatable]
                public partial class Inner
                {
                    public string Name { get; set; }
                }
            }
            """;

        // "Inner" = 5 chars, starts at col 18 (after "    public class "), ends at col 23
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(6, 18, 6, 23)
                .WithArguments("Inner"));
    }
}
