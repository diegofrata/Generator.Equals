using System.Collections.Generic;

namespace Generator.Equals.Tests.RecordStructs
{
    public partial class UnorderedEquality
    {
        [Equatable]
        public partial record struct Sample
        {
            [UnorderedEquality] public List<int>? Properties { get; init; }
        }
    }
}