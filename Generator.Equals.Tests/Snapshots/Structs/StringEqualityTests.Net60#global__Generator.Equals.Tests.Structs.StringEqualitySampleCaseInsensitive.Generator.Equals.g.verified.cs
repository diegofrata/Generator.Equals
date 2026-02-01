//HintName: global__Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct StringEqualitySampleCaseInsensitive : global::System.IEquatable<global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive left, global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive left, global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive x, global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive y)
            {
                return true
                    && global::System.StringComparer.CurrentCultureIgnoreCase.Equals(x.Name!, y.Name!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.StringEqualitySampleCaseInsensitive obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Name!,
                    global::System.StringComparer.CurrentCultureIgnoreCase
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
