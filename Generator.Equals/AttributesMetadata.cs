using System.Collections.Immutable;
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
        public INamedTypeSymbol StringEquality { get; }
        public INamedTypeSymbol CustomEquality { get; }
        public ImmutableDictionary<long, string> StringComparisonLookup { get; }

        public AttributesMetadata(
            INamedTypeSymbol equatable,
            INamedTypeSymbol defaultEquality,
            INamedTypeSymbol orderedEquality,
            INamedTypeSymbol ignoreEquality,
            INamedTypeSymbol unorderedEquality,
            INamedTypeSymbol referenceEquality,
            INamedTypeSymbol setEquality,
            INamedTypeSymbol stringEquality,
            INamedTypeSymbol customEquality,
            ImmutableDictionary<long, string> stringComparisonLookup)
        {
            Equatable = equatable;
            DefaultEquality = defaultEquality;
            OrderedEquality = orderedEquality;
            IgnoreEquality = ignoreEquality;
            UnorderedEquality = unorderedEquality;
            ReferenceEquality = referenceEquality;
            SetEquality = setEquality;
            StringEquality = stringEquality;
            CustomEquality = customEquality;
            StringComparisonLookup = stringComparisonLookup;
        }
    }
}
