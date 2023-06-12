//HintName: Generator.Equals.Tests.Structs.ObsoleteMembers.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial class ObsoleteMembers
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
                global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample left,
                global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample left, global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                obj is global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample o && Equals(o);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Structs.ObsoleteMembers.Sample other)
            {
                return true
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.NoComment!, other.NoComment!)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Comment!, other.Comment!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.NoComment!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                hashCode.Add(
                    this.Comment!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
