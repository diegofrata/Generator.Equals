//HintName: global__Generator.Equals.Tests.RecordStructs.CustomEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct CustomEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample other)
        {
            return true
                && global::Generator.Equals.Tests.RecordStructs.CustomEqualityComparer1.Default.Equals(this.Name1!, other.Name1!)
                && global::Generator.Equals.Tests.RecordStructs.CustomEqualityComparer2.Instance.Equals(this.Name2!, other.Name2!)
                && new global::Generator.Equals.Tests.RecordStructs.CustomEqualityLengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name1!,
                global::Generator.Equals.Tests.RecordStructs.CustomEqualityComparer1.Default
            );
            hashCode.Add(
                this.Name2!,
                global::Generator.Equals.Tests.RecordStructs.CustomEqualityComparer2.Instance
            );
            hashCode.Add(
                this.Name3!,
                new global::Generator.Equals.Tests.RecordStructs.CustomEqualityLengthEqualityComparer()
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample x, global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample x, global::Generator.Equals.Tests.RecordStructs.CustomEqualitySample y, global::Generator.Equals.MemberPath path = default)
            {
                if (!global::Generator.Equals.Tests.RecordStructs.CustomEqualityComparer1.Default.Equals(x.Name1!, y.Name1!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Name1")), x.Name1, y.Name1);
                if (!global::Generator.Equals.Tests.RecordStructs.CustomEqualityComparer2.Instance.Equals(x.Name2!, y.Name2!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Name2")), x.Name2, y.Name2);
                if (!new global::Generator.Equals.Tests.RecordStructs.CustomEqualityLengthEqualityComparer().Equals(x.Name3!, y.Name3!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Name3")), x.Name3, y.Name3);
            }
        }
    }
}
