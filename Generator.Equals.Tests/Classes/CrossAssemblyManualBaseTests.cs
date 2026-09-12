extern alias GeneratorEquals;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Tests.Classes;

/// <summary>
/// Cross-assembly (metadata) coverage for base-equality delegation. When the base lives in a REFERENCED
/// assembly the generator sees it only through metadata, so these tests guard the metadata reads the
/// same-assembly snapshot tests can't exercise: IsOverride for a hand-written Equals(object)/GetHashCode,
/// HasGeneratedEqualityComparer for an [Equatable] ancestor (whose [Equatable] attribute is erased from
/// metadata), IsRecord, and the resulting delegation decision.
/// </summary>
public sealed class CrossAssemblyManualBaseTests
{
    static MetadataReference[] CoreReferences()
    {
        var refs = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Attribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location),
        };

        var runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        foreach (var name in new[] { "System.Runtime.dll", "netstandard.dll" })
        {
            var path = Path.Combine(runtimeDir, name);
            if (File.Exists(path))
                refs.Add(MetadataReference.CreateFromFile(path));
        }

        return refs.ToArray();
    }

    static Compilation Compile(string assemblyName, string source, params MetadataReference[] extra) =>
        CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source, cancellationToken: TestContext.Current.CancellationToken)],
            CoreReferences().Concat(extra).ToArray(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    /// <summary>Runs the generator on <paramref name="compilation"/> and returns it with the generated trees added.</summary>
    static Compilation WithGenerated(Compilation compilation)
    {
        CSharpGeneratorDriver
            .Create(new GeneratorEquals::Generator.Equals.EqualsGenerator().AsSourceGenerator())
            .RunGeneratorsAndUpdateCompilation(compilation, out var updated, out _, TestContext.Current.CancellationToken);
        return updated;
    }

    /// <summary>Runs the generator on <paramref name="compilation"/> and returns the generated source whose hint name contains <paramref name="hintNameContains"/>.</summary>
    static string GeneratedSourceFor(Compilation compilation, string hintNameContains)
    {
        var result = CSharpGeneratorDriver
            .Create(new GeneratorEquals::Generator.Equals.EqualsGenerator().AsSourceGenerator())
            .RunGenerators(compilation, TestContext.Current.CancellationToken)
            .GetRunResult();

        return result.Results
            .SelectMany(r => r.GeneratedSources)
            .Single(s => s.HintName.Contains(hintNameContains))
            .SourceText.ToString();
    }

    [Fact]
    public void ManualClassBase_InReferencedAssembly_IsDelegatedToWithObjectCast()
    {
        // A hand-written base (no [Equatable]) compiled into another assembly. Only its IsOverride flags
        // and Equals(object)/GetHashCode signatures survive in metadata.
        const string external = """
            using System;

            namespace ExternalLib;

            public class ManualAnimal
            {
                public ManualAnimal(string name) => Name = name;
                public string Name { get; }
                public override bool Equals(object? o) =>
                    o is ManualAnimal a && string.Equals(Name, a.Name, StringComparison.OrdinalIgnoreCase);
                public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
            }
            """;

        const string main = """
            using Generator.Equals;

            namespace MainApp;

            [Equatable]
            public partial class Dog : ExternalLib.ManualAnimal
            {
                public Dog(string name, string breed) : base(name) => Breed = breed;
                public string Breed { get; }
            }
            """;

        var externalRef = Compile("ExternalLib", external).ToMetadataReference();
        var generated = GeneratedSourceFor(Compile("MainApp", main, externalRef), "Dog");

        generated.Should().Contain("base.Equals((object?) other)",
            "the metadata-only hand-written base must be detected and delegated to with the object cast");
        generated.Should().Contain("private bool __BaseEquals(",
            "a hand-written base with no comparer needs the bridge");
        generated.Should().Contain("if (!x.__BaseEquals(y))",
            "Inequalities reports the base portion coarsely via the bridge");
        generated.Should().Contain("other.GetType() == this.GetType()",
            "a hand-written base gives no exact-runtime-type guarantee, so the guard stays in front of the delegation");
        generated.Should().NotContain("Equals(this.Name!, other.Name!)",
            "delegation must replace the property-comparison fallback");
    }

    [Fact]
    public void RecordIntermediate_UnderEquatableAncestorInReferencedAssembly_UsesBridge()
    {
        // External: an [Equatable] record root (its generated comparer is the only cross-assembly signal,
        // since [Equatable] itself is erased from metadata) and a NON-[Equatable] record intermediate.
        const string external = """
            using Generator.Equals;

            namespace ExternalLib;

            [Equatable]
            public partial record Root(int A);

            public partial record Mid(int A, int B) : Root(A);
            """;

        const string main = """
            using Generator.Equals;

            namespace MainApp;

            [Equatable]
            public partial record Leaf(int A, int B, int C) : ExternalLib.Mid(A, B);
            """;

        // Run the generator on the external assembly so Root carries its generated EqualityComparer in metadata.
        var externalRef = WithGenerated(Compile("ExternalLib", external)).ToMetadataReference();
        var generated = GeneratedSourceFor(Compile("MainApp", main, externalRef), "Leaf");

        generated.Should().Contain("private bool __BaseEquals(",
            "a non-[Equatable] record intermediate is opaque to the inherited comparer, so the bridge is needed");
        generated.Should().Contain("if (!x.__BaseEquals(y))",
            "the base portion must be reported coarsely, not delegated member-level (which would skip Mid.B)");
        generated.Should().NotContain(".EqualityComparer.Default.Inequalities(x, y, path)",
            "member-level delegation to the immediate base's inherited comparer would silently skip the record intermediate");
    }
}
