//HintName: global__Generator.Equals.Tests.Structs.CustomEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct CustomEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.Structs.CustomEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.CustomEqualitySample left, global::Generator.Equals.Tests.Structs.CustomEqualitySample right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.CustomEqualitySample left, global::Generator.Equals.Tests.Structs.CustomEqualitySample right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.CustomEqualitySample o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.CustomEqualitySample other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.CustomEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.CustomEqualitySample x, global::Generator.Equals.Tests.Structs.CustomEqualitySample y)
            {
                return true
                    && global::Generator.Equals.Tests.Structs.CustomEqualityComparer1.Default.Equals(x.Name1!, y.Name1!)
                    && global::Generator.Equals.Tests.Structs.CustomEqualityComparer2.Instance.Equals(x.Name2!, y.Name2!)
                    && new global::Generator.Equals.Tests.Structs.CustomEqualityLengthEqualityComparer().Equals(x.Name3!, y.Name3!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.CustomEqualitySample obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Name1!,
                    global::Generator.Equals.Tests.Structs.CustomEqualityComparer1.Default
                );
                hashCode.Add(
                    obj.Name2!,
                    global::Generator.Equals.Tests.Structs.CustomEqualityComparer2.Instance
                );
                hashCode.Add(
                    obj.Name3!,
                    new global::Generator.Equals.Tests.Structs.CustomEqualityLengthEqualityComparer()
                );
                
                return hashCode.ToHashCode();
            }
            
            /// <summary>
            /// Returns the differences between two instances.
            /// </summary>
            /// <param name="x">The first instance to compare.</param>
            /// <param name="y">The second instance to compare.</param>
            /// <param name="path">The base path for difference reporting.</param>
            /// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public global::System.Collections.Generic.IEnumerable<(string Path, object? Left, object? Right)> Diff(global::Generator.Equals.Tests.Structs.CustomEqualitySample x, global::Generator.Equals.Tests.Structs.CustomEqualitySample y, string? path = null)
            {
                var __path = string.IsNullOrEmpty(path) ? "" : path + ".";
                
                if (!global::Generator.Equals.Tests.Structs.CustomEqualityComparer1.Default.Equals(x.Name1!, y.Name1!))
                    yield return (__path + "Name1", x.Name1, y.Name1);
                if (!global::Generator.Equals.Tests.Structs.CustomEqualityComparer2.Instance.Equals(x.Name2!, y.Name2!))
                    yield return (__path + "Name2", x.Name2, y.Name2);
                if (!new global::Generator.Equals.Tests.Structs.CustomEqualityLengthEqualityComparer().Equals(x.Name3!, y.Name3!))
                    yield return (__path + "Name3", x.Name3, y.Name3);
            }
        }
    }
}
