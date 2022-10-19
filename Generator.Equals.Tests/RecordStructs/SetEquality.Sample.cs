using System.Collections.Generic;

namespace Generator.Equals.Tests.RecordStructs
{
    public partial class SetEquality
    {
        [Equatable]
        public partial record struct Sample
        {
            [SetEquality] public List<int>? Properties { get; set; }
        }
    }
}