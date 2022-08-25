using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public class AttributesMetadata
    {
        public INamedTypeSymbol Equatable { get; }
        public INamedTypeSymbol DefaultEquality { get; }
        public INamedTypeSymbol OrderedEquality { get; }
        public INamedTypeSymbol IgnoreEquality { get; }
        public INamedTypeSymbol UnorderedEquality { get; }
        public INamedTypeSymbol ReferenceEquality { get; }
        public INamedTypeSymbol SetEquality { get; }
        public INamedTypeSymbol CustomEquality { get; }

        public AttributesMetadata(
            INamedTypeSymbol equatable,
            INamedTypeSymbol defaultEquality,
            INamedTypeSymbol orderedEquality,
            INamedTypeSymbol ignoreEquality,
            INamedTypeSymbol unorderedEquality, 
            INamedTypeSymbol referenceEquality, 
            INamedTypeSymbol setEquality,
            INamedTypeSymbol customEquality)
        {
            Equatable = equatable;
            DefaultEquality = defaultEquality;
            OrderedEquality = orderedEquality;
            IgnoreEquality = ignoreEquality;
            UnorderedEquality = unorderedEquality;
            ReferenceEquality = referenceEquality;
            SetEquality = setEquality;
            CustomEquality = customEquality;
        }
    }
}
