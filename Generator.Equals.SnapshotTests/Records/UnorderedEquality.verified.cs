//HintName: Generator.Equals.Tests.Records.UnorderedEquality.Sample.Generator.Equals.g.cs
namespace Generator.Equals.Tests.Records {
partial class UnorderedEquality {
partial record Sample {
#nullable enable
#pragma warning disable CS0612,CS0618
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public virtual bool Equals(global::Generator.Equals.Tests.Records.UnorderedEquality.Sample? other) {
return !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
&& global::Generator.Equals.UnorderedEqualityComparer<int>.Default.Equals(Properties!, other.Properties!)
;
}
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            
hashCode.Add(this.EqualityContract);
hashCode.Add(this.Properties!, global::Generator.Equals.UnorderedEqualityComparer<int>.Default);
return hashCode.ToHashCode();
}
#pragma warning restore CS0612,CS0618
#nullable restore

}
}
}
