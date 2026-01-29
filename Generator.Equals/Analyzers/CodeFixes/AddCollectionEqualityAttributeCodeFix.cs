using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Generator.Equals.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Generator.Equals.Analyzers.CodeFixes;

/// <summary>
/// Code fix for GE001: Adds collection equality attribute to collection properties.
/// Offers [OrderedEquality], [UnorderedEquality], or [SetEquality] based on collection type.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AddCollectionEqualityAttributeCodeFix))]
[Shared]
public sealed class AddCollectionEqualityAttributeCodeFix : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(DiagnosticDescriptors.CollectionMissingAttribute.Id);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root == null)
            return;

        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Find the property declaration
        var node = root.FindNode(diagnosticSpan);
        var propertyDeclaration = node.FirstAncestorOrSelf<PropertyDeclarationSyntax>();

        if (propertyDeclaration == null)
            return;

        // Get semantic model to determine the best attribute
        var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
        if (semanticModel == null)
            return;

        var propertySymbol = semanticModel.GetDeclaredSymbol(propertyDeclaration, context.CancellationToken);
        var propertyType = propertySymbol?.Type;

        // Determine smart default based on collection type
        var defaultAttribute = GetDefaultAttribute(propertyType);

        // Register code actions in order of relevance
        if (defaultAttribute == "SetEquality")
        {
            // For sets: SetEquality first, then others
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "SetEquality", isDefault: true);
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "UnorderedEquality", isDefault: false);
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "OrderedEquality", isDefault: false);
        }
        else if (defaultAttribute == "UnorderedEquality")
        {
            // For dictionaries: UnorderedEquality first
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "UnorderedEquality", isDefault: true);
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "OrderedEquality", isDefault: false);
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "SetEquality", isDefault: false);
        }
        else
        {
            // For other collections: OrderedEquality first (most common)
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "OrderedEquality", isDefault: true);
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "UnorderedEquality", isDefault: false);
            RegisterCodeFix(context, propertyDeclaration, diagnostic, "SetEquality", isDefault: false);
        }
    }

    private static void RegisterCodeFix(
        CodeFixContext context,
        PropertyDeclarationSyntax propertyDeclaration,
        Diagnostic diagnostic,
        string attributeName,
        bool isDefault)
    {
        var title = isDefault
            ? $"Add [{attributeName}] (recommended)"
            : $"Add [{attributeName}]";

        var codeAction = CodeAction.Create(
            title: title,
            createChangedDocument: ct => AddAttributeAsync(context.Document, propertyDeclaration, attributeName, ct),
            equivalenceKey: $"AddCollectionAttribute_{attributeName}");

        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static string GetDefaultAttribute(ITypeSymbol? propertyType)
    {
        if (propertyType == null)
            return "OrderedEquality";

        // Check for ISet<T> (HashSet, SortedSet, etc.)
        if (propertyType.IsSet())
            return "SetEquality";

        // Check for IDictionary<TKey, TValue>
        if (propertyType.IsDictionary())
            return "UnorderedEquality";

        // Default to OrderedEquality for other collections (List, Array, etc.)
        return "OrderedEquality";
    }

    private static async Task<Document> AddAttributeAsync(
        Document document,
        PropertyDeclarationSyntax propertyDeclaration,
        string attributeName,
        CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root == null)
            return document;

        // Create the attribute
        var attribute = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(attributeName));
        var attributeList = SyntaxFactory.AttributeList(SyntaxFactory.SingletonSeparatedList(attribute));

        // Add leading trivia from the property to the attribute list
        var leadingTrivia = propertyDeclaration.GetLeadingTrivia();
        attributeList = attributeList.WithLeadingTrivia(leadingTrivia);

        // Create new attribute lists
        var newAttributeLists = propertyDeclaration.AttributeLists.Insert(0, attributeList);

        // Update property with new attributes
        var newPropertyDeclaration = propertyDeclaration
            .WithAttributeLists(newAttributeLists)
            .WithLeadingTrivia(SyntaxFactory.TriviaList()); // Remove duplicate leading trivia

        // If there were no attributes before, we need to handle the trivia correctly
        if (propertyDeclaration.AttributeLists.Count == 0)
        {
            // Add newline after attribute list
            var lastAttr = newPropertyDeclaration.AttributeLists[newPropertyDeclaration.AttributeLists.Count - 1];
            var updatedAttrList = lastAttr.WithTrailingTrivia(SyntaxFactory.LineFeed);
            newPropertyDeclaration = newPropertyDeclaration.WithAttributeLists(
                newPropertyDeclaration.AttributeLists.Replace(lastAttr, updatedAttrList));

            // Add leading trivia to modifiers or type
            if (newPropertyDeclaration.Modifiers.Any())
            {
                var firstModifier = newPropertyDeclaration.Modifiers[0];
                var updatedModifier = firstModifier.WithLeadingTrivia(leadingTrivia);
                newPropertyDeclaration = newPropertyDeclaration.WithModifiers(
                    newPropertyDeclaration.Modifiers.Replace(firstModifier, updatedModifier));
            }
            else
            {
                newPropertyDeclaration = newPropertyDeclaration.WithType(
                    newPropertyDeclaration.Type.WithLeadingTrivia(leadingTrivia));
            }
        }

        var newRoot = root.ReplaceNode(propertyDeclaration, newPropertyDeclaration);
        return document.WithSyntaxRoot(newRoot);
    }
}
