extern alias GeneratorEquals;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using GeneratorEquals::Generator.Equals.Analyzers;
using TypeSymbolExtensions = GeneratorEquals::Generator.Equals.Extensions.TypeSymbolExtensions;

namespace Generator.Equals.Tests.Analyzers;

/// <summary>
/// Tests for cross-assembly [Equatable] detection.
/// These tests verify that types with [Equatable] from referenced assemblies
/// are correctly detected and don't trigger GE002.
/// </summary>
public sealed class CrossAssemblyEquatableTests
{
    private static readonly MetadataReference[] CoreReferences = GetCoreReferences();

    private static MetadataReference[] GetCoreReferences()
    {
        var refs = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(EquatableAttribute).Assembly.Location),
        };

        // Get runtime assemblies
        var runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

        // Add System.Runtime for core types
        var systemRuntime = Path.Combine(runtimeDir, "System.Runtime.dll");
        if (File.Exists(systemRuntime))
            refs.Add(MetadataReference.CreateFromFile(systemRuntime));

        var netstandard = Path.Combine(runtimeDir, "netstandard.dll");
        if (File.Exists(netstandard))
            refs.Add(MetadataReference.CreateFromFile(netstandard));

        return refs.ToArray();
    }

    [Fact]
    public void TypeWithEquatable_FromReferencedAssembly_IsDetected()
    {
        // First, compile an "external" assembly with a type processed by Generator.Equals
        // (simulating what Generator.Equals produces with [GeneratedCode] on the Equals method)
        var externalSource = """
            using System;
            using System.CodeDom.Compiler;
            using Generator.Equals;

            namespace ExternalLib;

            [Equatable]
            public partial class ExternalType : IEquatable<ExternalType>
            {
                public string Name { get; set; }

                [GeneratedCode("Generator.Equals", "1.0.0.0")]
                public bool Equals(ExternalType other) => other != null && Name == other.Name;
            }
            """;

        var externalCompilation = CSharpCompilation.Create(
            "ExternalLib",
            new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Verify the external compilation has no errors
        var externalDiagnostics = externalCompilation.GetDiagnostics(TestContext.Current.CancellationToken);
        var errors = externalDiagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (errors.Count > 0)
            throw new Exception($"External compilation had errors: {string.Join(", ", errors.Select(e => e.GetMessage()))}");

        // Create a reference to the external compilation
        var externalReference = externalCompilation.ToMetadataReference();

        // Now compile the "main" assembly that references the external type
        var mainSource = """
            using Generator.Equals;
            using ExternalLib;

            namespace MainApp;

            [Equatable]
            public partial class Container
            {
                public ExternalType External { get; set; }
            }
            """;

        var mainReferences = CoreReferences.Append(externalReference).ToArray();

        var mainCompilation = CSharpCompilation.Create(
            "MainApp",
            new[] { CSharpSyntaxTree.ParseText(mainSource, cancellationToken: TestContext.Current.CancellationToken) },
            mainReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Verify the main compilation has no errors
        var mainDiagnostics = mainCompilation.GetDiagnostics(TestContext.Current.CancellationToken);
        var mainErrors = mainDiagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (mainErrors.Count > 0)
            throw new Exception($"Main compilation had errors: {string.Join(", ", mainErrors.Select(e => e.GetMessage()))}");

        // Get the ExternalType symbol from the main compilation (as it sees it from metadata)
        var externalTypeSymbol = mainCompilation.GetTypeByMetadataName("ExternalLib.ExternalType");
        if (externalTypeSymbol == null)
            throw new Exception("Could not find ExternalLib.ExternalType in main compilation");

        // Debug: Let's see what attributes are actually on this type
        var attributes = externalTypeSymbol.GetAttributes();
        var attributeInfo = string.Join(", ", attributes.Select(a =>
            $"{a.AttributeClass?.ContainingNamespace?.ToDisplayString()}.{a.AttributeClass?.Name}"));

        // This is the key test - does HasProperEquality detect the attribute on a metadata type?
        ITypeSymbol typeSymbol = externalTypeSymbol;
        var hasEquatable = TypeSymbolExtensions.HasProperEquality(typeSymbol);

        if (!hasEquatable)
            throw new Exception($"Expected ExternalType to have IEquatable<T>. Found attributes: [{attributeInfo}]");

        // Also verify IsComplexType returns false (since it has [Equatable])
        var isComplex = TypeSymbolExtensions.IsComplexType(typeSymbol);
        if (isComplex)
            throw new Exception($"Expected ExternalType to NOT be considered complex since it has [Equatable]. Attributes: [{attributeInfo}]");
    }

    [Fact]
    public async Task TypeWithEquatable_FromReferencedAssembly_DoesNotTriggerGE002()
    {
        // This test runs the actual analyzer against code that references an external type processed by Generator.Equals
        var externalSource = """
            using System;
            using System.CodeDom.Compiler;
            using Generator.Equals;

            namespace ExternalLib;

            [Equatable]
            public partial class ExternalType : IEquatable<ExternalType>
            {
                public string Name { get; set; }

                [GeneratedCode("Generator.Equals", "1.0.0.0")]
                public bool Equals(ExternalType other) => other != null && Name == other.Name;
            }
            """;

        var externalCompilation = CSharpCompilation.Create(
            "ExternalLib",
            new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var externalReference = externalCompilation.ToMetadataReference();

        var mainSource = """
            using Generator.Equals;
            using ExternalLib;

            namespace MainApp;

            [Equatable]
            public partial class Container
            {
                public ExternalType External { get; set; }
            }
            """;

        var mainReferences = CoreReferences.Append(externalReference).ToArray();

        var mainCompilation = CSharpCompilation.Create(
            "MainApp",
            new[] { CSharpSyntaxTree.ParseText(mainSource, cancellationToken: TestContext.Current.CancellationToken) },
            mainReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Run the analyzer
        DiagnosticAnalyzer analyzer = new EquatableAnalyzer();
        var compilationWithAnalyzers = mainCompilation.WithAnalyzers(
            ImmutableArray.Create(analyzer));

        var analyzerDiagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

        // Filter for GE002 diagnostics
        var ge002Diagnostics = analyzerDiagnostics
            .Where(d => d.Id == "GE002")
            .ToList();

        if (ge002Diagnostics.Count > 0)
            throw new Exception($"Expected no GE002 diagnostics but got: {string.Join(", ", ge002Diagnostics.Select(d => d.GetMessage()))}");
    }

    [Fact]
    public void RecordWithEquatable_FromReferencedAssembly_IsDetected()
    {
        // Test with a record type (user's scenario)
        var externalSource = """
            using Generator.Equals;

            namespace ExternalLib;

            [Equatable]
            public partial record ExternalSettings
            {
                public string Name { get; init; }
            }
            """;

        var externalCompilation = CSharpCompilation.Create(
            "ExternalLib",
            new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Verify the external compilation has no errors
        var externalDiagnostics = externalCompilation.GetDiagnostics(TestContext.Current.CancellationToken);
        var errors = externalDiagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (errors.Count > 0)
            throw new Exception($"External compilation had errors: {string.Join(", ", errors.Select(e => e.GetMessage()))}");

        var externalReference = externalCompilation.ToMetadataReference();

        // Main assembly references the external record
        var mainSource = """
            using Generator.Equals;
            using ExternalLib;

            namespace MainApp;

            [Equatable]
            public readonly partial record struct ContainerState
            {
                public required ExternalSettings Settings { get; init; }
            }
            """;

        var mainReferences = CoreReferences.Append(externalReference).ToArray();

        var mainCompilation = CSharpCompilation.Create(
            "MainApp",
            new[] { CSharpSyntaxTree.ParseText(mainSource, cancellationToken: TestContext.Current.CancellationToken) },
            mainReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Get the ExternalSettings symbol as seen from the main compilation
        var lbSettingsSymbol = mainCompilation.GetTypeByMetadataName("ExternalLib.ExternalSettings");
        if (lbSettingsSymbol == null)
            throw new Exception("Could not find ExternalLib.ExternalSettings in main compilation");

        // Debug output
        var attributes = lbSettingsSymbol.GetAttributes();
        var attributeInfo = string.Join(", ", attributes.Select(a =>
            $"[{a.AttributeClass?.ContainingNamespace?.ToDisplayString()}.{a.AttributeClass?.Name}]"));

        // Check HasProperEquality
        ITypeSymbol typeSymbol = lbSettingsSymbol;
        var hasEquatable = TypeSymbolExtensions.HasProperEquality(typeSymbol);

        if (!hasEquatable)
            throw new Exception($"Expected ExternalSettings to have IEquatable<T>. Found attributes: {attributeInfo}. IsRecord={lbSettingsSymbol.IsRecord}");

        // Verify IsComplexType returns false
        var isComplex = TypeSymbolExtensions.IsComplexType(typeSymbol);
        if (isComplex)
            throw new Exception($"Expected ExternalSettings to NOT be complex since it has [Equatable]. Attributes: {attributeInfo}");
    }

    [Fact]
    public async Task RecordWithEquatable_FromReferencedAssembly_DoesNotTriggerGE002()
    {
        var externalSource = """
            using Generator.Equals;

            namespace ExternalLib;

            [Equatable]
            public partial record ExternalSettings
            {
                public string Name { get; init; }
            }
            """;

        var externalCompilation = CSharpCompilation.Create(
            "ExternalLib",
            new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var externalReference = externalCompilation.ToMetadataReference();

        var mainSource = """
            using Generator.Equals;
            using ExternalLib;

            namespace MainApp;

            [Equatable]
            public readonly partial record struct ContainerState
            {
                public required ExternalSettings Settings { get; init; }
            }
            """;

        var mainReferences = CoreReferences.Append(externalReference).ToArray();

        var mainCompilation = CSharpCompilation.Create(
            "MainApp",
            new[] { CSharpSyntaxTree.ParseText(mainSource, cancellationToken: TestContext.Current.CancellationToken) },
            mainReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Run the analyzer
        DiagnosticAnalyzer analyzer = new EquatableAnalyzer();
        var compilationWithAnalyzers = mainCompilation.WithAnalyzers(
            ImmutableArray.Create(analyzer));

        var analyzerDiagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

        var ge002Diagnostics = analyzerDiagnostics
            .Where(d => d.Id == "GE002")
            .ToList();

        if (ge002Diagnostics.Count > 0)
            throw new Exception($"Expected no GE002 diagnostics for record with [Equatable] but got: {string.Join(", ", ge002Diagnostics.Select(d => d.GetMessage()))}");
    }

    [Fact]
    public async Task RecordWithEquatable_FromEmittedDll_DoesNotTriggerGE002()
    {
        // This test emits to an actual DLL file to simulate real-world scenario more closely
        var tempDir = Path.Combine(Path.GetTempPath(), $"GE_Test_{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);

        try
        {
            var externalDllPath = Path.Combine(tempDir, "ExternalLib.dll");

            var externalSource = """
                using Generator.Equals;

                namespace ExternalLib;

                [Equatable]
                public partial record ExternalSettings
                {
                    public string Name { get; init; }
                }
                """;

            var externalCompilation = CSharpCompilation.Create(
                "ExternalLib",
                new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
                CoreReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Emit to actual DLL
            var emitResult = externalCompilation.Emit(externalDllPath, cancellationToken: TestContext.Current.CancellationToken);
            if (!emitResult.Success)
            {
                var errors = emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error);
                throw new Exception($"Failed to emit external DLL: {string.Join(", ", errors.Select(e => e.GetMessage()))}");
            }

            // Debug: Check if the attribute exists in the raw metadata using reflection
            var reflectionAssembly = System.Reflection.Assembly.LoadFrom(externalDllPath);
            var reflectionType = reflectionAssembly.GetType("ExternalLib.ExternalSettings");
            var reflectionAttrs = reflectionType?.GetCustomAttributes(false);
            var reflectionAttrNames = reflectionAttrs?.Select(a => a.GetType().FullName).ToList() ?? new List<string>();

            // Reference the emitted DLL (not the in-memory compilation)
            var externalReference = MetadataReference.CreateFromFile(externalDllPath);

            var mainSource = """
                using Generator.Equals;
                using ExternalLib;

                namespace MainApp;

                [Equatable]
                public readonly partial record struct ContainerState
                {
                    public required ExternalSettings Settings { get; init; }
                }
                """;

            var mainReferences = CoreReferences.Append(externalReference).ToArray();

            var mainCompilation = CSharpCompilation.Create(
                "MainApp",
                new[] { CSharpSyntaxTree.ParseText(mainSource, cancellationToken: TestContext.Current.CancellationToken) },
                mainReferences,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Debug: Check compilation errors first
            var mainErrors = mainCompilation.GetDiagnostics(TestContext.Current.CancellationToken).Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
            if (mainErrors.Count > 0)
                throw new Exception($"Main compilation had errors: {string.Join(", ", mainErrors.Select(e => e.GetMessage()))}");

            // Debug: Check if we can see the attribute on the type from the emitted DLL
            var externalSettingsSymbol = mainCompilation.GetTypeByMetadataName("ExternalLib.ExternalSettings");
            if (externalSettingsSymbol == null)
                throw new Exception("Could not find ExternalLib.ExternalSettings in main compilation");

            var attributes = externalSettingsSymbol.GetAttributes();

            // Detailed debug info
            var debugInfo = new List<string>
            {
                $"Type: {externalSettingsSymbol.ToDisplayString()}",
                $"ContainingAssembly: {externalSettingsSymbol.ContainingAssembly?.Name}",
                $"Locations: {string.Join(", ", externalSettingsSymbol.Locations.Select(l => l.Kind))}",
                $"Attribute count (Roslyn): {attributes.Length}",
                $"Attribute count (Reflection): {reflectionAttrNames.Count}",
                $"Reflection attributes: [{string.Join(", ", reflectionAttrNames)}]"
            };

            foreach (var attr in attributes)
            {
                debugInfo.Add($"  Attribute:");
                debugInfo.Add($"    AttributeClass: {attr.AttributeClass?.ToDisplayString() ?? "null"}");
                debugInfo.Add($"    AttributeClass?.Name: {attr.AttributeClass?.Name ?? "null"}");
                debugInfo.Add($"    AttributeClass?.ContainingAssembly: {attr.AttributeClass?.ContainingAssembly?.Name ?? "null"}");
                debugInfo.Add($"    AttributeClass is ErrorTypeSymbol: {attr.AttributeClass?.TypeKind == TypeKind.Error}");
            }

            var attributeInfo = string.Join(", ", attributes.Select(a =>
                $"[{a.AttributeClass?.ContainingNamespace?.ToDisplayString()}.{a.AttributeClass?.Name}]"));

            // Check HasProperEquality
            ITypeSymbol typeSymbol = externalSettingsSymbol;
            var hasEquatable = TypeSymbolExtensions.HasProperEquality(typeSymbol);

            if (!hasEquatable)
                throw new Exception($"Expected ExternalSettings from emitted DLL to have [Equatable].\nDebug info:\n{string.Join("\n", debugInfo)}");

            // Run the analyzer
            DiagnosticAnalyzer analyzer = new EquatableAnalyzer();
            var compilationWithAnalyzers = mainCompilation.WithAnalyzers(
                ImmutableArray.Create(analyzer));

            var analyzerDiagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

            var ge002Diagnostics = analyzerDiagnostics
                .Where(d => d.Id == "GE002")
                .ToList();

            if (ge002Diagnostics.Count > 0)
                throw new Exception($"Expected no GE002 diagnostics for record from emitted DLL but got: {string.Join(", ", ge002Diagnostics.Select(d => d.GetMessage()))}. Attributes on ExternalSettings: {attributeInfo}");
        }
        finally
        {
            // Cleanup
            try { Directory.Delete(tempDir, true); } catch { /* ignore cleanup errors */ }
        }
    }

    [Fact]
    public async Task TypeWithMultipleAttributes_FromReferencedAssembly_DoesNotTriggerGE002()
    {
        // Mimics the real scenario: LbSettings has [GenerateShape, Equatable, GenerateSerializer, Alias]
        // We'll create mock attributes to simulate having multiple attributes from different packages

        // First create a "mock package" with some attributes
        var mockPackageSource = """
            namespace MockPackage;

            [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
            public class GenerateShapeAttribute : System.Attribute { }

            [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
            public class GenerateSerializerAttribute : System.Attribute { }

            [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
            public class AliasAttribute : System.Attribute
            {
                public AliasAttribute(string name) { }
            }
            """;

        var mockPackageCompilation = CSharpCompilation.Create(
            "MockPackage",
            new[] { CSharpSyntaxTree.ParseText(mockPackageSource, cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var mockPackageReference = mockPackageCompilation.ToMetadataReference();

        // Now compile the "external" assembly with a type that has MULTIPLE attributes including [Equatable]
        var externalSource = """
            using Generator.Equals;
            using MockPackage;

            namespace ExternalLib;

            [GenerateShape, Equatable, GenerateSerializer, Alias(nameof(Settings))]
            public sealed partial record Settings
            {
                public required ulong Version { get; init; }
                public required bool Enabled { get; init; }
            }
            """;

        var externalRefs = CoreReferences.Append(mockPackageReference).ToArray();
        var externalCompilation = CSharpCompilation.Create(
            "ExternalLib",
            new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
            externalRefs,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var externalErrors = externalCompilation.GetDiagnostics(TestContext.Current.CancellationToken).Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (externalErrors.Count > 0)
            throw new Exception($"External compilation had errors: {string.Join(", ", externalErrors.Select(e => e.GetMessage()))}");

        var externalReference = externalCompilation.ToMetadataReference();

        // Main assembly references the external type
        var mainSource = """
            using Generator.Equals;
            using ExternalLib;

            namespace MainApp;

            [Equatable]
            public readonly partial record struct ContainerState
            {
                public required Settings Settings { get; init; }
            }
            """;

        var mainReferences = CoreReferences.Append(mockPackageReference).Append(externalReference).ToArray();

        var mainCompilation = CSharpCompilation.Create(
            "MainApp",
            new[] { CSharpSyntaxTree.ParseText(mainSource, cancellationToken: TestContext.Current.CancellationToken) },
            mainReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // Debug: Check the attributes on Settings
        var settingsSymbol = mainCompilation.GetTypeByMetadataName("ExternalLib.Settings");
        if (settingsSymbol == null)
            throw new Exception("Could not find ExternalLib.Settings in main compilation");

        var attributes = settingsSymbol.GetAttributes();
        var attributeInfo = string.Join(", ", attributes.Select(a =>
            $"[{a.AttributeClass?.ContainingNamespace?.ToDisplayString()}.{a.AttributeClass?.Name}]"));

        // Check HasProperEquality
        ITypeSymbol typeSymbol = settingsSymbol;
        var hasEquatable = TypeSymbolExtensions.HasProperEquality(typeSymbol);

        if (!hasEquatable)
            throw new Exception($"Expected Settings to have IEquatable<T>. Found attributes: {attributeInfo}");

        // Run the analyzer
        DiagnosticAnalyzer analyzer = new EquatableAnalyzer();
        var compilationWithAnalyzers = mainCompilation.WithAnalyzers(
            ImmutableArray.Create(analyzer));

        var analyzerDiagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(TestContext.Current.CancellationToken);

        var ge002Diagnostics = analyzerDiagnostics
            .Where(d => d.Id == "GE002")
            .ToList();

        if (ge002Diagnostics.Count > 0)
            throw new Exception($"Expected no GE002 diagnostics for type with multiple attributes including [Equatable] but got: {string.Join(", ", ge002Diagnostics.Select(d => d.GetMessage()))}. Attributes on Settings: {attributeInfo}");
    }

    [Fact]
    public void DebugAttributeDetection_ShowsAttributeDetails()
    {
        // This test prints detailed info about attributes to help debug
        var externalSource = """
            using Generator.Equals;

            namespace ExternalLib;

            // Simulating user's setup with multiple attributes
            [System.Obsolete]  // Just to have another attribute
            [Equatable]
            public sealed partial record ExternalSettings
            {
                public string Name { get; init; }
            }
            """;

        var externalCompilation = CSharpCompilation.Create(
            "ExternalLib",
            new[] { CSharpSyntaxTree.ParseText(externalSource, cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var errors = externalCompilation.GetDiagnostics(TestContext.Current.CancellationToken)
            .Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (errors.Count > 0)
            throw new Exception($"Compilation errors: {string.Join(", ", errors.Select(e => e.GetMessage()))}");

        var externalReference = externalCompilation.ToMetadataReference();

        var mainCompilation = CSharpCompilation.Create(
            "MainApp",
            new[] { CSharpSyntaxTree.ParseText("// empty", cancellationToken: TestContext.Current.CancellationToken) },
            CoreReferences.Append(externalReference).ToArray(),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var lbSettingsSymbol = mainCompilation.GetTypeByMetadataName("ExternalLib.ExternalSettings");
        if (lbSettingsSymbol == null)
            throw new Exception("Could not find ExternalLib.ExternalSettings");

        var details = new List<string>
        {
            $"Type: {lbSettingsSymbol.ToDisplayString()}",
            $"IsRecord: {lbSettingsSymbol.IsRecord}",
            $"TypeKind: {lbSettingsSymbol.TypeKind}",
            $"IsFromSource: {!lbSettingsSymbol.Locations.All(l => l.Kind == LocationKind.MetadataFile)}",
            $"ContainingAssembly: {lbSettingsSymbol.ContainingAssembly?.Name}",
            "Attributes:"
        };

        foreach (var attr in lbSettingsSymbol.GetAttributes())
        {
            var attrClass = attr.AttributeClass;
            details.Add($"  - Name: {attrClass?.Name}");
            details.Add($"    Namespace: {attrClass?.ContainingNamespace?.ToDisplayString()}");
            details.Add($"    FullName: {attrClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}");
            details.Add($"    IsEquatableAttr: {attrClass?.Name is "EquatableAttribute" or "Equatable"}");
            details.Add($"    NamespaceMatches: {attrClass?.ContainingNamespace?.ToDisplayString() == "Generator.Equals"}");
        }

        ITypeSymbol typeSymbol = lbSettingsSymbol;
        var hasEquatable = TypeSymbolExtensions.HasProperEquality(typeSymbol);
        var isComplex = TypeSymbolExtensions.IsComplexType(typeSymbol);

        details.Add($"HasProperEquality result: {hasEquatable}");
        details.Add($"IsComplexType result: {isComplex}");

        // Always pass - this is just for debug output
        if (!hasEquatable)
            throw new Exception($"Debug info:\n{string.Join("\n", details)}");
    }

}
