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

namespace Generator.Equals.Analyzers.CodeFixes;

/// <summary>
/// Code fix for GE006: Adds 'partial' modifier to type declarations.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MakeTypePartialCodeFix))]
[Shared]
public sealed class MakeTypePartialCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.NonPartialType.Id);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
            return;

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the type declaration identified by the diagnostic
        var node = root.FindNode(diagnosticSpan);
        var typeDeclaration = node.FirstAncestorOrSelf<TypeDeclarationSyntax>();

        if (typeDeclaration == null)
            return;

        var codeAction = CodeAction.Create(
            title: "Make type partial",
            createChangedDocument: ct => MakeTypePartialAsync(context.Document, typeDeclaration, ct),
            equivalenceKey: "MakeTypePartial");

        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static async Task<Document> MakeTypePartialAsync(
        Document document,
        TypeDeclarationSyntax typeDeclaration,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
            return document;

        // Add partial modifier
        var partialToken = SyntaxFactory.Token(SyntaxKind.PartialKeyword)
            .WithTrailingTrivia(SyntaxFactory.Space);

        // Find the right position to insert 'partial' (before the type keyword)
        var typeKeywordIndex = typeDeclaration.Modifiers.Count;
        var newModifiers = typeDeclaration.Modifiers.Insert(typeKeywordIndex, partialToken);

        var newTypeDeclaration = typeDeclaration.WithModifiers(newModifiers);
        var newRoot = root.ReplaceNode(typeDeclaration, newTypeDeclaration);

        return document.WithSyntaxRoot(newRoot);
    }
}
