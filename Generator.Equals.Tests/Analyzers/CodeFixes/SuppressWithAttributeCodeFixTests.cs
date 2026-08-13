extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;
using GeneratorEquals::Generator.Equals.Analyzers.CodeFixes;

namespace Generator.Equals.Tests.Analyzers.CodeFixes;

/// <summary>
/// Tests for SuppressWithAttributeCodeFix.
/// </summary>
public sealed class SuppressWithAttributeCodeFixTests : CodeFixTestBase<EquatableAnalyzer, SuppressWithAttributeCodeFix>
{
    [Fact]
    public async Task GE002_AddsSuppressMessage()
    {
        const string source = """
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                public Address HomeAddress { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Generator.Equals", "GE002:Complex object property type lacks [Equatable]")]
                public Address HomeAddress { get; set; }
            }
            """;

        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(11, 12, 11, 19)
                .WithArguments("HomeAddress", "Address"));
    }

    [Fact]
    public async Task GE001_AddsSuppressMessage()
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
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Generator.Equals", "GE001:Collection property missing equality attribute")]
                public List<int> Items { get; set; }
            }
            """;

        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(7, 12, 7, 21)
                .WithArguments("Items"));
    }

    [Fact]
    public async Task GE003_AddsSuppressMessage()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Item { public string Name { get; set; } }

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public List<Item> Items { get; set; }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Item { public string Name { get; set; } }

            [Equatable]
            public partial class Sample
            {
                [System.Diagnostics.CodeAnalysis.SuppressMessage("Generator.Equals", "GE003:Collection element type lacks [Equatable]")]
                [OrderedEquality]
                public List<Item> Items { get; set; }
            }
            """;

        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.CollectionElementMissingEquatable)
                .WithSpan(10, 12, 10, 22)
                .WithArguments("Items", "Item"));
    }
}
