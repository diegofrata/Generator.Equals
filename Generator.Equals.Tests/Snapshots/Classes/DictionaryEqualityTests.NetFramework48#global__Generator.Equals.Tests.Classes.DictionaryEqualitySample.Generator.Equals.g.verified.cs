//HintName: global__Generator.Equals.Tests.Classes.DictionaryEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Classes
{
    partial class DictionaryEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.Classes.DictionaryEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? left, global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? left, global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            Equals(obj as global::Generator.Equals.Tests.Classes.DictionaryEqualitySample);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        bool global::System.IEquatable<global::Generator.Equals.Tests.Classes.DictionaryEqualitySample>.Equals(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? obj) => Equals((object?) obj);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        protected bool Equals(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            
            return other.GetType() == this.GetType()
                && global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default.Equals(this.Properties!, other.Properties!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.GetType());
            hashCode.Add(
                this.Properties!,
                global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Classes.DictionaryEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? x, global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? x, global::Generator.Equals.Tests.Classes.DictionaryEqualitySample? y, global::Generator.Equals.MemberPath path = default)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return new global::Generator.Equals.Inequality(path, x, y);
                    yield break;
                }
                
                if (!global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default.Equals(x.Properties!, y.Properties!))
                {
                    var __propPath = path.Append(global::Generator.Equals.MemberPathSegment.Property("Properties"));
                    var __xDict = x.Properties;
                    var __yDict = y.Properties;
                    
                    if (__xDict is null && __yDict is not null)
                    {
                        foreach (var __kvp in __yDict)
                            yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), null, __kvp.Value);
                    }
                    else if (__xDict is not null && __yDict is null)
                    {
                        foreach (var __kvp in __xDict)
                            yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), __kvp.Value, null);
                    }
                    else if (__xDict is not null && __yDict is not null)
                    {
                        foreach (var __kvp in __xDict)
                        {
                            if (!__yDict.TryGetValue(__kvp.Key, out var __yVal))
                                yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), __kvp.Value, null);
                            else if (!global::System.Object.Equals(__kvp.Value, __yVal))
                                yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), __kvp.Value, __yVal);
                        }
                        foreach (var __kvp in __yDict)
                        {
                            if (!__xDict.ContainsKey(__kvp.Key))
                                yield return new global::Generator.Equals.Inequality(__propPath.Append(global::Generator.Equals.MemberPathSegment.Key(__kvp.Key)), null, __kvp.Value);
                        }
                    }
                }
            }
        }
    }
}
