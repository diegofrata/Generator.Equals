using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Generator.Equals;

internal record GeneratorOptions
{
    public StringComparison DefaultStringComparison { get; init; } = StringComparison.Ordinal;
    public ArrayComparison ArrayCompare { get; init; } = ArrayComparison.Ordered;

    public GeneratorOptions(AnalyzerConfigOptions options)
    {
        // if (options.TryGetValue("build_property.DemoSourceGenerator_Counter", out var counterEnabledValue))
        // {
        //     DefaultStringComparison = IsFeatureEnabled(counterEnabledValue);
        // }

        if (options.TryGetValue("generator_equals_comparison_string", out var stringComparison))
        {
            DefaultStringComparison = Enum.TryParse<StringComparison>(stringComparison, out var comparison)
                ? comparison
                : StringComparison.Ordinal;
        }
        
        if (options.TryGetValue("generator_equals_comparison_enumerable", out var arrayComparison))
        {
            ArrayCompare = Enum.TryParse<ArrayComparison>(arrayComparison, out var comparison)
                ? comparison
                : ArrayComparison.Ordered;
        }
    }

    private static bool IsFeatureEnabled(string counterEnabledValue)
    {
        return Equals("enable", counterEnabledValue)
               || Equals("enabled", counterEnabledValue)
               || Equals("true", counterEnabledValue)
               || Equals("1", counterEnabledValue);

        static bool Equals(string v1, string v2)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(v1, v2);
        }
    }
}

internal enum ArrayComparison
{
    Ordered,
    Unordered,
}