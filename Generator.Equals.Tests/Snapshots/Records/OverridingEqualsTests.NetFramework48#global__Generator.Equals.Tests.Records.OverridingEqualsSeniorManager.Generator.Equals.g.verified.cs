//HintName: global__Generator.Equals.Tests.Records.OverridingEqualsSeniorManager.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record OverridingEqualsSeniorManager
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.OverridingEqualsSeniorManager? other)
        {
            return
                base.Equals((global::Generator.Equals.Tests.Records.OverridingEqualsManager?)other)
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
                global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public new sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.OverridingEqualsSeniorManager>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.OverridingEqualsSeniorManager? x, global::Generator.Equals.Tests.Records.OverridingEqualsSeniorManager? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.OverridingEqualsSeniorManager obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
