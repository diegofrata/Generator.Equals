//HintName: global__Generator.Equals.Tests.Records.InheritedOverrideSkipped.Child.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class InheritedOverrideSkipped
    {
        partial record Child
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.InheritedOverrideSkipped.Child? other)
            {
                return
                    base.Equals((global::Generator.Equals.Tests.Records.InheritedOverrideSkipped.Parent?)other)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(base.GetHashCode());
                
                return hashCode.ToHashCode();
            }
        }
    }
}
