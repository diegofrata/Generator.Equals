extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE007: Conflicting attributes on same property.
/// </summary>
public sealed class GE007ConflictingAttributesTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task OrderedAndUnorderedEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                [UnorderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [UnorderedEquality] = 17 chars, start col 6, end col 23
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(8, 6, 8, 23)
                .WithArguments("Items", "OrderedEquality", "UnorderedEquality"));
    }

    [Fact]
    public async Task OrderedAndSetEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                [SetEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [SetEquality] = 11 chars, start col 6, end col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(8, 6, 8, 17)
                .WithArguments("Items", "OrderedEquality", "SetEquality"));
    }

    [Fact]
    public async Task UnorderedAndSetEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                [SetEquality]
                public HashSet<int> Items { get; set; }
            }
            """;

        // [SetEquality] = 11 chars, start col 6, end col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(8, 6, 8, 17)
                .WithArguments("Items", "UnorderedEquality", "SetEquality"));
    }

    [Fact]
    public async Task IgnoreAndOrderedEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [IgnoreEquality]
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(8, 6, 8, 21)
                .WithArguments("Items", "IgnoreEquality", "OrderedEquality"));
    }

    [Fact]
    public async Task IgnoreAndStringEquality_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [IgnoreEquality]
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public string Name { get; set; }
            }
            """;

        // [StringEquality(StringComparison.OrdinalIgnoreCase)] = 50 chars, start col 6, end col 56
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(8, 6, 8, 56)
                .WithArguments("Name", "IgnoreEquality", "StringEquality"));
    }

    [Fact]
    public async Task ReferenceAndOrderedEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [ReferenceEquality]
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(8, 6, 8, 21)
                .WithArguments("Items", "ReferenceEquality", "OrderedEquality"));
    }

    [Fact]
    public async Task StringAndCustomEquality_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                [CustomEquality(typeof(EqualityComparer<string>))]
                public string Name { get; set; }
            }
            """;

        // [CustomEquality(typeof(EqualityComparer<string>))] = 48 chars, start col 6, end col 54
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(9, 6, 9, 54)
                .WithArguments("Name", "StringEquality", "CustomEquality"));
    }

    [Fact]
    public async Task SingleAttribute_NoDiagnostic()
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
    public async Task DefaultAndCollectionEquality_NoDiagnostic()
    {
        // DefaultEquality doesn't conflict with collection equality attributes
        // This is because DefaultEquality marks a property for inclusion in explicit mode
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable(Explicit = true)]
            public partial class Sample
            {
                [DefaultEquality]
                [OrderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NoEquatableAttribute_NoDiagnostic()
    {
        // If the type doesn't have [Equatable], we report GE004 instead
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Sample
            {
                [OrderedEquality]
                [UnorderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // GE004 is reported instead of GE007
        // [OrderedEquality] = 15 chars, start col 6, end col 21
        // [UnorderedEquality] = 17 chars, start col 6, end col 23
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("Items", "OrderedEquality", "Sample"),
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(7, 6, 7, 23)
                .WithArguments("Items", "UnorderedEquality", "Sample"));
    }
}
