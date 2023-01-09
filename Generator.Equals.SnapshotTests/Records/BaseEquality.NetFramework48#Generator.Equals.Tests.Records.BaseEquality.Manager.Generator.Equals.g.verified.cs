//HintName: Generator.Equals.Tests.Records.BaseEquality.Manager.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class BaseEquality
    {
        partial record Manager
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.BaseEquality.Manager? other)
            {
                return
                    base.Equals((global::Generator.Equals.Tests.Records.BaseEquality.Person?)other)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Department!, other.Department!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(base.GetHashCode());
                hashCode.Add(
                    this.Department!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
