//HintName: global__Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class OrderedEqualityWithComparer
    {
        partial class SampleWithCustomComparer : global::System.IEquatable<global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer? left,
                global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer? left, global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer>.Equals(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer? obj) => Equals((object?) obj);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            protected bool Equals(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparer.SampleWithCustomComparer? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return other.GetType() == this.GetType()
                    && new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(this.Names!, other.Names!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Names!,
                    new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase)
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
