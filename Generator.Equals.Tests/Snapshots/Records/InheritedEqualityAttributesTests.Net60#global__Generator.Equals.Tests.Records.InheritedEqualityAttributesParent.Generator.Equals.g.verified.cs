//HintName: global__Generator.Equals.Tests.Records.InheritedEqualityAttributesParent.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record InheritedEqualityAttributesParent
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default.Equals(this.Ints!, other.Ints!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Ints!,
                global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? x, global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? x, global::Generator.Equals.Tests.Records.InheritedEqualityAttributesParent? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default.Equals(x.Ints!, y.Ints!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Ints"));
                    var __xList = x.Ints is null ? new global::System.Collections.Generic.List<global::System.Int32>() : new global::System.Collections.Generic.List<global::System.Int32>(x.Ints);
                    var __yList = y.Ints is null ? new global::System.Collections.Generic.List<global::System.Int32>() : new global::System.Collections.Generic.List<global::System.Int32>(y.Ints);
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
