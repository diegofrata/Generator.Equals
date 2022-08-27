using System.Collections.Generic;

namespace Generator.Equals.Tests.Records
{
    public partial class UnorderedEquality
    {
        [Equatable]
        public partial record Sample
        {
            [UnorderedEquality] public List<int>? Properties { get; init; }
        }
    }
}