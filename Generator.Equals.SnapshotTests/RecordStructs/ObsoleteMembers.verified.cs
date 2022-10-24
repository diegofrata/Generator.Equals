//HintName: Generator.Equals.Tests.RecordStructs.ObsoleteMembers.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618

namespace Generator.Equals.Tests.RecordStructs
{
    partial class ObsoleteMembers
    {
        partial record struct Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.ObsoleteMembers.Sample other)
            {
                return true
                    && global::System.Collections.Generic.EqualityComparer<global::System.String>.Default.Equals(NoComment!, other.NoComment!)
                    && global::System.Collections.Generic.EqualityComparer<global::System.String>.Default.Equals(Comment!, other.Comment!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.NoComment!,
                    global::System.Collections.Generic.EqualityComparer<global::System.String>.Default);
                hashCode.Add(
                    this.Comment!,
                    global::System.Collections.Generic.EqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
