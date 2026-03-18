//HintName: global__Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct PrecisionEqualityNullableIntSample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample other)
        {
            return true
                && (this.Value == other.Value || (this.Value.HasValue && other.Value.HasValue && global::System.Math.Abs(this.Value.Value - other.Value.Value) < 5))
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample x, global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample obj)
            {
                return obj.GetHashCode();
            }
            
            /// <summary>
            /// Returns the inequalities between two instances.
            /// </summary>
            /// <param name="x">The first instance to compare.</param>
            /// <param name="y">The second instance to compare.</param>
            /// <param name="path">The base path for difference reporting.</param>
            /// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample x, global::Generator.Equals.Tests.RecordStructs.PrecisionEqualityNullableIntSample y, global::Generator.Equals.MemberPath path = default)
            {
                if (!(x.Value == y.Value || (x.Value.HasValue && y.Value.HasValue && global::System.Math.Abs(x.Value.Value - y.Value.Value) < 5)))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Value")), x.Value, y.Value);
            }
        }
    }
}
