//HintName: global__Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct SetEqualityWithComparerSampleWithLengthComparer
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer other)
        {
            return true
                && new global::Generator.Equals.SetEqualityComparer<global::System.String>(global::Generator.Equals.Tests.RecordStructs.SetLengthEqualityComparer.Default).Equals(this.Values!, other.Values!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Values!,
                new global::Generator.Equals.SetEqualityComparer<global::System.String>(global::Generator.Equals.Tests.RecordStructs.SetLengthEqualityComparer.Default)
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer x, global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer x, global::Generator.Equals.Tests.RecordStructs.SetEqualityWithComparerSampleWithLengthComparer y, global::Generator.Equals.MemberPath path = default)
            {
                if (!new global::Generator.Equals.SetEqualityComparer<global::System.String>(global::Generator.Equals.Tests.RecordStructs.SetLengthEqualityComparer.Default).Equals(x.Values!, y.Values!))
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
