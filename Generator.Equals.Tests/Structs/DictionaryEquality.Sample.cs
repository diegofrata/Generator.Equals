using System.Collections.Generic;

namespace Generator.Equals.Tests.Structs
{
    public partial class DictionaryEquality
    {
        [Equatable]
        public partial struct Sample
        {
            [UnorderedEquality] public Dictionary<string, int>? Properties { get; set; }
        }
    }
}