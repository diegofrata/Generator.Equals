using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using SourceGeneratorTestHelpers;

namespace Generator.Equals.DynamicGenerationTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        string input = """
                       using Generator.Equals;

                       namespace Tests;

                       [EquatableAttribute]
                       partial class MyRecord(
                           [property: OrderedEquality] string[] Fruits
                       );
                       """;

        var references = new List<MetadataReference>()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location),
        };
        
        // add netstandard
        references.Add(MetadataReference.CreateFromFile(typeof(System.Runtime.GCSettings).Assembly.Location));
        
        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new Microsoft.CodeAnalysis.CSharp.CSharpParseOptions()
            {
            },
            references
        );

        var gensource = result.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);

        //var genSourceString = gensource.ToString();

        //// Assert in the same namespace
        //Assert.Contains("namespace Tests", genSourceString);
    }
}