extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;
using GeneratorEquals::Generator.Equals.Analyzers.CodeFixes;

namespace Generator.Equals.Tests.Analyzers.CodeFixes;

/// <summary>
/// Tests for AddEquatableAttributeCodeFix (fixes GE004).
/// </summary>
public sealed class AddEquatableAttributeCodeFixTests : CodeFixTestBase<EquatableAnalyzer, AddEquatableAttributeCodeFix>
{
    [Fact]
    public async Task OrphanedAttribute_AddsEquatableAndPartial()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Sample
            {
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("Items", "OrderedEquality", "Sample"));
    }

    [Fact]
    public async Task OrphanedAttribute_OnPartialClass_OnlyAddsEquatable()
    {
        const string source = """
            using Generator.Equals;

            public partial class Sample
            {
                [IgnoreEquality]
                public string Secret { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [IgnoreEquality]
                public string Secret { get; set; }
            }
            """;

        // [IgnoreEquality] = 14 chars, start col 6, end col 20
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 20)
                .WithArguments("Secret", "IgnoreEquality", "Sample"));
    }

    [Fact]
    public async Task OrphanedAttribute_OnStruct_AddsEquatableAndPartial()
    {
        const string source = """
            using Generator.Equals;

            public struct Sample
            {
                [IgnoreEquality]
                public string Value { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            public partial struct Sample
            {
                [IgnoreEquality]
                public string Value { get; set; }
            }
            """;

        // [IgnoreEquality] = 14 chars, start col 6, end col 20
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 20)
                .WithArguments("Value", "IgnoreEquality", "Sample"));
    }

    [Fact]
    public async Task OrphanedAttribute_OnNestedClass_AddsEquatableAndPartial()
    {
        const string source = """
            using Generator.Equals;

            public class Outer
            {
                public class Inner
                {
                    [IgnoreEquality]
                    public string Value { get; set; }
                }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            public class Outer
            {
                [Equatable]
                public partial class Inner
                {
                    [IgnoreEquality]
                    public string Value { get; set; }
                }
            }
            """;

        // [IgnoreEquality] = 14 chars, start col 10 (nested), end col 24
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(7, 10, 7, 24)
                .WithArguments("Value", "IgnoreEquality", "Inner"));
    }

    [Fact]
    public async Task OrphanedAttribute_WithStringEquality_AddsEquatableAndPartial()
    {
        const string source = """
            using System;
            using Generator.Equals;

            public class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public string Name { get; set; }
            }
            """;

        const string fixedSource = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public string Name { get; set; }
            }
            """;

        // [StringEquality(StringComparison.OrdinalIgnoreCase)] = 50 chars, start col 6, end col 56
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 56)
                .WithArguments("Name", "StringEquality", "Sample"));
    }
}
