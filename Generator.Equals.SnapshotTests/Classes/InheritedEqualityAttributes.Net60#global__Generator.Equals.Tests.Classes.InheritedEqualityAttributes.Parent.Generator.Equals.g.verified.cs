//HintName: global__Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class InheritedEqualityAttributes
    {
        partial class Parent : global::System.IEquatable<global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent? left,
                global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent? left, global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent>.Equals(global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent? obj) => Equals((object?) obj);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            protected bool Equals(global::Generator.Equals.Tests.Classes.InheritedEqualityAttributes.Parent? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return other.GetType() == this.GetType()
                    && global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default.Equals(this.Ints!, other.Ints!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Ints!,
                    global::Generator.Equals.OrderedEqualityComparer<global::System.Int32>.Default
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
