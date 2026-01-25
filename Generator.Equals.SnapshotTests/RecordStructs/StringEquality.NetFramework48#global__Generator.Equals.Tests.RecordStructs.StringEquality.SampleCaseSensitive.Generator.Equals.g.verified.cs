//HintName: global__Generator.Equals.Tests.RecordStructs.StringEquality.SampleCaseSensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial class StringEquality
    {
        partial record struct SampleCaseSensitive
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.StringEquality.SampleCaseSensitive other)
            {
                return true
                    && global::System.StringComparer.CurrentCulture.Equals(this.Name!, other.Name!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Name!,
                    global::System.StringComparer.CurrentCulture
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
