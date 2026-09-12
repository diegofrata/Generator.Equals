//HintName: global__Generator.Equals.Tests.Classes.ManualFinch.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class ManualFinch : global::System.IEquatable<global::Generator.Equals.Tests.Classes.ManualFinch>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.ManualFinch? left, global::Generator.Equals.Tests.Classes.ManualFinch? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.ManualFinch? left, global::Generator.Equals.Tests.Classes.ManualFinch? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.ManualFinch);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.ManualFinch>.Equals(global::Generator.Equals.Tests.Classes.ManualFinch? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.ManualFinch? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && base.Equals(other as global::Generator.Equals.Tests.Classes.IEquatableBird)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.Height!, other.Height!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Color!, other.Color!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(base.GetHashCode());
            hashCode.Add(
                this.Height!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default
            );
            hashCode.Add(
                this.Color!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        // Non-virtual bridge to the base type's Equals, for use by the nested comparer.
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        private bool __BaseEquals(global::Generator.Equals.Tests.Classes.ManualPlainPerch? other) => base.Equals(other as global::Generator.Equals.Tests.Classes.IEquatableBird);
        
        /// <summary>
        /// An equality comparer for the enclosing type that uses the generated equality semantics.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.ManualFinch>
        {
            /// <summary>
            /// Gets the default instance of the comparer.
            /// </summary>
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.ManualFinch? x, global::Generator.Equals.Tests.Classes.ManualFinch? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals((object?) y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.ManualFinch obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Classes.ManualFinch? x, global::Generator.Equals.Tests.Classes.ManualFinch? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!x.__BaseEquals(y))
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(x.Height!, y.Height!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Height")), x.Height, y.Height);
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(x.Color!, y.Color!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Color")), x.Color, y.Color);
            }
        }
    }
}
