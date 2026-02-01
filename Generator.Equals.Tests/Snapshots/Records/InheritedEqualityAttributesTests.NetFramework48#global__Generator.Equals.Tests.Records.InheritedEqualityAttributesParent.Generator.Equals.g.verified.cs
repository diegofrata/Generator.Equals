//HintName: global__Generator.Equals.Tests.Records.InheritedEqualityAttributesParent.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record InheritedEqualityAttributesParent
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default.Equals(this.Ints!, other.Ints!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Ints!,
                global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? x, global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
