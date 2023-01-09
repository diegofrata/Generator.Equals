//HintName: Generator.Equals.Tests.Records.CustomEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class CustomEquality
    {
        partial record Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.CustomEquality.Sample? other)
            {
                return
                    !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                    && global::Generator.Equals.Tests.Records.CustomEquality.Comparer1.Default.Equals(this.Name1!, other.Name1!)
                    && global::Generator.Equals.Tests.Records.CustomEquality.Comparer2.Instance.Equals(this.Name2!, other.Name2!)
                    && new global::Generator.Equals.Tests.Records.CustomEquality.LengthEqualityComparer().Equals(this.Name3!, other.Name3!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.EqualityContract);
                hashCode.Add(
                    this.Name1!,
                    global::Generator.Equals.Tests.Records.CustomEquality.Comparer1.Default);
                hashCode.Add(
                    this.Name2!,
                    global::Generator.Equals.Tests.Records.CustomEquality.Comparer2.Instance);
                hashCode.Add(
                    this.Name3!,
                    new global::Generator.Equals.Tests.Records.CustomEquality.LengthEqualityComparer());
                
                return hashCode.ToHashCode();
            }
        }
    }
}
