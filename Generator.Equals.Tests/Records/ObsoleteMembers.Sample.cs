using System;

namespace Generator.Equals.Tests.Records
{
    public partial class ObsoleteMembers
    {
        // DO NOT ADD [Obsolete] TO THIS MODEL. It would suppress the obsoletes on the properties by itself.
        // This is why there is a separate ObsoleteRecord test.
        [Equatable]
        public partial record Sample(
            [property: Obsolete] string NoComment
            , [property: Obsolete("a comment")] string Comment
            // Could not find a way to suppress this
            // , [property: Obsolete("a comment", true)] string CommentAndError = ""
            // No idea what to do with this
            // , [property: Obsolete(DiagnosticId = "CUSTOM0001")] string OtherDiagnosticCode = ""
        );
    }
}