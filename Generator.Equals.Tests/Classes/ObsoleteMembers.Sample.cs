using System;

namespace Generator.Equals.Tests.Classes
{
    public partial class ObsoleteMembers
    {
    
        // DO NOT ADD [Obsolete] TO THIS MODEL. It would suppress the obsoletes on the properties by itself.
        // This is why there is a separate ObsoleteClass test.
        [Equatable]
        public partial class Sample
        {
            public Sample(string value)
            {
#pragma warning disable CS0612
                NoComment = value;
#pragma warning restore CS0612
#pragma warning disable CS0618
                Comment = value;
#pragma warning restore CS0618
                // This pragma does not work, and didn't work in the generator either.
// #pragma warning disable CS0619
//                 CommentAndError = value;
// #pragma warning restore CS0619
            }

            [Obsolete]
            public string NoComment { get; }

            [Obsolete("Having a comment causes a different error code")]
            public string Comment { get; }

            // Could not find a way to suppress this
            // [Obsolete("Having a comment and IsError causes a different error code", true)]
            // public string? CommentAndError { get; }
            //
            // No idea what to do with this
            // [Obsolete(DiagnosticId = "CUSTOM0001")]
            // public string? OtherDiagnosticCode { get; }
        }
    }
}