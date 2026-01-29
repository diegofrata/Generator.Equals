using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Generator.Equals.Analyzers.CodeFixes;

/// <summary>
/// Code fix for GE004: Adds [Equatable] attribute to containing type.
/// Also adds 'partial' modifier if missing.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddEquatableAttributeCodeFix))]
[Shared]
public sealed class AddEquatableAttributeCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.OrphanedEqualityAttribute.Id);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the member with the orphaned attribute
        var node = root.FindNode(diagnosticSpan);
        var memberDeclaration = node.FirstAncestorOrSelf<MemberDeclarationSyntax>();

        // Get the containing type
        var typeDeclaration = memberDeclaration?.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (typeDeclaration == null)
            return;

        var codeAction = CodeAction.Create(
            title: "Add [Equatable] to containing type",
            createChangedDocument: ct => AddEquatableAttributeAsync(context.Document, typeDeclaration, ct),
            equivalenceKey: "AddEquatableAttribute");

        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static async Task<Document> AddEquatableAttributeAsync(
        Document document,
        TypeDeclarationSyntax typeDeclaration,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;

        // Create [Equatable] attribute using SyntaxGenerator
        var equatableAttribute = generator.Attribute(generator.IdentifierName("Equatable"));

        // Add the attribute to the type declaration
        var newTypeDeclaration = (TypeDeclarationSyntax)generator.AddAttributes(typeDeclaration, equatableAttribute);

        // Add partial modifier if missing
        if (!typeDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            newTypeDeclaration = (TypeDeclarationSyntax)generator.WithModifiers(
                newTypeDeclaration,
                generator.GetModifiers(newTypeDeclaration).WithPartial(true));
        }

        editor.ReplaceNode(typeDeclaration, newTypeDeclaration);
        return editor.GetChangedDocument();
    }
}
