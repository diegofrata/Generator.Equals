//HintName: global__Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class PlainIntermediateDumpTruck : global::System.IEquatable<global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? left, global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? left, global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck>.Equals(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return base.Equals(other as global::Generator.Equals.Tests.Classes.PlainIntermediateTruck)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(this.PayloadKg!, other.PayloadKg!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.Boolean>.Default.Equals(this.Tipping!, other.Tipping!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(base.GetHashCode());
            hashCode.Add(
                this.PayloadKg!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default
            );
            hashCode.Add(
                this.Tipping!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.Boolean>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        /// <summary>
        /// An equality comparer for the enclosing type that uses the generated equality semantics.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public new sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck>
        {
            /// <summary>
            /// Gets the default instance of the comparer.
            /// </summary>
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? x, global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals((object?) y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? x, global::Generator.Equals.Tests.Classes.PlainIntermediateDumpTruck? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                foreach (var __ineq in global::Generator.Equals.Tests.Classes.PlainIntermediateTruck.EqualityComparer.Default.Inequalities(x, y, path))
                    yield return __ineq;
                
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(x.PayloadKg!, y.PayloadKg!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("PayloadKg")), x.PayloadKg, y.PayloadKg);
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.Boolean>.Default.Equals(x.Tipping!, y.Tipping!))
                    yield return new global::Generator.Equals.Inequality(path.Append(global::Generator.Equals.MemberPathSegment.Property("Tipping")), x.Tipping, y.Tipping);
            }
        }
    }
}
