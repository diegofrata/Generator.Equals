//HintName: global__Generator.Equals.Tests.Records.GenericParameterEqualitySample_TName_ TAge_.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record GenericParameterEqualitySample<TName, TAge>
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge>? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.DefaultEqualityComparer<TName>.Default.Equals(this.Name!, other.Name!)
                && global::Generator.Equals.DefaultEqualityComparer<TAge>.Default.Equals(this.Age!, other.Age!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Name!,
                global::Generator.Equals.DefaultEqualityComparer<TName>.Default
            );
            hashCode.Add(
                this.Age!,
                global::Generator.Equals.DefaultEqualityComparer<TAge>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge>>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge>? x, global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge>? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge> obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge>? x, global::Generator.Equals.Tests.Records.GenericParameterEqualitySample<TName, TAge>? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!global::Generator.Equals.DefaultEqualityComparer<TName>.Default.Equals(x.Name!, y.Name!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Name")), x.Name, y.Name);
                if (!global::Generator.Equals.DefaultEqualityComparer<TAge>.Default.Equals(x.Age!, y.Age!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Age")), x.Age, y.Age);
            }
        }
    }
}
