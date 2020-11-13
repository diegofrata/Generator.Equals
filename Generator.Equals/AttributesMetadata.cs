﻿using Microsoft.CodeAnalysis;

namespace Generator.Equals
{
    public class AttributesMetadata
    {
        public INamedTypeSymbol Equatable { get; }
        public INamedTypeSymbol SequenceEquality { get; }
        public INamedTypeSymbol IgnoreEquality { get; }
        public INamedTypeSymbol DictionaryEquality { get; }
        public INamedTypeSymbol UnorderedSequenceEquality { get; }

        public AttributesMetadata(
            INamedTypeSymbol equatable,
            INamedTypeSymbol sequenceEquality,
            INamedTypeSymbol ignoreEquality,
            INamedTypeSymbol dictionaryEquality,
            INamedTypeSymbol unorderedSequenceEquality
        )
        {
            Equatable = equatable;
            SequenceEquality = sequenceEquality;
            IgnoreEquality = ignoreEquality;
            DictionaryEquality = dictionaryEquality;
            UnorderedSequenceEquality = unorderedSequenceEquality;
        }
    }
}
