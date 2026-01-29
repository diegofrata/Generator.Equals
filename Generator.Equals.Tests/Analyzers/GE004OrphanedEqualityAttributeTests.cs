extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE004: Equality attribute used but containing type lacks [Equatable].
/// </summary>
public sealed class GE004OrphanedEqualityAttributeTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task OrderedEquality_WithoutEquatable_ReportsDiagnostic()
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

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("Items", "OrderedEquality", "Sample"));
    }

    [Fact]
    public async Task UnorderedEquality_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Sample
            {
                [UnorderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [UnorderedEquality] = 17 chars, start col 6, end col 23
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 23)
                .WithArguments("Items", "UnorderedEquality", "Sample"));
    }

    [Fact]
    public async Task SetEquality_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Sample
            {
                [SetEquality]
                public HashSet<int> Items { get; set; }
            }
            """;

        // [SetEquality] = 11 chars, start col 6, end col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 17)
                .WithArguments("Items", "SetEquality", "Sample"));
    }

    [Fact]
    public async Task StringEquality_WithoutEquatable_ReportsDiagnostic()
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

        // [StringEquality(StringComparison.OrdinalIgnoreCase)] = 50 chars, start col 6, end col 56
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 56)
                .WithArguments("Name", "StringEquality", "Sample"));
    }

    [Fact]
    public async Task IgnoreEquality_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Sample
            {
                [IgnoreEquality]
                public string Secret { get; set; }
            }
            """;

        // [IgnoreEquality] = 14 chars, start col 6, end col 20
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 20)
                .WithArguments("Secret", "IgnoreEquality", "Sample"));
    }

    [Fact]
    public async Task ReferenceEquality_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Other { }

            public class Sample
            {
                [ReferenceEquality]
                public Other Reference { get; set; }
            }
            """;

        // [ReferenceEquality] = 17 chars, start col 6, end col 23
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(7, 6, 7, 23)
                .WithArguments("Reference", "ReferenceEquality", "Sample"));
    }

    [Fact]
    public async Task DefaultEquality_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Sample
            {
                [DefaultEquality]
                public int Value { get; set; }
            }
            """;

        // [DefaultEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 21)
                .WithArguments("Value", "DefaultEquality", "Sample"));
    }

    [Fact]
    public async Task CustomEquality_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Sample
            {
                [CustomEquality(typeof(EqualityComparer<int>))]
                public int Value { get; set; }
            }
            """;

        // [CustomEquality(typeof(EqualityComparer<int>))] = 45 chars, start col 6, end col 51
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 51)
                .WithArguments("Value", "CustomEquality", "Sample"));
    }

    [Fact]
    public async Task EqualityAttribute_WithEquatable_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task MultipleOrphanedAttributes_ReportsMultipleDiagnostics()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Sample
            {
                [OrderedEquality]
                public List<int> Items { get; set; }

                [IgnoreEquality]
                public string Secret { get; set; }
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        // [IgnoreEquality] = 14 chars, start col 6, end col 20
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("Items", "OrderedEquality", "Sample"),
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(9, 6, 9, 20)
                .WithArguments("Secret", "IgnoreEquality", "Sample"));
    }

    [Fact]
    public async Task NestedClass_WithoutEquatable_ReportsDiagnostic()
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

        // [IgnoreEquality] = 14 chars, start col 10, end col 24
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(7, 10, 7, 24)
                .WithArguments("Value", "IgnoreEquality", "Inner"));
    }

    [Fact]
    public async Task FieldWithEqualityAttribute_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Sample
            {
                [IgnoreEquality]
                public string _secret;
            }
            """;

        // [IgnoreEquality] = 14 chars, start col 6, end col 20
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 20)
                .WithArguments("_secret", "IgnoreEquality", "Sample"));
    }
}
