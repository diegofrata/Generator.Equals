namespace Generator.Equals.Tests.DynamicGeneration;

public class LocalFieldsAssertions
{
    [Fact]
    public static void Test()
    {
        var code = """
                   [Equatable]
                   public partial class Sample
                   {
                      public Sample(string name)
                      {
                          Name = name;
                      }
                   
                      [ReferenceEquality] public string Name { get; }
                   }
                   """;

        var generated = Utils.GeneratorTestHelper.RunGenerator(code);

        var gensource = generated.Results
                .SelectMany(x => x.GeneratedSources)
                .Select(x => x.SourceText)
                .ToList()
            ;

        Assert.NotNull(gensource);
    }
}