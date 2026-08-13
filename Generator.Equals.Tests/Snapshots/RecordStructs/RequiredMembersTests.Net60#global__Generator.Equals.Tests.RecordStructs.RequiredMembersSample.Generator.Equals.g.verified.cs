//HintName: global__Generator.Equals.Tests.RecordStructs.RequiredMembersSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct RequiredMembersSample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample other)
        {
            return true
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Name!, other.Name!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.Age!, other.Age!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String?>.Default.Equals(this.Optional!, other.Optional!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            hashCode.Add(
                this.Age!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default
            );
            hashCode.Add(
                this.Optional!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String?>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample x, global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample x, global::Generator.Equals.Tests.RecordStructs.RequiredMembersSample y, global::Generator.Equals.MemberPath path = default)
            {
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(x.Name!, y.Name!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Name")), x.Name, y.Name);
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(x.Age!, y.Age!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Age")), x.Age, y.Age);
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String?>.Default.Equals(x.Optional!, y.Optional!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Optional")), x.Optional, y.Optional);
            }
        }
    }
}
