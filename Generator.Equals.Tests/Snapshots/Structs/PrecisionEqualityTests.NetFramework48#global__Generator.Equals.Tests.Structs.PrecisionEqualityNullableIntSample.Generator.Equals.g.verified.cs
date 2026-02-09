//HintName: global__Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct PrecisionEqualityNullableIntSample : global::System.IEquatable<global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample left, global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample left, global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample x, global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample y)
            {
                return true
                    && (x.Value == y.Value || (x.Value.HasValue && y.Value.HasValue && global::System.Math.Abs(x.Value.Value - y.Value.Value) < 5))
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample obj)
            {
                var hashCode = new global::System.HashCode();
                
                
                return hashCode.ToHashCode();
            }
            
            /// <summary>
            /// Returns the differences between two instances.
            /// </summary>
            /// <param name="x">The first instance to compare.</param>
            /// <param name="y">The second instance to compare.</param>
            /// <param name="path">The base path for difference reporting.</param>
            /// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public global::System.Collections.Generic.IEnumerable<(string Path, object? Left, object? Right)> Diff(global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample x, global::Generator.Equals.Tests.Structs.PrecisionEqualityNullableIntSample y, string? path = null)
            {
                var __path = string.IsNullOrEmpty(path) ? "" : path + ".";
                
                if (!(x.Value == y.Value || (x.Value.HasValue && y.Value.HasValue && global::System.Math.Abs(x.Value.Value - y.Value.Value) < 5)))
                    yield return (__path + "Value", x.Value, y.Value);
            }
        }
    }
}
