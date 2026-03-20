//HintName: global__Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct OrderedEqualityWithComparerSampleWithStringComparison
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison other)
        {
            return true
                && new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(this.Tags!, other.Tags!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Tags!,
                new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase)
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison x, global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison x, global::Generator.Equals.Tests.RecordStructs.OrderedEqualityWithComparerSampleWithStringComparison y, global::Generator.Equals.MemberPath path = default)
            {
                if (!new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(x.Tags!, y.Tags!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Tags"));
                    var __xList = x.Tags is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(x.Tags);
                    var __yList = y.Tags is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(y.Tags);
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
