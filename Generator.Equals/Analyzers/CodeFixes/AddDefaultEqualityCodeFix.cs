using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Generator.Equals.Analyzers.CodeFixes;

/// <summary>
/// Code fix that adds [DefaultEquality] attribute to suppress GE002 and GE003.
/// Useful for types that implement their own Equals/GetHashCode (e.g., protobuf-generated types).
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddDefaultEqualityCodeFix))]
[Shared]
public sealed class AddDefaultEqualityCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(
            DiagnosticDescriptors.ComplexTypeMissingEquatable.Id,       // GE002
            DiagnosticDescriptors.CollectionElementMissingEquatable.Id); // GE003

    public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
            return;

        foreach (var diagnostic in context.Diagnostics)
        {
            var node = root.FindNode(diagnostic.Location.SourceSpan);

            // Find the property to add the attribute to
            var property = node.FirstAncestorOrSelf<PropertyDeclarationSyntax>();
            if (property == null)
                continue;

            var codeAction = CodeAction.Create(
                title: "Add [DefaultEquality] (type has its own equality)",
                createChangedDocument: ct => AddDefaultEqualityAsync(
                    context.Document, property, ct),
                equivalenceKey: "AddDefaultEquality");

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }

    private static async Task<Document> AddDefaultEqualityAsync(
        Document document,
        PropertyDeclarationSyntax property,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;

        // Create the [DefaultEquality] attribute
        var attribute = generator.Attribute(
            generator.IdentifierName("DefaultEquality"));

        // Add the attribute to the property
        var newProperty = generator.AddAttributes(property, attribute);
        editor.ReplaceNode(property, newProperty);

        return editor.GetChangedDocument();
    }
}
