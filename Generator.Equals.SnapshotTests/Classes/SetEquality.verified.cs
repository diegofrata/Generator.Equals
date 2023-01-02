//HintName: Generator.Equals.Tests.Classes.SetEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618

namespace Generator.Equals.Tests.Classes
{
    partial class SetEquality
    {
        partial class Sample : global::System.IEquatable<Sample>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.SetEquality.Sample? left,
                global::Generator.Equals.Tests.Classes.SetEquality.Sample? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.SetEquality.Sample?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.SetEquality.Sample? left, global::Generator.Equals.Tests.Classes.SetEquality.Sample? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj)
            {
                var other = obj as global::Generator.Equals.Tests.Classes.SetEquality.Sample;
                return
                    !ReferenceEquals(other, null) && this.GetType() == other.GetType()
                    && global::Generator.Equals.SetEqualityComparer<int>.Default.Equals(this.Properties!, other.Properties!)
                    ;
            }
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Classes.SetEquality.Sample? other) =>
                Equals((object?) other);
            
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Properties!,
                    global::Generator.Equals.SetEqualityComparer<int>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
