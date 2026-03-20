//HintName: global__Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record UnorderedEqualityWithComparerSampleWithCustomComparer
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(this.Names!, other.Names!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Names!,
                new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase)
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer? x, global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer? x, global::Generator.Equals.Tests.Records.UnorderedEqualityWithComparerSampleWithCustomComparer? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(x.Names!, y.Names!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Names"));
                    var __xSet = x.Names is null ? new global::System.Collections.Generic.HashSet<global::System.String>() : new global::System.Collections.Generic.HashSet<global::System.String>(x.Names);
                    var __ySet = y.Names is null ? new global::System.Collections.Generic.HashSet<global::System.String>() : new global::System.Collections.Generic.HashSet<global::System.String>(y.Names);
                    
                    foreach (var __removed in global::System.Linq.Enumerable.Except(__xSet, __ySet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Removed()), __removed, null);
                    
                    foreach (var __added in global::System.Linq.Enumerable.Except(__ySet, __xSet))
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Added()), null, __added);
                }
            }
        }
    }
}
