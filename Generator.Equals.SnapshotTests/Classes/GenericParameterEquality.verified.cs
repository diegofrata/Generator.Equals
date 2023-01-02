//HintName: Generator.Equals.Tests.Classes.GenericParameterEquality.Sample_TName_ TAge_.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618

namespace Generator.Equals.Tests.Classes
{
    partial class GenericParameterEquality
    {
        partial class Sample<TName, TAge> : global::System.IEquatable<Sample<TName, TAge>>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>? left,
                global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>? left, global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj)
            {
                var other = obj as global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>;
                return
                    !ReferenceEquals(other, null) && this.GetType() == other.GetType()
                    && global::Generator.Equals.DefaultEqualityComparer<TName>.Default.Equals(this.Name!, other.Name!)
                    && global::Generator.Equals.DefaultEqualityComparer<TAge>.Default.Equals(this.Age!, other.Age!)
                    ;
            }
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Classes.GenericParameterEquality.Sample<TName, TAge>? other) =>
                Equals((object?) other);
            
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Name!,
                    global::Generator.Equals.DefaultEqualityComparer<TName>.Default);
                hashCode.Add(
                    this.Age!,
                    global::Generator.Equals.DefaultEqualityComparer<TAge>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
