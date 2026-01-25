//HintName: global__Generator.Equals.Tests.Records.StringEquality.SampleCaseInsensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class StringEquality
    {
        partial record SampleCaseInsensitive
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.StringEquality.SampleCaseInsensitive? other)
            {
                return
                    !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                    && global::System.StringComparer.CurrentCultureIgnoreCase.Equals(this.Name!, other.Name!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.EqualityContract);
                hashCode.Add(
                    this.Name!,
                    global::System.StringComparer.CurrentCultureIgnoreCase
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
