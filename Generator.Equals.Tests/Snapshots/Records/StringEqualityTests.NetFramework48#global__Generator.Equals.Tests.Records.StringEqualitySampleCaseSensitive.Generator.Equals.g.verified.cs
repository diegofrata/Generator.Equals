//HintName: global__Generator.Equals.Tests.Records.StringEqualitySampleCaseSensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record StringEqualitySampleCaseSensitive
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.StringEqualitySampleCaseSensitive? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::System.StringComparer.CurrentCulture.Equals(this.Name!, other.Name!)
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
                global::System.StringComparer.CurrentCulture
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.StringEqualitySampleCaseSensitive>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.StringEqualitySampleCaseSensitive? x, global::Generator.Equals.Tests.Records.StringEqualitySampleCaseSensitive? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.StringEqualitySampleCaseSensitive obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
