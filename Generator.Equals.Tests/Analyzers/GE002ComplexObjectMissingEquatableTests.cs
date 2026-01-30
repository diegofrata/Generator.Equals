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
    public async Task NullableEnumProperty_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public enum SelectionId { None, First, Second }

            [Equatable]
            public partial class Sample
            {
                public SelectionId? Id { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task RecordStructWithOnlyValueTypes_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            namespace System.Runtime.CompilerServices { internal static class IsExternalInit { } }

            public readonly record struct Point(int X, int Y);

            public readonly record struct Rectangle(Point TopLeft, Point BottomRight);

            [Equatable]
            public partial class Shape
            {
                public Rectangle Bounds { get; set; }
            }
            """;

        // Record structs with only value types have deep value equality - no diagnostic needed
        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task RecordStructWithCollection_ReportsDiagnostic()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            namespace System.Runtime.CompilerServices { internal static class IsExternalInit { } }

            public readonly record struct Container(string Name, List<int> Items);

            [Equatable]
            public partial class Sample
            {
                public Container Data { get; set; }
            }
            """;

        // Record struct contains a collection which uses reference equality - should warn
        // "Container" = 9 chars, starts at col 12, ends at col 21
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(11, 12, 11, 21)
                .WithArguments("Data", "Container"));
    }

    [Fact]
    public async Task NestedRecordStructsWithOnlyValueTypes_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            namespace System.Runtime.CompilerServices { internal static class IsExternalInit { } }

            public enum Side { Buy, Sell }

            public readonly record struct RunnerKey(long Id, string Name);

            public readonly record struct MatchedOrder(
                string OrderId,
                RunnerKey RunnerKey,
                Side Side,
                decimal Price,
                decimal Size);

            [Equatable]
            public partial class Sample
            {
                public MatchedOrder Order { get; set; }
            }
            """;

        // Deeply nested record structs with only value types - no diagnostic needed
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
    public async Task StructWithOnlyValueTypes_NoDiagnostic()
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

        // Structs with only value types have value equality via ValueType.Equals
        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task StructWithClassField_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public class Label
            {
                public string Text { get; set; }
            }

            public struct PointWithLabel
            {
                public int X { get; set; }
                public int Y { get; set; }
                public Label Tag { get; set; }
            }

            [Equatable]
            public partial class Shape
            {
                public PointWithLabel Center { get; set; }
            }
            """;

        // Struct contains a class field which uses reference equality - should warn
        // "PointWithLabel" = 14 chars, starts at col 12, ends at col 26
        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(18, 12, 18, 26)
                .WithArguments("Center", "PointWithLabel"));
    }
}
