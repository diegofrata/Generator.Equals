using System;

namespace Generator.Equals.Tests.Records
{
    public partial class ObsoleteRecord
    {
        [Equatable]
        [Obsolete("Make sure the obsolete on the object model does not add warnings")]
        public partial record Sample(string Name);
    }
}