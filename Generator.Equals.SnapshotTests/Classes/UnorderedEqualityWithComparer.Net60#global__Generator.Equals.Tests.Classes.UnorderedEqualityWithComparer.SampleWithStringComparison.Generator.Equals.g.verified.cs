//HintName: global__Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class UnorderedEqualityWithComparer
    {
        partial class SampleWithStringComparison : global::System.IEquatable<global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison? left,
                global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison? left, global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison>.Equals(global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison? obj) => Equals((object?) obj);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            protected bool Equals(global::Generator.Equals.Tests.Classes.UnorderedEqualityWithComparer.SampleWithStringComparison? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return other.GetType() == this.GetType()
                    && new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase).Equals(this.Tags!, other.Tags!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Tags!,
                    new global::Generator.Equals.UnorderedEqualityComparer<global::System.String>(global::System.StringComparer.OrdinalIgnoreCase)
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
