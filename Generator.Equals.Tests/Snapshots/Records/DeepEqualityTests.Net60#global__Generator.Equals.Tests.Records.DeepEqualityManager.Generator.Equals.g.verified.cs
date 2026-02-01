//HintName: global__Generator.Equals.Tests.Records.DeepEqualityManager.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record DeepEqualityManager
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.DeepEqualityManager? other)
        {
            return
                base.Equals((global::Generator.Equals.Tests.Records.DeepEqualityPerson?)other)
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
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public new sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.DeepEqualityManager>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.DeepEqualityManager? x, global::Generator.Equals.Tests.Records.DeepEqualityManager? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.DeepEqualityManager obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
