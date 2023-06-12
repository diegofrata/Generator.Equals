//HintName: Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial class MultiplePartialsEquality
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
                global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample left,
                global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample left, global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                obj is global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample o && Equals(o);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Structs.MultiplePartialsEquality.Sample other)
            {
                return true
                    && global::Generator.Equals.OrderedEqualityComparer<string>.Default.Equals(this.Addresses!, other.Addresses!)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.FirstName!, other.FirstName!)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.LastName!, other.LastName!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Addresses!,
                    global::Generator.Equals.OrderedEqualityComparer<string>.Default);
                hashCode.Add(
                    this.FirstName!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                hashCode.Add(
                    this.LastName!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
