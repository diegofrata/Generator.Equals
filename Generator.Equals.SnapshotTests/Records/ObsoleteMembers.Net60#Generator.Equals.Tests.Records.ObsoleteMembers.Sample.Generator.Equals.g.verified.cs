//HintName: Generator.Equals.Tests.Records.ObsoleteMembers.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class ObsoleteMembers
    {
        partial record Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.ObsoleteMembers.Sample? other)
            {
                return
                    !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.NoComment!, other.NoComment!)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Comment!, other.Comment!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.EqualityContract);
                hashCode.Add(
                    this.NoComment!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                hashCode.Add(
                    this.Comment!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
