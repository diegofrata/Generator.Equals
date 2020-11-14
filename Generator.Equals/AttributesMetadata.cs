using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public class AttributesMetadata
    {
        public INamedTypeSymbol Equatable { get; }
        public INamedTypeSymbol SequenceEquality { get; }
        public INamedTypeSymbol IgnoreEquality { get; }
        public INamedTypeSymbol UnorderedSequenceEquality { get; }
        public INamedTypeSymbol ReferenceEquality { get; }

        public AttributesMetadata(
            INamedTypeSymbol equatable,
            INamedTypeSymbol sequenceEquality,
            INamedTypeSymbol ignoreEquality,
            INamedTypeSymbol unorderedSequenceEquality, 
            INamedTypeSymbol referenceEquality)
        {
            Equatable = equatable;
            SequenceEquality = sequenceEquality;
            IgnoreEquality = ignoreEquality;
            UnorderedSequenceEquality = unorderedSequenceEquality;
            ReferenceEquality = referenceEquality;
        }
    }
}
