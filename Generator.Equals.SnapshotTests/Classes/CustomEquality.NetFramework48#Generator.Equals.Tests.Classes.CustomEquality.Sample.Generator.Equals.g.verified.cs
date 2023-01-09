//HintName: Generator.Equals.Tests.Classes.CustomEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class CustomEquality
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
                global::Generator.Equals.Tests.Classes.CustomEquality.Sample? left,
                global::Generator.Equals.Tests.Classes.CustomEquality.Sample? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.CustomEquality.Sample?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.CustomEquality.Sample? left, global::Generator.Equals.Tests.Classes.CustomEquality.Sample? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.CustomEquality.Sample);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.CustomEquality.Sample>.Equals(global::Generator.Equals.Tests.Classes.CustomEquality.Sample? obj) => Equals((object?) obj);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            protected bool Equals(global::Generator.Equals.Tests.Classes.CustomEquality.Sample? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return other.GetType() == this.GetType()
                    && global::Generator.Equals.Tests.Classes.CustomEquality.Comparer1.Default.Equals(this.Name1!, other.Name1!)
                    && global::Generator.Equals.Tests.Classes.CustomEquality.Comparer2.Instance.Equals(this.Name2!, other.Name2!)
                    && new global::Generator.Equals.Tests.Classes.CustomEquality.LengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Name1!,
                    global::Generator.Equals.Tests.Classes.CustomEquality.Comparer1.Default);
                hashCode.Add(
                    this.Name2!,
                    global::Generator.Equals.Tests.Classes.CustomEquality.Comparer2.Instance);
                hashCode.Add(
                    this.Name3!,
                    new global::Generator.Equals.Tests.Classes.CustomEquality.LengthEqualityComparer());
                
                return hashCode.ToHashCode();
            }
        }
    }
}
