//HintName: global__Generator.Equals.Tests.Records.InheritedFromNonEquatable.Child.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class InheritedFromNonEquatable
    {
        partial record Child
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.InheritedFromNonEquatable.Child? other)
            {
                return
                    base.Equals((global::Generator.Equals.Tests.Records.InheritedFromNonEquatable.Parent?)other)
                    && global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default.Equals(this.Ints!, other.Ints!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(base.GetHashCode());
                hashCode.Add(
                    this.Ints!,
                    global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
