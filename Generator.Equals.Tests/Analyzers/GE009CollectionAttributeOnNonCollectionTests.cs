extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE009: Collection equality attribute on non-collection type.
/// </summary>
public sealed class GE009CollectionAttributeOnNonCollectionTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task OrderedEqualityOnInt_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public int Value { get; set; }
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("Value", "OrderedEquality", "int"));
    }

    [Fact]
    public async Task UnorderedEqualityOnString_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public string Name { get; set; }
            }
            """;

        // [UnorderedEquality] = 17 chars, start col 6, end col 23
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(6, 6, 6, 23)
                .WithArguments("Name", "UnorderedEquality", "string"));
    }

    [Fact]
    public async Task SetEqualityOnBool_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [SetEquality]
                public bool IsActive { get; set; }
            }
            """;

        // [SetEquality] = 11 chars, start col 6, end col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(6, 6, 6, 17)
                .WithArguments("IsActive", "SetEquality", "bool"));
    }

    [Fact]
    public async Task OrderedEqualityOnDateTime_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public DateTime Created { get; set; }
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(7, 6, 7, 21)
                .WithArguments("Created", "OrderedEquality", "DateTime"));
    }

    [Fact]
    public async Task OrderedEqualityOnComplexType_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public Address HomeAddress { get; set; }
            }
            """;

        // Also reports GE002 because Address is a complex type without [Equatable]
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(11, 6, 11, 21)
                .WithArguments("HomeAddress", "OrderedEquality", "Address"),
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(12, 12, 12, 19)
                .WithArguments("HomeAddress", "Address"));
    }

    [Fact]
    public async Task OrderedEqualityOnList_NoDiagnostic()
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
    public async Task UnorderedEqualityOnArray_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public int[] Numbers { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task SetEqualityOnHashSet_NoDiagnostic()
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
    public async Task OrderedEqualityOnIEnumerable_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public IEnumerable<int> Items { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task UnorderedEqualityOnDictionary_NoDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [UnorderedEquality]
                public Dictionary<string, int> Lookup { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NoEquatableAttribute_ReportsGE004InsteadOfGE009()
    {
        const string source = """
            using Generator.Equals;

            public class Sample
            {
                [OrderedEquality]
                public int Value { get; set; }
            }
            """;

        // Only GE004 is reported when the type doesn't have [Equatable]
        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(5, 6, 5, 21)
                .WithArguments("Value", "OrderedEquality", "Sample"));
    }

    [Fact]
    public async Task CollectionAttributeOnField_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                public int _value;
            }
            """;

        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("_value", "OrderedEquality", "int"));
    }

    [Fact]
    public async Task MultipleCollectionAttributesOnNonCollection_ReportsMultipleDiagnostics()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [OrderedEquality]
                [UnorderedEquality]
                public int Value { get; set; }
            }
            """;

        // Reports GE007 for conflict plus GE009 for each
        // [UnorderedEquality] = 17 chars, start col 6, end col 23
        // [OrderedEquality] = 15 chars, start col 6, end col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ConflictingAttributes)
                .WithSpan(7, 6, 7, 23)
                .WithArguments("Value", "OrderedEquality", "UnorderedEquality"),
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(6, 6, 6, 21)
                .WithArguments("Value", "OrderedEquality", "int"),
            Diagnostic(DiagnosticDescriptors.CollectionAttributeOnNonCollection)
                .WithSpan(7, 6, 7, 23)
                .WithArguments("Value", "UnorderedEquality", "int"));
    }
}
