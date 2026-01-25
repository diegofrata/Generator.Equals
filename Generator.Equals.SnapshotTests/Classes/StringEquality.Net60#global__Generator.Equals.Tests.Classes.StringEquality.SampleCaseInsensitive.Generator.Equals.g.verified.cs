//HintName: global__Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class StringEquality
    {
        partial class SampleCaseInsensitive : global::System.IEquatable<global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive? left,
                global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive? right) =>
                global::Generator.Equals.DefaultEqualityComparer<global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive?>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive? left, global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive? right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                Equals(obj as global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive>.Equals(global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive? obj) => Equals((object?) obj);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            protected bool Equals(global::Generator.Equals.Tests.Classes.StringEquality.SampleCaseInsensitive? other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                
                return other.GetType() == this.GetType()
                    && global::System.StringComparer.CurrentCultureIgnoreCase.Equals(this.Name!, other.Name!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(this.GetType());
                hashCode.Add(
                    this.Name!,
                    global::System.StringComparer.CurrentCultureIgnoreCase
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
