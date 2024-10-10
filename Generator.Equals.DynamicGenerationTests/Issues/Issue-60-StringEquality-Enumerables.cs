using Microsoft.CodeAnalysis.CSharp;
using SourceGeneratorTestHelpers;

namespace Generator.Equals.DynamicGenerationTests.Issues;

public class Issue_60_StringEquality_Enumerables
{
    [Fact]
    public void Test3_Struct_UnorderedEquality()
    {
        // StringComparer.OrdinalIgnoreCase;
        
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
            UnitTest1.References
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);

        Assert.Contains("new global::Generator.Equals.UnorderedEqualityComparer<string>(StringComparer.OrdinalIgnoreCase)",
            gensource.FirstOrDefault()?.ToString());
    }
}