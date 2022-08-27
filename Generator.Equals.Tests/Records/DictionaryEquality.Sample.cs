using System.Collections.Generic;

namespace Generator.Equals.Tests.Records
{
    public partial class DictionaryEquality
    {
        [Equatable]
        public partial record Sample
        {
            [UnorderedEquality] public Dictionary<string, int>? Properties { get; init; }
        }
    }
}