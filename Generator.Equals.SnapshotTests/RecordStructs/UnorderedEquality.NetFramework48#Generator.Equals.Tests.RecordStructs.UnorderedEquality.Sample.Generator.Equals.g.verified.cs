//HintName: Generator.Equals.Tests.RecordStructs.UnorderedEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial class UnorderedEquality
    {
        partial record struct Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.UnorderedEquality.Sample other)
            {
                return true
                    && global::Generator.Equals.UnorderedEqualityComparer<int>.Default.Equals(this.Properties!, other.Properties!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Properties!,
                    global::Generator.Equals.UnorderedEqualityComparer<int>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
