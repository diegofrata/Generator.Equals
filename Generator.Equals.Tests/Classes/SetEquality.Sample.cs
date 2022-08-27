using System.Collections.Generic;

namespace Generator.Equals.Tests.Classes
{
    public partial class SetEquality
    {
        [Equatable]
        public partial class Sample
        {
            [SetEquality] public List<int>? Properties { get; set; }
        }
    }
}