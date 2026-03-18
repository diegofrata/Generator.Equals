//HintName: global__Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class OrderedEqualityWithComparerSampleWithLengthComparer : global::System.IEquatable<global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? left, global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? left, global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer>.Equals(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.Classes.OrderedLengthEqualityComparer.Default).Equals(this.Values!, other.Values!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
            hashCode.Add(
                this.Values!,
                new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.Classes.OrderedLengthEqualityComparer.Default)
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? x, global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? x, global::Generator.Equals.Tests.Classes.OrderedEqualityWithComparerSampleWithLengthComparer? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!new global::Generator.Equals.OrderedEqualityComparer<global::System.String>(global::Generator.Equals.Tests.Classes.OrderedLengthEqualityComparer.Default).Equals(x.Values!, y.Values!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Values"));
                    var __xList = x.Values is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(x.Values);
                    var __yList = y.Values is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(y.Values);
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
