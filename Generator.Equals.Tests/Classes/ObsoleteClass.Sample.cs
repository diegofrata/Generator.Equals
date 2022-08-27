using System;

namespace Generator.Equals.Tests.Classes
{
    public partial class ObsoleteClass
    {
        [Equatable]
        [Obsolete("Make sure the obsolete on the object model does not add warnings")]
        public partial class Sample
        {
            public Sample(string value)
            {
                Something = value;
            }

            public string Something { get; }
        }
    }
}