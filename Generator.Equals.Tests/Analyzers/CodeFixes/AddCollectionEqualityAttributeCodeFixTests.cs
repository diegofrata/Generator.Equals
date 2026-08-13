extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;
using GeneratorEquals::Generator.Equals.Analyzers.CodeFixes;

namespace Generator.Equals.Tests.Analyzers.CodeFixes;

/// <summary>
/// Tests for AddCollectionEqualityAttributeCodeFix (fixes GE001).
/// </summary>
public sealed class AddCollectionEqualityAttributeCodeFixTests : CodeFixTestBase<EquatableAnalyzer, AddCollectionEqualityAttributeCodeFix>
{
    [Fact]
    public async Task ListProperty_AddsOrderedEquality()
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

        // "List<int>" = 9 chars, starts at col 12, ends at col 21
        // Index 0 is the recommended option (OrderedEquality for List)
        await VerifyCodeFixAsync(source, fixedSource, 0,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task ListProperty_AddsUnorderedEquality()
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

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public List<int> Items { get; set; }
            }
            """;

        // "List<int>" = 9 chars, starts at col 12, ends at col 21
        // Index 1 is UnorderedEquality
        await VerifyCodeFixAsync(source, fixedSource, 1,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task HashSetProperty_AddsSetEquality()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public HashSet<int> Items { get; set; }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [SetEquality]
                public HashSet<int> Items { get; set; }
            }
            """;

        // "HashSet<int>" = 12 chars, starts at col 12, ends at col 24
        // Index 0 is the recommended option (SetEquality for HashSet)
        await VerifyCodeFixAsync(source, fixedSource, 0,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 24)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task DictionaryProperty_AddsUnorderedEquality()
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

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public Dictionary<string, int> Lookup { get; set; }
            }
            """;

        // "Dictionary<string, int>" = 23 chars, starts at col 12, ends at col 35
        // Index 0 is the recommended option (UnorderedEquality for Dictionary)
        await VerifyCodeFixAsync(source, fixedSource, 0,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 35)
                .WithArguments("Lookup"));
    }

    [Fact]
    public async Task ArrayProperty_AddsOrderedEquality()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public int[] Numbers { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public int[] Numbers { get; set; }
            }
            """;

        // "int[]" = 5 chars, starts at col 12, ends at col 17
        await VerifyCodeFixAsync(source, fixedSource, 0,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(6, 12, 6, 17)
                .WithArguments("Numbers"));
    }

    [Fact]
    public async Task PropertyWithExistingAttributes_AddsAttributeCorrectly()
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

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable(Explicit = true)]
            public partial class Sample
            {
                [OrderedEquality]
                [DefaultEquality]
                public List<int> Items { get; set; }
            }
            """;

        // "List<int>" = 9 chars, starts at col 12, ends at col 21
        await VerifyCodeFixAsync(source, fixedSource, 0,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(8, 12, 8, 21)
                .WithArguments("Items"));
    }
}
