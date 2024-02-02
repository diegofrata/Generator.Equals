using System;

namespace Generator.Equals.Tests.Classes
{
    public partial class StringEquality
    {
        [Equatable]
        public partial class SampleCaseInsensitive
        {
            public SampleCaseInsensitive(string name)
            {
                Name = name;
            }

            [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
            public string Name { get; }
        }

        [Equatable]
        public partial class SampleCaseSensitive
        {
            public SampleCaseSensitive(string name)
            {
                Name = name;
            }

            [StringEquality(StringComparison.CurrentCulture)]
            public string Name { get; }
        }
    }
}
