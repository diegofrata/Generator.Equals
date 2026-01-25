//HintName: global__Generator.Equals.Tests.RecordStructs.StringEquality.SampleCaseInsensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial class StringEquality
    {
        partial record struct SampleCaseInsensitive
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.StringEquality.SampleCaseInsensitive other)
            {
                return true
                    && global::System.StringComparer.CurrentCultureIgnoreCase.Equals(this.Name!, other.Name!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Name!,
                    global::System.StringComparer.CurrentCultureIgnoreCase
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
