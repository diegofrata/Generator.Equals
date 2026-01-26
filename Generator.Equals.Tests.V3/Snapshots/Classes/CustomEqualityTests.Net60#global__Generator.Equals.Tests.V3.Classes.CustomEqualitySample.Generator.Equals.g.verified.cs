//HintName: global__Generator.Equals.Tests.V3.Classes.CustomEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.V3.Classes
{
    partial class CustomEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(
            global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample? left,
            global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample? right) =>
            global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample?>.Default
                .Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample? left, global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample? right) =>
            !(left == right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample>.Equals(global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.V3.Classes.CustomEqualitySample? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && global::Generator.Equals.Tests.V3.Classes.Comparer1.Default.Equals(this.Name1!, other.Name1!)
                && global::Generator.Equals.Tests.V3.Classes.Comparer2.Instance.Equals(this.Name2!, other.Name2!)
                && new global::Generator.Equals.Tests.V3.Classes.CustomLengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
            hashCode.Add(
                this.Name1!,
                global::Generator.Equals.Tests.V3.Classes.Comparer1.Default
            );
            hashCode.Add(
                this.Name2!,
                global::Generator.Equals.Tests.V3.Classes.Comparer2.Instance
            );
            hashCode.Add(
                this.Name3!,
                new global::Generator.Equals.Tests.V3.Classes.CustomLengthEqualityComparer()
            );
            
            return hashCode.ToHashCode();
        }
    }
}
