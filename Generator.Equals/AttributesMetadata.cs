using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public class AttributesMetadata
    {
        public INamedTypeSymbol Equatable { get; }
        public INamedTypeSymbol OrderedEquality { get; }
        public INamedTypeSymbol IgnoreEquality { get; }
        public INamedTypeSymbol UnorderedEquality { get; }
        public INamedTypeSymbol ReferenceEquality { get; }
        
        public INamedTypeSymbol SetEquality { get; }

        public AttributesMetadata(
            INamedTypeSymbol equatable,
            INamedTypeSymbol orderedEquality,
            INamedTypeSymbol ignoreEquality,
            INamedTypeSymbol unorderedEquality, 
            INamedTypeSymbol referenceEquality, 
            INamedTypeSymbol setEquality)
        {
            Equatable = equatable;
            OrderedEquality = orderedEquality;
            IgnoreEquality = ignoreEquality;
            UnorderedEquality = unorderedEquality;
            ReferenceEquality = referenceEquality;
            SetEquality = setEquality;
        }
    }
}
