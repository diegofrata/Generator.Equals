//HintName: Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample.Generator.Equals.g.cs
namespace Generator.Equals.Tests.Classes {
partial class MultiplePartialsEquality {
partial class Sample : global::System.IEquatable<Sample> {
#nullable enable
#pragma warning disable CS0612,CS0618
/// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public static bool operator ==(global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample? left, global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample? right) => global::System.Collections.Generic.EqualityComparer<global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample?>.Default.Equals(left, right);
/// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public static bool operator !=(global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample? left, global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample? right) => !(left == right);
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public override bool Equals(object? obj) => Equals(obj as global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample);
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public bool Equals(global::Generator.Equals.Tests.Classes.MultiplePartialsEquality.Sample? other) {
return !ReferenceEquals(other, null) && this.GetType() == other.GetType()
&& global::Generator.Equals.OrderedEqualityComparer<string>.Default.Equals(Addresses!, other.Addresses!)
&& global::System.Collections.Generic.EqualityComparer<global::System.String>.Default.Equals(FirstName!, other.FirstName!)
&& global::System.Collections.Generic.EqualityComparer<global::System.String>.Default.Equals(LastName!, other.LastName!)
;
}
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            
hashCode.Add(this.GetType());
hashCode.Add(this.Addresses!, global::Generator.Equals.OrderedEqualityComparer<string>.Default);
hashCode.Add(this.FirstName!, global::System.Collections.Generic.EqualityComparer<global::System.String>.Default);
hashCode.Add(this.LastName!, global::System.Collections.Generic.EqualityComparer<global::System.String>.Default);
return hashCode.ToHashCode();
}
#pragma warning restore CS0612,CS0618
#nullable restore
}

}
}
