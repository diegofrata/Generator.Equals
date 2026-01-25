//HintName: global__Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial class StringEquality
    {
        partial struct SampleCaseSensitive : global::System.IEquatable<global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive left,
                global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive left, global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                obj is global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive o && Equals(o);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Structs.StringEquality.SampleCaseSensitive other)
            {
                return true
                    && global::System.StringComparer.CurrentCulture.Equals(this.Name!, other.Name!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Name!,
                    global::System.StringComparer.CurrentCulture
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
