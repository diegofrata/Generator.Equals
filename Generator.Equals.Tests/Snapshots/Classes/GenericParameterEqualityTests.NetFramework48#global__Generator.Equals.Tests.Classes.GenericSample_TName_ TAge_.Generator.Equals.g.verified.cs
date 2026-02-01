//HintName: global__Generator.Equals.Tests.Classes.GenericSample_TName_ TAge_.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class GenericSample<TName, TAge> : global::System.IEquatable<global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? left, global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? left, global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>>.Equals(global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && global::Generator.Equals.DefaultEqualityComparer<TName>.Default.Equals(this.Name!, other.Name!)
                && global::Generator.Equals.DefaultEqualityComparer<TAge>.Default.Equals(this.Age!, other.Age!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
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
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? x, global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge>? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.GenericSample<TName, TAge> obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
