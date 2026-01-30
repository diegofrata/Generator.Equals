extern alias GeneratorEquals;

using GeneratorEquals::Generator.Equals.Analyzers;
using GeneratorEquals::Generator.Equals.Analyzers.CodeFixes;

namespace Generator.Equals.Tests.Analyzers.CodeFixes;

/// <summary>
/// Tests for AddDefaultEqualityCodeFix.
/// </summary>
public sealed class AddDefaultEqualityCodeFixTests : CodeFixTestBase<EquatableAnalyzer, AddDefaultEqualityCodeFix>
{
    [Fact]
    public async Task GE002_AddsDefaultEquality()
    {
        const string source = """
            using Generator.Equals;

            public class ProtobufMessage
            {
                public string Value { get; set; }
                public override bool Equals(object obj) => obj is ProtobufMessage other && Value == other.Value;
                public override int GetHashCode() => Value?.GetHashCode() ?? 0;
            }

            [Equatable]
            public partial class Container
            {
                public ProtobufMessage Message { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            public class ProtobufMessage
            {
                public string Value { get; set; }
                public override bool Equals(object obj) => obj is ProtobufMessage other && Value == other.Value;
                public override int GetHashCode() => Value?.GetHashCode() ?? 0;
            }

            [Equatable]
            public partial class Container
            {
                [DefaultEquality]
                public ProtobufMessage Message { get; set; }
            }
            """;

        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(13, 12, 13, 27)
                .WithArguments("Message", "ProtobufMessage"));
    }

    [Fact]
    public async Task GE003_AddsDefaultEquality()
    {
        const string source = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class ProtobufMessage
            {
                public string Value { get; set; }
                public override bool Equals(object obj) => obj is ProtobufMessage other && Value == other.Value;
                public override int GetHashCode() => Value?.GetHashCode() ?? 0;
            }

            [Equatable]
            public partial class Container
            {
                [OrderedEquality]
                public List<ProtobufMessage> Messages { get; set; }
            }
            """;

        const string fixedSource = """
            using System.Collections.Generic;
            using Generator.Equals;

            public class ProtobufMessage
            {
                public string Value { get; set; }
                public override bool Equals(object obj) => obj is ProtobufMessage other && Value == other.Value;
                public override int GetHashCode() => Value?.GetHashCode() ?? 0;
            }

            [Equatable]
            public partial class Container
            {
                [OrderedEquality]
                [DefaultEquality]
                public List<ProtobufMessage> Messages { get; set; }
            }
            """;

        // "List<ProtobufMessage>" = 21 chars, starts at col 12, ends at col 33
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.CollectionElementMissingEquatable)
                .WithSpan(15, 12, 15, 33)
                .WithArguments("Messages", "ProtobufMessage"));
    }

    [Fact]
    public async Task GE002_AddsDefaultEquality_NullableType()
    {
        const string source = """
            using Generator.Equals;

            public class ExternalType
            {
                public int Id { get; set; }
            }

            [Equatable]
            public partial class Container
            {
                public ExternalType? Item { get; set; }
            }
            """;

        const string fixedSource = """
            using Generator.Equals;

            public class ExternalType
            {
                public int Id { get; set; }
            }

            [Equatable]
            public partial class Container
            {
                [DefaultEquality]
                public ExternalType? Item { get; set; }
            }
            """;

        // "ExternalType?" is the full type name including the nullable marker
        await VerifyCodeFixAsync(source, fixedSource,
            Diagnostic(DiagnosticDescriptors.ComplexTypeMissingEquatable)
                .WithSpan(11, 12, 11, 25)
                .WithArguments("Item", "ExternalType?"));
    }
}
