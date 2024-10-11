extern alias genEquals;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

namespace Generator.Equals.Tests.DynamicGeneration;

public class Base_Assertions
{
    // Check if immutable arrays are comparable
    [Fact]
    public void Immutable_Arrays_Eqatable()
    {
        var a = ImmutableArray.Create(1, 2, 3);
        var b = ImmutableArray.Create(1, 2, 3);
        Assert.Equal(a, b);
    }

    [Fact]
    public void Immutable_Arrays_Comparable()
    {
        var a = ImmutableArray.Create(1, 2, 3);
        var b = ImmutableArray.Create(1, 2, 3);
        Assert.True(a.SequenceEqual(b));
    }

    // Check if equality type model is comparable by value
    [Fact]
    public void EqualityTypeModel_Eqatable()
    {
        var a = CreateEqualityTypeModelMock();
        var b = CreateEqualityTypeModelMock();
        Assert.Equal(a, b);
    }

    [Fact]
    public void AttributesMetadata_Equatable()
    {
        var a = AttributesMetadata.CreateDefault();
        var b = AttributesMetadata.CreateDefault();
        Assert.True(a.Equals(b));
    }

    internal static EqualityTypeModel CreateEqualityTypeModelMock()
    {
        var containingSymbols = ImmutableArray.Create<ContainingSymbol>(
            new NamespaceContainingSymbol { Name = "Namespace1" },
            new TypeContainingSymbol { Name = "ContainingType", Kind = null }
        );

        var attributesMetadata = AttributesMetadata.Instance;

        var equalityMemberModels = ImmutableArray.Create(
            new EqualityMemberModel
            {
                PropertyName = "Property1",
                TypeName = "int",
                EqualityType = EqualityType.DefaultEquality
            },
            new EqualityMemberModel
            {
                PropertyName = "Property2",
                TypeName = "string",
                EqualityType = EqualityType.StringEquality,
                StringComparer = attributesMetadata.StringComparisonLookup[4]
            }
        );

        return new EqualityTypeModel
        {
            TypeName = "MyType",
            ContainingSymbols = containingSymbols,
            AttributesMetadata = attributesMetadata,
            ExplicitMode = false,
            IgnoreInheritedMembers = false,
            SyntaxKind = SyntaxKind.ClassDeclaration,
            TypeName = "MyType",
            BaseTypeName = "BaseType",
            IsSealed = true,
            ContainingSymbols = containingSymbols,
            AttributesMetadata = attributesMetadata,
            ExplicitMode = false,
            IgnoreInheritedMembers = false,
            BuildEqualityModels = equalityMemberModels,
            Fullname = "Namespace1.MyType"
        };
    }
}