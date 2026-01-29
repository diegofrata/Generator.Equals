extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE008: [StringEquality] on non-string type.
/// </summary>
public sealed class GE008StringEqualityOnNonStringTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task StringEqualityOnInt_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public int Value { get; set; }
            }
            """;

        // [StringEquality(StringComparison.OrdinalIgnoreCase)] = 50 chars, start col 6, end col 56
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(7, 6, 7, 56)
                .WithArguments("Value", "int"));
    }

    [Fact]
    public async Task StringEqualityOnBool_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public bool IsActive { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(7, 6, 7, 56)
                .WithArguments("IsActive", "bool"));
    }

    [Fact]
    public async Task StringEqualityOnDateTime_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public DateTime Created { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(7, 6, 7, 56)
                .WithArguments("Created", "DateTime"));
    }

    [Fact]
    public async Task StringEqualityOnCollection_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public List<string> Names { get; set; }
            }
            """;

        // Also reports GE001 because collection is missing collection equality attribute
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(8, 6, 8, 56)
                .WithArguments("Names", "List<string>"),
            Diagnostic(DiagnosticDescriptors.CollectionMissingAttribute)
                .WithSpan(9, 12, 9, 24)
                .WithArguments("Names"));
    }

    [Fact]
    public async Task StringEqualityOnObject_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public object Data { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(7, 6, 7, 56)
                .WithArguments("Data", "object"));
    }

    [Fact]
    public async Task StringEqualityOnComplexType_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
            }

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public Address HomeAddress { get; set; }
            }
            """;

        // Also reports GE002 because Address is a complex type without [Equatable]
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(12, 6, 12, 56)
                .WithArguments("HomeAddress", "Address"),
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(13, 12, 13, 19)
                .WithArguments("HomeAddress", "Address"));
    }

    [Fact]
    public async Task StringEqualityOnString_NoDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StringEqualityOnNullableString_NoDiagnostic()
    {
        const string source = """
            #nullable enable
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public string? Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NoEquatableAttribute_ReportsGE004InsteadOfGE008()
    {
        const string source = """
            using System;
            using Generator.Equals;

            public class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public int Value { get; set; }
            }
            """;

        // Only GE004 is reported when the type doesn't have [Equatable]
        // [StringEquality(StringComparison.OrdinalIgnoreCase)] = 50 chars, start col 6, end col 56
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.OrphanedEqualityAttribute)
                .WithSpan(6, 6, 6, 56)
                .WithArguments("Value", "StringEquality", "Sample"));
    }

    [Fact]
    public async Task StringEqualityOnField_ReportsDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                [StringEquality(StringComparison.OrdinalIgnoreCase)]
                public int _value;
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.StringEqualityOnNonString)
                .WithSpan(7, 6, 7, 56)
                .WithArguments("_value", "int"));
    }
}
