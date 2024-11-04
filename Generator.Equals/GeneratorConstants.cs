using System.CodeDom.Compiler;
using System.Collections.Immutable;
using Generator.Equals.Generators.Core;
using Generator.Equals.Models;
using Microsoft.CodeAnalysis;

namespace Generator.Equals;

internal class GeneratorConstants
{
    internal const string GeneratedCodeAttributeDeclaration =
        "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"Generator.Equals\", \"1.0.0.0\")]";

    internal const string EnableNullableContext = "#nullable enable";

    // CS0612: Obsolete with no comment
    // CS0618: obsolete with comment
    internal const string SuppressObsoleteWarningsPragma = "#pragma warning disable CS0612,CS0618";

    internal const string SuppressTypeConflictsWarningsPragma = "#pragma warning disable CS0436";

    internal static readonly string[] EqualsOperatorCodeComment = @"
/// <summary>
/// Indicates whether the object on the left is equal to the object on the right.
/// </summary>
/// <param name=""left"">The left object</param>
/// <param name=""right"">The right object</param>
/// <returns>true if the objects are equal; otherwise, false.</returns>".ToLines();

    internal static readonly string[] NotEqualsOperatorCodeComment = @"
/// <summary>
/// Indicates whether the object on the left is not equal to the object on the right.
/// </summary>
/// <param name=""left"">The left object</param>
/// <param name=""right"">The right object</param>
/// <returns>true if the objects are not equal; otherwise, false.</returns>".ToLines();

    internal const string InheritDocComment = "/// <inheritdoc/>";
}