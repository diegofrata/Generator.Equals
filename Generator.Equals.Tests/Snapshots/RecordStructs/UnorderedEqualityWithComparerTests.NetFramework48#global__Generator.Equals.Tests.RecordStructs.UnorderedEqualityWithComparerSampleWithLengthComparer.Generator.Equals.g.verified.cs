//HintName: global__Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct UnorderedEqualityWithComparerSampleWithLengthComparer
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer other)
        {
            return true
                && new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.RecordStructs.UnorderedLengthEqualityComparer.Default).Equals(this.Values!, other.Values!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Values!,
                new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.RecordStructs.UnorderedLengthEqualityComparer.Default)
            );
            
            return hashCode.ToHashCode();
        }
        
        /// <summary>
        /// An equality comparer for the enclosing type that uses the generated equality semantics.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer>
        {
            /// <summary>
            /// Gets the default instance of the comparer.
            /// </summary>
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer x, global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer x, global::Generator.Equals.Tests.RecordStructs.UnorderedEqualityWithComparerSampleWithLengthComparer y, global::Generator.Equals.MemberPath path = default)
            {
                if (!new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.RecordStructs.UnorderedLengthEqualityComparer.Default).Equals(x.Values!, y.Values!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Values"));
                    var __xList = x.Values is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(x.Values);
                    var __remaining = y.Values is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(y.Values);
                    if (__xList.Count == 0 && __remaining.Count == 0)
                        yield return new global::Generator.Equals.Inequality(__propPath, x.Values, y.Values);
                    var __elementComparer = global::Generator.Equals.Tests.RecordStructs.UnorderedLengthEqualityComparer.Default;
                    foreach (var __item in __xList)
                    {
                        var __match = __remaining.FindIndex(__candidate => __elementComparer.Equals(__item, __candidate));
                        if (__match < 0)
                            yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Removed()), __item, null);
                        else
                            __remaining.RemoveAt(__match);
                    }
                    foreach (var __added in __remaining)
                        yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Added()), null, __added);
                }
            }
        }
    }
}
