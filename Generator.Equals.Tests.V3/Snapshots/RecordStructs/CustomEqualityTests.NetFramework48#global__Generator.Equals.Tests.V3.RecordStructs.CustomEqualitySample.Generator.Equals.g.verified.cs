//HintName: global__Generator.Equals.Tests.V3.RecordStructs.CustomEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.V3.RecordStructs
{
    partial record struct CustomEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualitySample other)
        {
            return true
                && global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualityComparer1.Default.Equals(this.Name1!, other.Name1!)
                && global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualityComparer2.Instance.Equals(this.Name2!, other.Name2!)
                && new global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualityLengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name1!,
                global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualityComparer1.Default
            );
            hashCode.Add(
                this.Name2!,
                global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualityComparer2.Instance
            );
            hashCode.Add(
                this.Name3!,
                new global::Generator.Equals.Tests.V3.RecordStructs.CustomEqualityLengthEqualityComparer()
            );
            
            return hashCode.ToHashCode();
        }
    }
}
