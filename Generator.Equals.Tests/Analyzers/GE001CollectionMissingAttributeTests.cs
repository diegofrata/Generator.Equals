extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE001: Collection property missing equality attribute.
/// </summary>
public sealed class GE001CollectionMissingAttributeTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task ListProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public List<int> Items { get; set; }
            }
            """;

        // List<int> is 9 chars, starts at col 12, ends at col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task ArrayProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public int[] Numbers { get; set; }
            }
            """;

        // int[] is 5 chars, starts at col 12, ends at col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(6, 12, 6, 17)
                .WithArguments("Numbers"));
    }

    [Fact]
    public async Task CollectionProperty_WithOrderedEquality_NoDiagnostic()
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
    public async Task CollectionProperty_WithUnorderedEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithSetEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [SetEquality]
                public HashSet<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithIgnoreEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [IgnoreEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionProperty_WithReferenceEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [ReferenceEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task ExplicitMode_CollectionWithDefaultEquality_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable(Explicit = true)]
            public partial class Sample
            {
                [DefaultEquality]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(8, 12, 8, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task ExplicitMode_CollectionWithoutDefaultEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable(Explicit = true)]
            public partial class Sample
            {
                public List<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StringProperty_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task DictionaryProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public Dictionary<string, int> Lookup { get; set; }
            }
            """;

        // Dictionary<string, int> is 23 chars, starts at col 12, ends at col 35
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 35)
                .WithArguments("Lookup"));
    }

    [Fact]
    public async Task IEnumerableProperty_WithoutAttribute_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public IEnumerable<int> Items { get; set; }
            }
            """;

        // IEnumerable<int> is 16 chars, starts at col 12, ends at col 28
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 28)
                .WithArguments("Items"));
    }
}
