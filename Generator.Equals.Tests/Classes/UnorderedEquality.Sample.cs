using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class UnorderedEquality
    {
        [Equatable]
        public partial class Sample
        {
            [UnorderedEquality] public List<int>? Properties { get; set; }
        }
    }
}