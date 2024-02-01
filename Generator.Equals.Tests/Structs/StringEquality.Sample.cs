using System;

namespace Generator.Equals.Tests.Structs
{
    public partial class StringEquality
    {
        [Equatable]
        public partial struct SampleCaseInsensitive
        {
            public SampleCaseInsensitive(string name)
            {
                Name = name;
            }

            [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
            public string Name { get; init; } = "";
        }

        [Equatable]
        public partial struct SampleCaseSensitive
        {
            public SampleCaseSensitive(string name)
            {
                Name = name;
            }

            [StringEquality(StringComparison.CurrentCulture)]
            public string Name { get; init; } = "";
        }
    }
}
