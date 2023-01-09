//HintName: Generator.Equals.Tests.Records.OverridingEquals.SeniorManager.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class OverridingEquals
    {
        partial record SeniorManager
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.OverridingEquals.SeniorManager? other)
            {
                return
                    base.Equals((global::Generator.Equals.Tests.Records.OverridingEquals.Manager?)other)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.Shares!, other.Shares!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(base.GetHashCode());
                hashCode.Add(
                    this.Shares!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
