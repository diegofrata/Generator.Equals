//HintName: global__Generator.Equals.Tests.Structs.FieldEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct FieldEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.Structs.FieldEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.FieldEqualitySample left, global::Generator.Equals.Tests.Structs.FieldEqualitySample right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.FieldEqualitySample left, global::Generator.Equals.Tests.Structs.FieldEqualitySample right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.FieldEqualitySample o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.FieldEqualitySample other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.FieldEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.FieldEqualitySample x, global::Generator.Equals.Tests.Structs.FieldEqualitySample y)
            {
                return true
                    && global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default.Equals(x._addresses!, y._addresses!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.FieldEqualitySample obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj._addresses!,
                    global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default
                );
                
                return hashCode.ToHashCode();
            }
            
            /// <summary>
            /// Returns the inequalities between two instances.
            /// </summary>
            /// <param name="x">The first instance to compare.</param>
            /// <param name="y">The second instance to compare.</param>
            /// <param name="path">The base path for difference reporting.</param>
            /// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Structs.FieldEqualitySample x, global::Generator.Equals.Tests.Structs.FieldEqualitySample y, global::Generator.Equals.MemberPath path = default)
            {
                if (!global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default.Equals(x._addresses!, y._addresses!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Field("_addresses"));
                    var __xList = x._addresses is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(x._addresses);
                    var __yList = y._addresses is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(y._addresses);
                    var __maxLen = global::System.Math.Max(__xList.Count, __yList.Count);
                    
                    for (var __i = 0; __i < __maxLen; __i++)
                    {
                        var __xVal = __i < __xList.Count ? (object?)__xList[__i] : null;
                        var __yVal = __i < __yList.Count ? (object?)__yList[__i] : null;
                        if (!global::System.Object.Equals(__xVal, __yVal))
                            yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Index(__i)), __xVal, __yVal);
                    }
                }
            }
        }
    }
}
