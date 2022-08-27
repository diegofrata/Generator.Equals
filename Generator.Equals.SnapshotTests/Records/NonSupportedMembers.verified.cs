//HintName: Generator.Equals.Tests.Records.NonSupportedMembers.Sample.Generator.Equals.g.cs
namespace Generator.Equals.Tests.Records {
partial class NonSupportedMembers {
partial record Sample {
#nullable enable
#pragma warning disable CS0612,CS0618
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public virtual bool Equals(global::Generator.Equals.Tests.Records.NonSupportedMembers.Sample? other) {
return !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
&& global::System.Collections.Generic.EqualityComparer<global::System.String>.Default.Equals(Name!, other.Name!)
;
}
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            
hashCode.Add(this.EqualityContract);
hashCode.Add(this.Name!, global::System.Collections.Generic.EqualityComparer<global::System.String>.Default);
return hashCode.ToHashCode();
}
#pragma warning restore CS0612,CS0618
#nullable restore

}
}
}
