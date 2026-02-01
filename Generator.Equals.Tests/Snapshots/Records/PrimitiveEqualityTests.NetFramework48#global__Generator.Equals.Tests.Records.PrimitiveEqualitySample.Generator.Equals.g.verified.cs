//HintName: global__Generator.Equals.Tests.Records.PrimitiveEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record PrimitiveEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.PrimitiveEqualitySample? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Name!, other.Name!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.Age!, other.Age!)
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
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            hashCode.Add(
                this.Age!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.PrimitiveEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.PrimitiveEqualitySample? x, global::Generator.Equals.Tests.Records.PrimitiveEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.PrimitiveEqualitySample obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
