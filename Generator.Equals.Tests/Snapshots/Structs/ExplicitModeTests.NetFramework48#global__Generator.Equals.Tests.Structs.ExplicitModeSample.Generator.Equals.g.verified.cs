//HintName: global__Generator.Equals.Tests.Structs.ExplicitModeSample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct ExplicitModeSample : global::System.IEquatable<global::Generator.Equals.Tests.Structs.ExplicitModeSample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.ExplicitModeSample left, global::Generator.Equals.Tests.Structs.ExplicitModeSample right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.ExplicitModeSample left, global::Generator.Equals.Tests.Structs.ExplicitModeSample right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.ExplicitModeSample o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.ExplicitModeSample other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.ExplicitModeSample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.ExplicitModeSample x, global::Generator.Equals.Tests.Structs.ExplicitModeSample y)
            {
                return true
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default.Equals(x.Age!, y.Age!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.ExplicitModeSample obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Age!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.Int32>.Default
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
