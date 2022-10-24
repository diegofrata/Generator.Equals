using System.Collections.Generic;

namespace Generator.Equals.Tests.Structs
{
    public partial class SetEquality
    {
        [Equatable]
        public partial struct Sample
        {
            [SetEquality] public List<int>? Properties { get; set; }
        }
    }
}