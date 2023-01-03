//HintName: Generator.Equals.Tests.Classes.BaseEquality.Manager.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618

namespace Generator.Equals.Tests.Classes
{
    partial class BaseEquality
    {
        partial class Manager : global::System.IEquatable<Manager>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.BaseEquality.Manager? left,
                global::Generator.Equals.Tests.Classes.BaseEquality.Manager? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.BaseEquality.Manager?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.BaseEquality.Manager? left, global::Generator.Equals.Tests.Classes.BaseEquality.Manager? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.BaseEquality.Manager);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Classes.BaseEquality.Manager? other)
            {
                return
                    base.Equals(other as global::Generator.Equals.Tests.Classes.BaseEquality.Person)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.Department!, other.Department!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(base.GetHashCode());
                hashCode.Add(
                    this.Department!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
