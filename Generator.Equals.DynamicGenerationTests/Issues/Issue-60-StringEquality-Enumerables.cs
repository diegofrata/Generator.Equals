using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGeneratorTestHelpers;

namespace Generator.Equals.DynamicGenerationTests.Issues;

public class Issue_60_StringEquality_Enumerables
{
    public static readonly List<PortableExecutableReference> References =
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location))
            .Select(x => MetadataReference.CreateFromFile(x.Location))
            .Append(MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location))
            .ToList();

    [Fact]
    public void Comparison_is_correctly_generated()
    {
        var input = SourceText.CSharp(
            """
            using System;
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Resource
            {
                [UnorderedEquality]
                [StringEqualityAttribute(StringComparison.OrdinalIgnoreCase)]
                public string[] Tags { get; set; } = Array.Empty<string>();

            }
            """
        );

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions(),
            References
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);

        var src = gensource.FirstOrDefault()?.ToString();

        Assert.Contains(
            "new global::Generator.Equals.UnorderedEqualityComparer<string>(global::System.StringComparer.OrdinalIgnoreCase)",
            src);
    }

    [Fact]
    public void Comparison_is_correctly_generated_without_attributes()
    {
        var input = SourceText.CSharp(
            """
            using System;
            using System.Collections.Generic;
            using Generator.Equals;

            [Equatable]
            public partial class Resource
            {
                public string[] Tags { get; set; } = Array.Empty<string>();
            }
            """
        );

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions(),
            References
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);

        var src = gensource.FirstOrDefault()?.ToString();

        Assert.Contains("new global::Generator.Equals.OrderedEqualityComparer<string>(global::System.StringComparer.Ordinal)",
            src);
    }
}