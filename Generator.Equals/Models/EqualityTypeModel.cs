using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Models;

internal sealed record EqualityTypeModel
{
    public required SyntaxKind SyntaxKind { get; init; }

    public required string TypeName { get; init; }
    public required string? BaseTypeName { get; init; }
    public required bool IsSealed { get; init; }
    public required EquatableImmutableArray<ContainingSymbol> ContainingSymbols { get; init; }
    public required AttributesMetadata AttributesMetadata { get; init; }
    public required bool ExplicitMode { get; init; }
    public required bool IgnoreInheritedMembers { get; init; }
    public required EquatableImmutableArray<EqualityMemberModel> BuildEqualityModels { get; init; }
    public required string Fullname { get; init; }
    
    // public GeneratorOptions? GlobalOptions { get; init; }
    
    public EqualityTypeModel WithGeneratorOptions(GeneratorOptions options)
    {
        EqualityMemberModel[] newModels = null;
        
        for (var index = 0; index < BuildEqualityModels.Items.Length; index++)
        {
            var equalityModel = BuildEqualityModels.Items[index];
            
            if (equalityModel.IsDefaultEqualityType
                && equalityModel.EqualityType == EqualityType.OrderedEquality)
            {
                if (options.ArrayCompare == ArrayComparison.Unordered)
                {
                    newModels ??= BuildEqualityModels.Items.ToArray();
                    
                    equalityModel = equalityModel with
                    {
                        EqualityType = EqualityType.UnorderedEquality
                    };
                    
                    newModels[index] = equalityModel;
                }
            }

            if (equalityModel.IsDefaultStringComparer)
            {
                newModels ??= BuildEqualityModels.Items.ToArray();
                
                equalityModel = equalityModel with
                {
                    StringComparer = options.DefaultStringComparison.ToString()
                };
                
                newModels[index] = equalityModel;
            }
        }

        return this with
        {
            BuildEqualityModels = newModels is null
                ? BuildEqualityModels
                : new EquatableImmutableArray<EqualityMemberModel>(newModels)
        };
    }
    
}