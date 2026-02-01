// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>", Scope = "member", Target = "~M:Generator.Equals.Models.AttributeMetadataExtensions.HasAttribute(Microsoft.CodeAnalysis.ISymbol,Generator.Equals.Models.AttributeMetadata)~System.Boolean")]

// RS2008: Release tracking is not needed for this project
[assembly: SuppressMessage("MicrosoftCodeAnalysisReleaseTracking", "RS2008:Enable analyzer release tracking", Justification = "Release tracking not required for this generator")]

// RS1038: The Workspaces reference is required for CodeFix providers
[assembly: SuppressMessage("MicrosoftCodeAnalysisCorrectness", "RS1038:Compiler extensions should be implemented in assemblies with compiler-provided references", Justification = "Workspaces reference is needed for CodeFix providers")]

// CA1062: Roslyn guarantees non-null context parameter in analyzer callbacks
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "Roslyn guarantees non-null context parameter", Scope = "member", Target = "~M:Generator.Equals.Analyzers.EquatableAnalyzer.Initialize(Microsoft.CodeAnalysis.Diagnostics.AnalysisContext)")]
