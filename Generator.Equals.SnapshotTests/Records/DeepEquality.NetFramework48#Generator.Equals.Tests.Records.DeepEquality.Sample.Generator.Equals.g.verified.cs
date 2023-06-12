//HintName: Generator.Equals.Tests.Records.DeepEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class DeepEquality
    {
        partial record Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.DeepEquality.Sample? other)
            {
                return
                    !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                    && global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Records.DeepEquality.Person>.Default.Equals(this.Person!, other.Person!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.EqualityContract);
                hashCode.Add(
                    this.Person!,
                    global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Records.DeepEquality.Person>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
