extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE002: Complex object property type lacks [Equatable].
/// </summary>
public sealed class GE002ComplexObjectMissingEquatableTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task ComplexProperty_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Address
            {
                public string Street { get; set; }
                public string City { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                public string Name { get; set; }
                public Address HomeAddress { get; set; }
            }
            """;

        // "Address" = 7 chars, starts at col 12, ends at col 19
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(13, 12, 13, 19)
                .WithArguments("HomeAddress", "Address"));
    }

    [Fact]
    public async Task ComplexProperty_WithEquatable_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Address
            {
                public string Street { get; set; }
                public string City { get; set; }
            }

            [Equatable]
            public partial class Person
            {
                public string Name { get; set; }
                public Address HomeAddress { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PrimitiveProperty_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public int Value { get; set; }
                public string Name { get; set; }
                public bool IsActive { get; set; }
                public double Amount { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task EnumProperty_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public enum Status { Active, Inactive }

            [Equatable]
            public partial class Sample
            {
                public Status CurrentStatus { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NullablePrimitiveProperty_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public int? Value { get; set; }
                public bool? IsActive { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task DateTimeProperty_NoDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public DateTime Created { get; set; }
                public DateTimeOffset Modified { get; set; }
                public TimeSpan Duration { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task GuidProperty_NoDiagnostic()
    {
        const string source = """
            using System;
            using Generator.Equals;

            [Equatable]
            public partial class Sample
            {
                public Guid Id { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task ComplexProperty_WithIgnoreEquality_NoDiagnostic()
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
                public string Name { get; set; }

                [IgnoreEquality]
                public Address HomeAddress { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task ComplexProperty_WithReferenceEquality_NoDiagnostic()
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
                public string Name { get; set; }

                [ReferenceEquality]
                public Address HomeAddress { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StructProperty_WithoutEquatable_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public struct Point
            {
                public int X { get; set; }
                public int Y { get; set; }
            }

            [Equatable]
            public partial class Shape
            {
                public Point Center { get; set; }
            }
            """;

        // "Point" = 5 chars, starts at col 12, ends at col 17
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(12, 12, 12, 17)
                .WithArguments("Center", "Point"));
    }
}
