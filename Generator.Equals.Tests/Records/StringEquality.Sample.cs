using System;

namespace Generator.Equals.Tests.Records
{
    public partial class StringEquality
    {
        [Equatable]
        public partial record SampleCaseInsensitive
        {
            [StringEquality(StringComparison.CurrentCultureIgnoreCase)]
            public string Name { get; init; } = "";
        }

        [Equatable]
        public partial record SampleCaseSensitive
        {
            [StringEquality(StringComparison.CurrentCulture)]
            public string Name { get; init; } = "";
        }
    }
}
