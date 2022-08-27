//HintName: Generator.Equals.Tests.Records.GenericParameterEquality.Sample_TName_ TAge_.Generator.Equals.g.cs
namespace Generator.Equals.Tests.Records {
partial class GenericParameterEquality {
partial record Sample<TName, TAge> {
#nullable enable
#pragma warning disable CS0612,CS0618
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public virtual bool Equals(global::Generator.Equals.Tests.Records.GenericParameterEquality.Sample<TName, TAge>? other) {
return !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
&& global::System.Collections.Generic.EqualityComparer<TName>.Default.Equals(Name!, other.Name!)
&& global::System.Collections.Generic.EqualityComparer<TAge>.Default.Equals(Age!, other.Age!)
;
}
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            
hashCode.Add(this.EqualityContract);
hashCode.Add(this.Name!, global::System.Collections.Generic.EqualityComparer<TName>.Default);
hashCode.Add(this.Age!, global::System.Collections.Generic.EqualityComparer<TAge>.Default);
return hashCode.ToHashCode();
}
#pragma warning restore CS0612,CS0618
#nullable restore

}
}
}
