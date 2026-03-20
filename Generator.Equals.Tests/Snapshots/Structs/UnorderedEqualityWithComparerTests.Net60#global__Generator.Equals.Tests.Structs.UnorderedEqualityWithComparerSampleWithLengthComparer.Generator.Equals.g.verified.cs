//HintName: global__Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct UnorderedEqualityWithComparerSampleWithLengthComparer : global::System.IEquatable<global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer left, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer left, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer x, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer y)
            {
                return true
                    && new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.Structs.UnorderedLengthEqualityComparer.Default).Equals(x.Values!, y.Values!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Values!,
                    new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.Structs.UnorderedLengthEqualityComparer.Default)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer x, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithLengthComparer y, global::Generator.Equals.MemberPath path = default)
            {
                if (!new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.Structs.UnorderedLengthEqualityComparer.Default).Equals(x.Values!, y.Values!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Values"));
                    var __xSet = x.Values is null ? new global::System.Collections.Generic.HashSet<global::System.String>() : new global::System.Collections.Generic.HashSet<global::System.String>(x.Values);
                    var __ySet = y.Values is null ? new global::System.Collections.Generic.HashSet<global::System.String>() : new global::System.Collections.Generic.HashSet<global::System.String>(y.Values);
                    
                    foreach (var __removed in global::System.Linq.Enumerable.Except(__xSet, __ySet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Removed()), __removed, null);
                    
                    foreach (var __added in global::System.Linq.Enumerable.Except(__ySet, __xSet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Added()), null, __added);
                }
            }
        }
    }
}
