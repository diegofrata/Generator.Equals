//HintName: global__Generator.Equals.Tests.Structs.GenericParameterEqualitySample_TName_ TAge_.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct GenericParameterEqualitySample<TName, TAge> : global::System.IEquatable<global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge>>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> left, global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> left, global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge>>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> x, global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> y)
            {
                return true
                    && global::Generator.Equals.DefaultEqualityComparer<TName>.Default.Equals(x.Name!, y.Name!)
                    && global::Generator.Equals.DefaultEqualityComparer<TAge>.Default.Equals(x.Age!, y.Age!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.GenericParameterEqualitySample<TName, TAge> obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Name!,
                    global::Generator.Equals.DefaultEqualityComparer<TName>.Default
                );
                hashCode.Add(
                    obj.Age!,
                    global::Generator.Equals.DefaultEqualityComparer<TAge>.Default
                );
                
                return hashCode.ToHashCode();
            }
        }
    }
}
