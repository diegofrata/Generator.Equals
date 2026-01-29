extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE003: Collection element type lacks [Equatable].
/// </summary>
public sealed class GE003CollectionElementMissingEquatableTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task ListOfComplexType_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                [OrderedEquality]
                public List<Address> Addresses { get; set; }
            }
            """;

        // "List<Address>" = 13 chars, starts at col 12, ends at col 25
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionElementMissingEquatable)
                .WithSpan(13, 12, 13, 25)
                .WithArguments("Addresses", "Address"));
    }

    [Fact]
    public async Task ArrayOfComplexType_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Item
            {
                public string Name { get; set; }
            }

            [Equatable]
            public partial class Container
            {
                [OrderedEquality]
                public Item[] Items { get; set; }
            }
            """;

        // "Item[]" = 6 chars, starts at col 12, ends at col 18
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionElementMissingEquatable)
                .WithSpan(12, 12, 12, 18)
                .WithArguments("Items", "Item"));
    }

    [Fact]
    public async Task ListOfComplexType_WithEquatable_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                [OrderedEquality]
                public List<Address> Addresses { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task ListOfPrimitiveType_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public List<int> Numbers { get; set; }

                [OrderedEquality]
                public List<string> Names { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task ListOfEnum_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public enum Status { Active, Inactive }

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public List<Status> Statuses { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionWithIgnoreEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                [IgnoreEquality]
                public List<Address> Addresses { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task CollectionWithReferenceEquality_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                [ReferenceEquality]
                public List<Address> Addresses { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task SetOfComplexType_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Tag
            {
                public string Name { get; set; }
            }

            [Equatable]
            public partial class Article
            {
                [SetEquality]
                public HashSet<Tag> Tags { get; set; }
            }
            """;

        // "HashSet<Tag>" = 12 chars, starts at col 12, ends at col 24
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionElementMissingEquatable)
                .WithSpan(13, 12, 13, 24)
                .WithArguments("Tags", "Tag"));
    }

    [Fact]
    public async Task CollectionWithoutAttribute_ReportsBothGE001AndGE003()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                public List<Address> Addresses { get; set; }
            }
            """;

        // "List<Address>" = 13 chars, starts at col 12, ends at col 25
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(12, 12, 12, 25)
                .WithArguments("Addresses"),
            Diagnostic(DiagnosticDescriptors.CollectionElementMissingEquatable)
                .WithSpan(12, 12, 12, 25)
                .WithArguments("Addresses", "Address"));
    }
}
