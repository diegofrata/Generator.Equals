//HintName: Generator.Equals.SnapshotTests.MyRecord.Generator.Equals.g.cs
namespace Generator.Equals.SnapshotTests {
partial record MyRecord {
#nullable enable
#pragma warning disable CS0612,CS0618
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public virtual bool Equals(global::Generator.Equals.SnapshotTests.MyRecord? other) {
return !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
&& global::Generator.Equals.OrderedEqualityComparer<string>.Default.Equals(Fruits!, other.Fruits!)
;
}
/// <inheritdoc/>
[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
public override int GetHashCode() {
                var hashCode = new global::System.HashCode();
            
hashCode.Add(this.EqualityContract);
hashCode.Add(this.Fruits!, global::Generator.Equals.OrderedEqualityComparer<string>.Default);
return hashCode.ToHashCode();
}
#pragma warning restore CS0612,CS0618
#nullable restore

}
}
