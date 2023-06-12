//HintName: Generator.Equals.Tests.RecordsStructs.FieldEquality.Sample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordsStructs
{
    partial class FieldEquality
    {
        partial record struct Sample
        {
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.RecordsStructs.FieldEquality.Sample other)
            {
                return true
                    && global::Generator.Equals.OrderedEqualityComparer<string>.Default.Equals(this._addresses!, other._addresses!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this._addresses!,
                    global::Generator.Equals.OrderedEqualityComparer<string>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
