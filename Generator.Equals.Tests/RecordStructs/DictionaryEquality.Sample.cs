using System.Collections.Generic;

namespace Generator.Equals.Tests.RecordStructs
{
    public partial class DictionaryEquality
    {
        [Equatable]
        public partial record struct Sample
        {
            [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
        }
    }
}