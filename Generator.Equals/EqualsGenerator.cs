﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Generator.Equals.Generators;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[assembly: InternalsVisibleTo("Generator.Equals.SnapshotTests")]

namespace Generator.Equals;

[Generator(LanguageNames.CSharp)]
public class EqualsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var provider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "Generator.Equals.EquatableAttribute",
                (syntaxNode, ct) => syntaxNode is ClassDeclarationSyntax or RecordDeclarationSyntax or StructDeclarationSyntax,
                (syntaxContext, ct) => new EqualityTypeModelTransformer(syntaxContext, ct).Transform()
            );

        context.RegisterSourceOutput(provider, (spc, ctx) =>
        {
            try
            {
                Execute(spc, ctx);
            }
            catch (Exception e)
            {
                // Console.WriteLine(e);
                throw;
            }
        });
    }

    private void Execute(SourceProductionContext productionContext, EqualityTypeModel model)
    {
        if (productionContext.CancellationToken.IsCancellationRequested)
            return;

        var source = model.SyntaxKind switch
        {
            SyntaxKind.StructDeclaration => StructEqualityGenerator.Generate(model),
            SyntaxKind.RecordStructDeclaration => RecordStructEqualityGenerator.Generate(model),
            SyntaxKind.RecordDeclaration => RecordEqualityGenerator.Generate(model),
            SyntaxKind.ClassDeclaration => ClassEqualityGenerator.Generate(model),
            _ => throw new Exception("should not have gotten here.")
        };

        var fileName = $"{EscapeFileName(model.Fullname)}.Generator.Equals.g.cs"!;
        productionContext.AddSource(fileName, source);
    }


    private static string EscapeFileName(string fileName) => new[] { '<', '>', ',' }
        .Aggregate(new StringBuilder(fileName), (s, c) => s.Replace(c, '_')).ToString();
}