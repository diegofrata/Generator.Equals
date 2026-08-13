//HintName: global__Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct UnorderedEqualityWithComparerSampleWithStringComparison : global::System.IEquatable<global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison left, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison left, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison x, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison y)
            {
                return true
                    && new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(x.Tags!, y.Tags!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Tags!,
                    new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison x, global::Generator.Equals.Tests.Structs.UnorderedEqualityWithComparerSampleWithStringComparison y, global::Generator.Equals.MemberPath path = default)
            {
                if (!new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(x.Tags!, y.Tags!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Tags"));
                    var __xSet = x.Tags is null ? new global::System.Collections.Generic.HashSet<global::System.String>() : new global::System.Collections.Generic.HashSet<global::System.String>(x.Tags);
                    var __ySet = y.Tags is null ? new global::System.Collections.Generic.HashSet<global::System.String>() : new global::System.Collections.Generic.HashSet<global::System.String>(y.Tags);
                    
                    foreach (var __removed in global::System.Linq.Enumerable.Except(__xSet, __ySet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Removed()), __removed, null);
                    
                    foreach (var __added in global::System.Linq.Enumerable.Except(__ySet, __xSet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Added()), null, __added);
                }
            }
        }
    }
}
