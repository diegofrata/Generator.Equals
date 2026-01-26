//HintName: global__Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.V3.Structs
{
    partial struct StringEqualitySampleCaseInsensitive : global::System.IEquatable<global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(
            global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive left,
            global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive right) =>
            global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive>.Default
                .Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive left, global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive right) =>
            !(left == right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive o && Equals(o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.V3.Structs.StringEqualitySampleCaseInsensitive other)
        {
            return true
                && global::System.StringComparer.CurrentCultureIgnoreCase.Equals(this.Name!, other.Name!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Name!,
                global::System.StringComparer.CurrentCultureIgnoreCase
            );
            
            return hashCode.ToHashCode();
        }
    }
}
