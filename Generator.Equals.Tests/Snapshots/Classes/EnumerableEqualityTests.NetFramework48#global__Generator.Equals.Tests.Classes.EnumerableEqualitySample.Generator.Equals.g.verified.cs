//HintName: global__Generator.Equals.Tests.Classes.EnumerableEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class EnumerableEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.Classes.EnumerableEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? left, global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? left, global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.EnumerableEqualitySample);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.EnumerableEqualitySample>.Equals(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && global::Generator.Equals.UnorderedEqualityComparer<global::System.Int32>.Default.Equals(this.Properties!, other.Properties!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
            hashCode.Add(
                this.Properties!,
                global::Generator.Equals.UnorderedEqualityComparer<global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.EnumerableEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? x, global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? x, global::Generator.Equals.Tests.Classes.EnumerableEqualitySample? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!global::Generator.Equals.UnorderedEqualityComparer<global::System.Int32>.Default.Equals(x.Properties!, y.Properties!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Properties"));
                    var __xSet = x.Properties is null ? new global::System.Collections.Generic.HashSet<global::System.Int32>() : new global::System.Collections.Generic.HashSet<global::System.Int32>(x.Properties);
                    var __ySet = y.Properties is null ? new global::System.Collections.Generic.HashSet<global::System.Int32>() : new global::System.Collections.Generic.HashSet<global::System.Int32>(y.Properties);
                    
                    foreach (var __removed in global::System.Linq.Enumerable.Except(__xSet, __ySet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Removed()), __removed, null);
                    
                    foreach (var __added in global::System.Linq.Enumerable.Except(__ySet, __xSet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Added()), null, __added);
                }
            }
        }
    }
}
