//HintName: global__Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class InheritedOverrideSkipped
    {
        partial class Child : global::System.IEquatable<global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child? left,
                global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child? left, global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child>.Equals(global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child? obj) => Equals((object?) obj);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            protected bool Equals(global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Child? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return base.Equals(other as global::Generator.Equals.Tests.Classes.InheritedOverrideSkipped.Parent)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(base.GetHashCode());
                
                return hashCode.ToHashCode();
            }
        }
    }
}
