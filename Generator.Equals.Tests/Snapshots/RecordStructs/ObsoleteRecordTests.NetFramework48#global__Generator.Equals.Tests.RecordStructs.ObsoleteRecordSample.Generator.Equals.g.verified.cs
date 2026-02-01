//HintName: global__Generator.Equals.Tests.RecordStructs.ObsoleteRecordSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct ObsoleteRecordSample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.ObsoleteRecordSample other)
        {
            return true
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Name!, other.Name!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.ObsoleteRecordSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.ObsoleteRecordSample x, global::Generator.Equals.Tests.RecordStructs.ObsoleteRecordSample y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.ObsoleteRecordSample obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
