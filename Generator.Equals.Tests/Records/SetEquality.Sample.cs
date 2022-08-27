using System.Collections.Generic;

namespace Generator.Equals.Tests.Records
{
    public partial class SetEquality
    {
        [Equatable]
        public partial record Sample
        {
            [SetEquality] public List<int>? Properties { get; set; }
        }
    }
}