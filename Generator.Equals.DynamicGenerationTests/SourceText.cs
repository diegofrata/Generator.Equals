using System.Diagnostics.CodeAnalysis;

namespace Generator.Equals.DynamicGenerationTests;

internal static class SourceText
{
    public static string CSharp([StringSyntax("c#-test")]string source) => source;
}
