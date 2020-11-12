using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public class AttributesMetadata
    {
        public INamedTypeSymbol Equatable { get; }
        public INamedTypeSymbol SequenceEquality { get; }
        public INamedTypeSymbol IgnoreEquality { get; }

        public AttributesMetadata(
            INamedTypeSymbol equatable,
            INamedTypeSymbol sequenceEquality,
            INamedTypeSymbol ignoreEquality
        )
        {
            Equatable = equatable;
            SequenceEquality = sequenceEquality;
            IgnoreEquality = ignoreEquality;
        }
    }
}