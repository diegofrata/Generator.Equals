//HintName: Generator.Equals.Tests.Structs.PrimitiveEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618

namespace Generator.Equals.Tests.Structs
{
    partial class PrimitiveEquality
    {
        partial struct Sample : global::System.IEquatable<Sample>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample left,
                global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample right) =>
                global::System.Collections.Generic.EqualityComparer<global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample left, global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                obj is global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample o && Equals(o);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Structs.PrimitiveEquality.Sample other)
            {
                return true
                    && global::System.Collections.Generic.EqualityComparer<global::System.String>.Default.Equals(Name!, other.Name!)
                    && global::System.Collections.Generic.EqualityComparer<global::System.Int32>.Default.Equals(Age!, other.Age!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Name!,
                    global::System.Collections.Generic.EqualityComparer<global::System.String>.Default);
                hashCode.Add(
                    this.Age!,
                    global::System.Collections.Generic.EqualityComparer<global::System.Int32>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
