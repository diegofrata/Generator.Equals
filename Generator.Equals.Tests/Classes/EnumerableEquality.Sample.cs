using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class EnumerableEquality
    {
        [Equatable]
        public partial class Sample
        {
            [UnorderedEquality] public IEnumerable<int>? Properties { get; set; }
        }
    }
}