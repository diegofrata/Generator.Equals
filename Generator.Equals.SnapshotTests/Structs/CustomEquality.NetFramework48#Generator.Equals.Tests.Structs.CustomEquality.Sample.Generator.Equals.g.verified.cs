//HintName: Generator.Equals.Tests.Structs.CustomEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial class CustomEquality
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
                global::Generator.Equals.Tests.Structs.CustomEquality.Sample left,
                global::Generator.Equals.Tests.Structs.CustomEquality.Sample right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Structs.CustomEquality.Sample>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Structs.CustomEquality.Sample left, global::Generator.Equals.Tests.Structs.CustomEquality.Sample right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                obj is global::Generator.Equals.Tests.Structs.CustomEquality.Sample o && Equals(o);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Structs.CustomEquality.Sample other)
            {
                return true
                    && global::Generator.Equals.Tests.Structs.CustomEquality.Comparer1.Default.Equals(this.Name1!, other.Name1!)
                    && global::Generator.Equals.Tests.Structs.CustomEquality.Comparer2.Instance.Equals(this.Name2!, other.Name2!)
                    && new global::Generator.Equals.Tests.Structs.CustomEquality.LengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Name1!,
                    global::Generator.Equals.Tests.Structs.CustomEquality.Comparer1.Default);
                hashCode.Add(
                    this.Name2!,
                    global::Generator.Equals.Tests.Structs.CustomEquality.Comparer2.Instance);
                hashCode.Add(
                    this.Name3!,
                    new global::Generator.Equals.Tests.Structs.CustomEquality.LengthEqualityComparer());
                
                return hashCode.ToHashCode();
            }
        }
    }
}
