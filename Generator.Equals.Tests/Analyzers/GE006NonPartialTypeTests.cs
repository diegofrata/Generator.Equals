extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for GE006: [Equatable] on non-partial type.
/// </summary>
public sealed class GE006NonPartialTypeTests : AnalyzerTestBase<EquatableAnalyzer>
{
    [Fact]
    public async Task NonPartialClass_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 14, 4, 20)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task NonPartialStruct_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public struct Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 15, 4, 21)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task PartialClass_NoDiagnostic()
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
    public async Task PartialStruct_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public partial struct Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PartialRecord_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            namespace System.Runtime.CompilerServices { internal static class IsExternalInit { } }

            [Equatable]
            public partial record Sample(string Name);
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task PartialRecordStruct_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            namespace System.Runtime.CompilerServices { internal static class IsExternalInit { } }

            [Equatable]
            public partial record struct Sample(string Name);
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task NestedNonPartialClass_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public partial class Outer
            {
                [Equatable]
                public class Inner
                {
                    public string Name { get; set; }
                }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(6, 18, 6, 23)
                .WithArguments("Inner"));
    }

    [Fact]
    public async Task NestedPartialClass_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            public partial class Outer
            {
                [Equatable]
                public partial class Inner
                {
                    public string Name { get; set; }
                }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task SealedNonPartialClass_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public sealed class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 21, 4, 27)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task SealedPartialClass_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public sealed partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task AbstractNonPartialClass_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public abstract class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 23, 4, 29)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task AbstractPartialClass_NoDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            public abstract partial class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyNoDiagnosticAsync(source);
    }

    [Fact]
    public async Task InternalNonPartialClass_ReportsDiagnostic()
    {
        const string source = """
            using Generator.Equals;

            [Equatable]
            internal class Sample
            {
                public string Name { get; set; }
            }
            """;

        await VerifyDiagnosticAsync(source,
            Diagnostic(DiagnosticDescriptors.NonPartialType)
                .WithSpan(4, 16, 4, 22)
                .WithArguments("Sample"));
    }

    [Fact]
    public async Task WithoutEquatable_NoDiagnostic()
    {
        const string source = """
            public class Sample
            {
                public string Name { get; set; }
            }
            """;

        // No diagnostic since type doesn't have [Equatable]
        await VerifyNoDiagnosticAsync(source);
    }
}
