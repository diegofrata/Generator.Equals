using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using SourceGeneratorTestHelpers;

namespace Generator.Equals.DynamicGenerationTests;

public class UnitTest1
{
    public static readonly List<PortableExecutableReference> References2 =
        AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
            .Select(_ => MetadataReference.CreateFromFile(_.Location))
            .Concat(new[]
            {
                // add your app/lib specifics, e.g.:
                MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location),
            })
            .ToList();

    [Fact]
    public void Test1()
    {
        string input = SourceText.CSharp(
            """
            using System;
            using Generator.Equals;

             namespace Tests;

             [Equatable]
             public partial record MyRecord(
                 [property: OrderedEquality] string[] Fruits,
                 [property: StringEquality(StringComparison.OrdinalIgnoreCase)] string Fruit
             );

            //[Equatable]
            //public partial record Person(int Age);
            """
        );

        var opts = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithNullableContextOptions(NullableContextOptions.Enable)

            ;

        // var driver = CSharpGeneratorDriver.Create(generator)
        //     .WithUpdatedAnalyzerConfigOptionsProvider(
        //         new TestAnalyzerConfigOptionsProvider(globalOptions));

        var result = IncrementalGenerator.Run<EqualsGenerator>
        (
            input,
            new CSharpParseOptions()
            {
            },
            References2
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