extern alias GeneratorEquals;

using System.Collections.Immutable;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;

namespace Generator.Equals.Tests.Infrastructure;

/// <summary>
/// Guards against CS1591 ("missing XML comment for publicly visible type or member") being raised
/// on the generated code when the consuming project enables <c>GenerateDocumentationFile</c>. See issue #76.
/// </summary>
public class XmlDocumentationTests
{
    const string MissingXmlCommentWarning = "CS1591";

    static readonly MetadataReference RuntimeReference = MetadataReference.CreateFromFile(
        typeof(Generator.Equals.EquatableAttribute).Assembly.Location);

    /// <summary>
    /// Every public member emitted by the generator must carry an XML doc comment, for all supported type kinds
    /// and for both root and derived types (derived types emit a slightly different comparer declaration).
    /// </summary>
    const string Source = """
        using Generator.Equals;

        namespace Sample;

        /// <summary>A class.</summary>
        [Equatable]
        public partial class Root
        {
            /// <summary>A property.</summary>
            public string? Name { get; set; }
        }

        /// <summary>A derived class.</summary>
        [Equatable]
        public partial class Derived : Root
        {
            /// <summary>A property.</summary>
            public int Age { get; set; }
        }

        /// <summary>A record.</summary>
        [Equatable]
        public partial record RootRecord
        {
            /// <summary>A property.</summary>
            public string? Name { get; set; }
        }

        /// <summary>A derived record.</summary>
        [Equatable]
        public partial record DerivedRecord : RootRecord
        {
            /// <summary>A property.</summary>
            public int Age { get; set; }
        }

        /// <summary>A struct.</summary>
        [Equatable]
        public partial struct SampleStruct
        {
            /// <summary>A property.</summary>
            public int Age { get; set; }
        }

        /// <summary>A record struct.</summary>
        [Equatable]
        public partial record struct SampleRecordStruct
        {
            /// <summary>A property.</summary>
            public int Age { get; set; }
        }
        """;

    [Fact]
    public async Task GeneratedCodeHasNoMissingXmlCommentWarnings()
    {
        // DocumentationMode.Diagnose is what <GenerateDocumentationFile>true</GenerateDocumentationFile> turns on.
        var parseOptions = new CSharpParseOptions(LanguageVersion.CSharp10, DocumentationMode.Diagnose);

        var references = await ReferenceAssemblies.Net.Net60.ResolveAsync(null, TestContext.Current.CancellationToken);

        var compilation = (Compilation)CSharpCompilation.Create(
            assemblyName: "TestAssembly",
            syntaxTrees: [CSharpSyntaxTree.ParseText(Source, parseOptions, cancellationToken: TestContext.Current.CancellationToken)],
            references: references.Add(RuntimeReference),
            options: new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                nullableContextOptions: NullableContextOptions.Enable));

        CSharpGeneratorDriver
            .Create([new GeneratorEquals::Generator.Equals.EqualsGenerator().AsSourceGenerator()], parseOptions: parseOptions)
            .RunGeneratorsAndUpdateCompilation(compilation, out var updated, out _, TestContext.Current.CancellationToken);

        var warnings = updated
            .GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Id == MissingXmlCommentWarning)
            .Select(d => $"{d.Location.GetLineSpan()}: {d.GetMessage()}")
            .ToImmutableArray();

        warnings.Should().BeEmpty();
    }
}
