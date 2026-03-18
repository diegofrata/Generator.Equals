//HintName: global__Generator.Equals.Tests.Classes.NullableEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class NullableEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.Classes.NullableEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.NullableEqualitySample? left, global::Generator.Equals.Tests.Classes.NullableEqualitySample? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.NullableEqualitySample? left, global::Generator.Equals.Tests.Classes.NullableEqualitySample? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.NullableEqualitySample);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.NullableEqualitySample>.Equals(global::Generator.Equals.Tests.Classes.NullableEqualitySample? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.NullableEqualitySample? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default.Equals(this.Addresses!, other.Addresses!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
            hashCode.Add(
                this.Addresses!,
                global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.NullableEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.NullableEqualitySample? x, global::Generator.Equals.Tests.Classes.NullableEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.NullableEqualitySample obj)
            {
                return obj.GetHashCode();
            }
            
            /// <summary>
            /// Returns the inequalities between two instances.
            /// </summary>
            /// <param name="x">The first instance to compare.</param>
            /// <param name="y">The second instance to compare.</param>
            /// <param name="path">The base path for difference reporting.</param>
            /// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Classes.NullableEqualitySample? x, global::Generator.Equals.Tests.Classes.NullableEqualitySample? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default.Equals(x.Addresses!, y.Addresses!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Addresses"));
                    var __xList = x.Addresses is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(x.Addresses);
                    var __yList = y.Addresses is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(y.Addresses);
                    var __maxLen = global::System.Math.Max(__xList.Count, __yList.Count);
                    
                    for (var __i = 0; __i < __maxLen; __i++)
                    {
                        var __xVal = __i < __xList.Count ? (object?)__xList[__i] : null;
                        var __yVal = __i < __yList.Count ? (object?)__yList[__i] : null;
                        if (!global::System.Object.Equals(__xVal, __yVal))
                            yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Index(__i)), __xVal, __yVal);
                    }
                }
            }
        }
    }
}
