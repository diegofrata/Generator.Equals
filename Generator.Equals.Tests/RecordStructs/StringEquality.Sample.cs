using System;

namespace Generator.Equals.Tests.RecordStructs
{
    public partial class StringEquality
    {
        [Equatable]
        public partial record struct SampleCaseInsensitive(
            [property: StringEquality(StringComparison.CurrentCultureIgnoreCase)]
            string Name);

        [Equatable]
        public partial record struct SampleCaseSensitive(
            [property: StringEquality(StringComparison.CurrentCulture)]
            string Name);
    }
}
