//HintName: global__Generator.Equals.Tests.V3.RecordStructs.ReferenceEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.V3.RecordStructs
{
    partial record struct ReferenceEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.V3.RecordStructs.ReferenceEqualitySample other)
        {
            return true
                && global::Generator.Equals.ReferenceEqualityComparer<global::System.String>.Default.Equals(this.Name!, other.Name!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name!,
                global::Generator.Equals.ReferenceEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
    }
}
