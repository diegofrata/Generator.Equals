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
/// Code fix that adds [SuppressMessage] attribute to suppress a diagnostic.
/// Available for informational diagnostics where the analyzer might be overly cautious.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SuppressWithAttributeCodeFix))]
[Shared]
public sealed class SuppressWithAttributeCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(
            DiagnosticDescriptors.CollectionMissingAttribute.Id,        // GE001
            DiagnosticDescriptors.ComplexTypeMissingEquatable.Id,       // GE002
            DiagnosticDescriptors.CollectionElementMissingEquatable.Id, // GE003
            DiagnosticDescriptors.OrphanedEqualityAttribute.Id,         // GE004
            DiagnosticDescriptors.ManualEqualsImplementation.Id);       // GE005

    public override FixAllProvider? GetFixAllProvider() => null; // Suppression should be intentional per-case

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
            return;

        foreach (var diagnostic in context.Diagnostics)
        {
            var node = root.FindNode(diagnostic.Location.SourceSpan);

            // Find the member to add the attribute to
            var member = node.FirstAncestorOrSelf<MemberDeclarationSyntax>();
            if (member == null)
                continue;

            var codeAction = CodeAction.Create(
                title: $"Suppress {diagnostic.Id} with [SuppressMessage]",
                createChangedDocument: ct => AddSuppressMessageAsync(
                    context.Document, member, diagnostic, ct),
                equivalenceKey: $"Suppress_{diagnostic.Id}");

            context.RegisterCodeFix(codeAction, diagnostic);
        }
    }

    private static async Task<Document> AddSuppressMessageAsync(
        Document document,
        MemberDeclarationSyntax member,
        Diagnostic diagnostic,
        CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);
        var generator = editor.Generator;

        // Build the [SuppressMessage] attribute
        // [SuppressMessage("Generator.Equals", "GE002:Complex object property type lacks [Equatable]")]
        var suppressMessageAttribute = SyntaxFactory.Attribute(
            SyntaxFactory.ParseName("System.Diagnostics.CodeAnalysis.SuppressMessage"),
            SyntaxFactory.AttributeArgumentList(
                SyntaxFactory.SeparatedList(new[]
                {
                    SyntaxFactory.AttributeArgument(
                        SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal("Generator.Equals"))),
                    SyntaxFactory.AttributeArgument(
                        SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression,
                            SyntaxFactory.Literal($"{diagnostic.Id}:{diagnostic.Descriptor.Title}")))
                })));

        // Add the attribute to the member
        var attributeList = SyntaxFactory.AttributeList(
            SyntaxFactory.SingletonSeparatedList(suppressMessageAttribute));

        var newMember = member.WithAttributeLists(
            member.AttributeLists.Insert(0, attributeList));

        editor.ReplaceNode(member, newMember);

        return editor.GetChangedDocument();
    }
}
