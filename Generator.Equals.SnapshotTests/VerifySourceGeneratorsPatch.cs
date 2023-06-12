using System.Collections;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace Generator.Equals.SnapshotTests;

public static class VerifySourceGeneratorsPatch
{
    public static void Patch()
    {
        var list = (IList)typeof(VerifierSettings)
            .GetField("typedConverters", BindingFlags.Static | BindingFlags.NonPublic)!
            .GetValue(null)!;
        
        list.RemoveAt(list.Count - 1);
        list.RemoveAt(list.Count - 1);
        
        VerifierSettings.RegisterFileConverter<GeneratorDriver>(Convert);
        VerifierSettings.RegisterFileConverter<GeneratorDriverRunResult>(Convert);
    }

    static ConversionResult Convert(GeneratorDriverRunResult target, IReadOnlyDictionary<string, object> context)
    {
        var exceptions = new List<Exception>();
        var targets = new List<Target>();
        foreach (var result in target.Results)
        {
            if (result.Exception != null)
            {
                exceptions.Add(result.Exception);
            }

            var collection = result.GeneratedSources
                .Where(x => !x.HintName.StartsWith("Generator.Equals.Runtime"))
                .OrderBy(x => x.HintName)
                .Select(SourceToTarget);
            targets.AddRange(collection);
        }

        if (exceptions.Count == 1)
        {
            throw exceptions.First();
        }

        if (exceptions.Count > 1)
        {
            throw new AggregateException(exceptions);
        }

        if (target.Diagnostics.Any())
        {
            var info = new
            {
                target.Diagnostics
            };
            return new(info, targets);
        }

        return new(null, targets);
    }

    static Target SourceToTarget(GeneratedSourceResult source)
    {
        var data = $"""
            //HintName: {source.HintName}
            {source.SourceText}
            """;
        return new("cs", data, Path.GetFileNameWithoutExtension(source.HintName));
    }

    static ConversionResult Convert(GeneratorDriver target, IReadOnlyDictionary<string, object> context) =>
        Convert(target.GetRunResult(), context);
}