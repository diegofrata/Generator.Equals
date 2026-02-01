//HintName: global__Generator.Equals.Tests.Records.CustomEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record CustomEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.CustomEqualitySample? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.Tests.Records.CustomEqualityComparer1.Default.Equals(this.Name1!, other.Name1!)
                && global::Generator.Equals.Tests.Records.CustomEqualityComparer2.Instance.Equals(this.Name2!, other.Name2!)
                && new global::Generator.Equals.Tests.Records.CustomEqualityLengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Name1!,
                global::Generator.Equals.Tests.Records.CustomEqualityComparer1.Default
            );
            hashCode.Add(
                this.Name2!,
                global::Generator.Equals.Tests.Records.CustomEqualityComparer2.Instance
            );
            hashCode.Add(
                this.Name3!,
                new global::Generator.Equals.Tests.Records.CustomEqualityLengthEqualityComparer()
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.CustomEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.CustomEqualitySample? x, global::Generator.Equals.Tests.Records.CustomEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.CustomEqualitySample obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
