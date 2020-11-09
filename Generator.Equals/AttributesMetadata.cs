using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    class AttributesMetadata
    {
        public INamedTypeSymbol Equatable { get; }
        public INamedTypeSymbol SequenceEquality { get; }

        public AttributesMetadata(INamedTypeSymbol equatable, INamedTypeSymbol sequenceEquality)
        {
            Equatable = equatable;
            SequenceEquality = sequenceEquality;
        }
    }
}