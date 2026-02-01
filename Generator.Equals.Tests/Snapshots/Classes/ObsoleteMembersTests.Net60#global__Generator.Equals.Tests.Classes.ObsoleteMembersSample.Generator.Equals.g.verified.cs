//HintName: global__Generator.Equals.Tests.Classes.ObsoleteMembersSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class ObsoleteMembersSample : global::System.IEquatable<global::Generator.Equals.Tests.Classes.ObsoleteMembersSample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? left, global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? left, global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.ObsoleteMembersSample);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.ObsoleteMembersSample>.Equals(global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.NoComment!, other.NoComment!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Comment!, other.Comment!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
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
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.ObsoleteMembersSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? x, global::Generator.Equals.Tests.Classes.ObsoleteMembersSample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.ObsoleteMembersSample obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
