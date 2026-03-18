//HintName: global__Generator.Equals.Tests.Records.ObsoleteMembersSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record ObsoleteMembersSample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.ObsoleteMembersSample? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.NoComment!, other.NoComment!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Comment!, other.Comment!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.NoComment!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            hashCode.Add(
                this.Comment!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.ObsoleteMembersSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.ObsoleteMembersSample? x, global::Generator.Equals.Tests.Records.ObsoleteMembersSample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.ObsoleteMembersSample obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Records.ObsoleteMembersSample? x, global::Generator.Equals.Tests.Records.ObsoleteMembersSample? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(x.NoComment!, y.NoComment!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("NoComment")), x.NoComment, y.NoComment);
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(x.Comment!, y.Comment!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Comment")), x.Comment, y.Comment);
            }
        }
    }
}
