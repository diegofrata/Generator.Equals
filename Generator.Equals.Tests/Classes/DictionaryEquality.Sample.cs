using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class DictionaryEquality
    {
        [Equatable]
        public partial class Sample
        {
            [UnorderedEquality] public Dictionary<string, int>? Properties { get; set; }
        }
    }
}