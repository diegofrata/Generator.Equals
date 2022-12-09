//HintName: Generator.Equals.Tests.Structs.GenericParameterEquality.Sample_TName_ TAge_.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618

namespace Generator.Equals.Tests.Structs
{
    partial class GenericParameterEquality
    {
        partial struct Sample<TName, TAge> : global::System.IEquatable<Sample<TName, TAge>>
        {
            /// <summary>
            /// Indicates whether the object on the left is equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator ==(
                global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge> left,
                global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge> right) =>
                global::System.Collections.Generic.EqualityComparer<global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge>>.Default
                    .Equals(left, right);
            
            /// <summary>
            /// Indicates whether the object on the left is not equal to the object on the right.
            /// </summary>
            /// <param name="left">The left object</param>
            /// <param name="right">The right object</param>
            /// <returns>true if the objects are not equal; otherwise, false.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public static bool operator !=(global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge> left, global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge> right) =>
                !(left == right);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override bool Equals(object? obj) =>
                obj is global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge> o && Equals(o);
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public bool Equals(global::Generator.Equals.Tests.Structs.GenericParameterEquality.Sample<TName, TAge> other)
            {
                return true
                    && global::System.Collections.Generic.EqualityComparer<TName>.Default.Equals(this.Name!, other.Name!)
                    && global::System.Collections.Generic.EqualityComparer<TAge>.Default.Equals(this.Age!, other.Age!)
                    ;
            }
            
            /// <inheritdoc/>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public override int GetHashCode()
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    this.Name!,
                    global::System.Collections.Generic.EqualityComparer<TName>.Default);
                hashCode.Add(
                    this.Age!,
                    global::System.Collections.Generic.EqualityComparer<TAge>.Default);
                
                return hashCode.ToHashCode();
            }
        }
    }
}
