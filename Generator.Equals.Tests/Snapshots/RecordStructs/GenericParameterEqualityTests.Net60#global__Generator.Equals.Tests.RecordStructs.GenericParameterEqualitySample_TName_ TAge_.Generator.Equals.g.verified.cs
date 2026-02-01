//HintName: global__Generator.Equals.Tests.RecordStructs.GenericParameterEqualitySample_TName_ TAge_.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct GenericParameterEqualitySample<TName, TAge>
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.GenericParameterEqualitySample<TName, TAge> other)
        {
            return true
                && global::Generator.Equals.DefaultEqualityComparer<TName>.Default.Equals(this.Name!, other.Name!)
                && global::Generator.Equals.DefaultEqualityComparer<TAge>.Default.Equals(this.Age!, other.Age!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name!,
                global::Generator.Equals.DefaultEqualityComparer<TName>.Default
            );
            hashCode.Add(
                this.Age!,
                global::Generator.Equals.DefaultEqualityComparer<TAge>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.GenericParameterEqualitySample<TName, TAge>>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.GenericParameterEqualitySample<TName, TAge> x, global::Generator.Equals.Tests.RecordStructs.GenericParameterEqualitySample<TName, TAge> y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.GenericParameterEqualitySample<TName, TAge> obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
