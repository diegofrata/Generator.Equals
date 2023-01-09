//HintName: Generator.Equals.Tests.Records.MultiplePartialsEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial class MultiplePartialsEquality
    {
        partial record Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public virtual bool Equals(global::Generator.Equals.Tests.Records.MultiplePartialsEquality.Sample? other)
            {
                return
                    !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                    && global::Generator.Equals.OrderedEqualityComparer<string>.Default.Equals(this.Addresses!, other.Addresses!)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.FirstName!, other.FirstName!)
                    && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.LastName!, other.LastName!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.EqualityContract);
                hashCode.Add(
                    this.Addresses!,
                    global::Generator.Equals.OrderedEqualityComparer<string>.Default);
                hashCode.Add(
                    this.FirstName!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                hashCode.Add(
                    this.LastName!,
                    global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
