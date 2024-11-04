using Generator.Equals.Tests.DynamicGeneration.Utils;

namespace Generator.Equals.Tests.DynamicGeneration;

public class DefaultsTest
{
    [Theory]
    [InlineData("Unordered",
        "global::Generator.Equals.UnorderedEqualityComparer<int>.Default.Equals(this.Properties!, other.Properties!)")]
    [InlineData("Ordered",
        "global::Generator.Equals.OrderedEqualityComparer<int>.Default.Equals(this.Properties!, other.Properties!)")]
    public void Global_Enumerable_Order_Is_Respected(string enumerableEquality, string expectedComparisonLine)
    {
        var source =
            """
            public partial class UnorderedEquality
            {
                [Equatable]
                public partial class Sample
                {
                    public List<int>? Properties { get; set; }
                }
            }
            """;


        // generator_equals_comparison_string = OrdinalIgnoreCase
        // generator_equals_comparison_enumerable = Unordered
        var genResult = GeneratorTestHelper.RunGenerator(source, new()
        {
            { "generator_equals_comparison_enumerable", enumerableEquality }
        });

        var gensource = genResult.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);

        Assert.Contains(
            expectedComparisonLine,
            gensource.FirstOrDefault()?.ToString());
    }
    
    // generator_equals_comparison_string is respected
    [Theory]
    [InlineData("OrdinalIgnoreCase",
        "global::System.StringComparer.OrdinalIgnoreCase.Equals(this.Tag!, other.Tag!)")]
    [InlineData("Ordinal",
        "global::System.StringComparer.Ordinal.Equals(this.Tag!, other.Tag!)")]
    [InlineData("InvariantCulture",
        "global::System.StringComparer.InvariantCulture.Equals(this.Tag!, other.Tag!)")]
    public void Global_String_Comparison_Is_Respected(string stringComparison, string expectedComparisonLine)
    {
        var source =
            """
            [Equatable]
            public partial class Resource
            {
                public string Tag {get;set;}
            }
            """;

        var genResult = GeneratorTestHelper.RunGenerator(source, new()
        {
            { "generator_equals_comparison_string", stringComparison }
        });
        
        var gensource = genResult.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;
        
        Assert.NotNull(gensource);
        
        Assert.Contains(
            expectedComparisonLine,
            gensource.FirstOrDefault()?.ToString());

    }
    
    [Theory]
    [InlineData("OrdinalIgnoreCase",
        "new global::Generator.Equals.OrderedEqualityComparer<string>(global::System.StringComparer.OrdinalIgnoreCase).Equals(this.Tags!, other.Tags!)")]
    [InlineData("Ordinal",
        "new global::Generator.Equals.OrderedEqualityComparer<string>(global::System.StringComparer.Ordinal).Equals(this.Tags!, other.Tags!)")]
    [InlineData("InvariantCulture",
        "new global::Generator.Equals.OrderedEqualityComparer<string>(global::System.StringComparer.InvariantCulture).Equals(this.Tags!, other.Tags!)")]
    public void Global_String_Comparison_Is_Respected_In_Lists(string stringComparison, string expectedComparisonLine)
    {
        var source =
            """
            [Equatable]
            public partial class Resource
            {
                public List<string>? Tags {get;set;}
            }
            """;
        
        var genResult = GeneratorTestHelper.RunGenerator(source, new()
        {
            { "generator_equals_comparison_string", stringComparison }
        });
        
        var gensource = genResult.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;
        
        Assert.NotNull(gensource);
        
        Assert.Contains(
            expectedComparisonLine,
            gensource.FirstOrDefault()?.ToString());

    }
}