using System;

namespace Generator.Equals.Tests.Structs
{
    public partial class ObsoleteStruct
    {
        [Equatable]
        [Obsolete("Make sure the obsolete on the object model does not add warnings")]
        public partial struct Sample
        {
            public Sample(string value)
            {
                Something = value;
            }

            public string Something { get; }
        }
    }
}