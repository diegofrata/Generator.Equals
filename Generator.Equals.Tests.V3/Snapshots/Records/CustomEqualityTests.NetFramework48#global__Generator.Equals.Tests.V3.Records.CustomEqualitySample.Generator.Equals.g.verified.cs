//HintName: global__Generator.Equals.Tests.V3.Records.CustomEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.V3.Records
{
    partial record CustomEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.V3.Records.CustomEqualitySample? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.Tests.V3.Records.CustomEqualityComparer1.Default.Equals(this.Name1!, other.Name1!)
                && global::Generator.Equals.Tests.V3.Records.CustomEqualityComparer2.Instance.Equals(this.Name2!, other.Name2!)
                && new global::Generator.Equals.Tests.V3.Records.CustomEqualityLengthEqualityComparer().Equals(this.Name3!, other.Name3!)
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
                global::Generator.Equals.Tests.V3.Records.CustomEqualityComparer1.Default
            );
            hashCode.Add(
                this.Name2!,
                global::Generator.Equals.Tests.V3.Records.CustomEqualityComparer2.Instance
            );
            hashCode.Add(
                this.Name3!,
                new global::Generator.Equals.Tests.V3.Records.CustomEqualityLengthEqualityComparer()
            );
            
            return hashCode.ToHashCode();
        }
    }
}
