//HintName: global__Generator.Equals.Tests.Structs.DictionaryEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Structs
{
    partial struct DictionaryEqualitySample : global::System.IEquatable<global::Generator.Equals.Tests.Structs.DictionaryEqualitySample>
    {
        /// <summary>
        /// Indicates whether the object on the left is equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator ==(global::Generator.Equals.Tests.Structs.DictionaryEqualitySample left, global::Generator.Equals.Tests.Structs.DictionaryEqualitySample right) =>
            EqualityComparer.Default.Equals(left, right);
        
        /// <summary>
        /// Indicates whether the object on the left is not equal to the object on the right.
        /// </summary>
        /// <param name="left">The left object</param>
        /// <param name="right">The right object</param>
        /// <returns>true if the objects are not equal; otherwise, false.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public static bool operator !=(global::Generator.Equals.Tests.Structs.DictionaryEqualitySample left, global::Generator.Equals.Tests.Structs.DictionaryEqualitySample right) =>
            !EqualityComparer.Default.Equals(left, right);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override bool Equals(object? obj) =>
            obj is global::Generator.Equals.Tests.Structs.DictionaryEqualitySample o && EqualityComparer.Default.Equals(this, o);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.Structs.DictionaryEqualitySample other) =>
            EqualityComparer.Default.Equals(this, other);
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode() =>
            EqualityComparer.Default.GetHashCode(this);
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Structs.DictionaryEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Structs.DictionaryEqualitySample x, global::Generator.Equals.Tests.Structs.DictionaryEqualitySample y)
            {
                return true
                    && global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default.Equals(x.Properties!, y.Properties!)
                    ;
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Structs.DictionaryEqualitySample obj)
            {
                var hashCode = new global::System.HashCode();
                
                hashCode.Add(
                    obj.Properties!,
                    global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default
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
            public global::System.Collections.Generic.IEnumerable<(string Path, object? Left, object? Right)> Diff(global::Generator.Equals.Tests.Structs.DictionaryEqualitySample x, global::Generator.Equals.Tests.Structs.DictionaryEqualitySample y, string? path = null)
            {
                var __path = string.IsNullOrEmpty(path) ? "" : path + ".";
                
                if (!global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default.Equals(x.Properties!, y.Properties!))
                {
                    var __xDict = x.Properties;
                    var __yDict = y.Properties;
                    
                    if (__xDict is null && __yDict is not null)
                    {
                        foreach (var __kvp in __yDict)
                            yield return (__path + $"Properties[{__kvp.Key}]", null, __kvp.Value);
                    }
                    else if (__xDict is not null && __yDict is null)
                    {
                        foreach (var __kvp in __xDict)
                            yield return (__path + $"Properties[{__kvp.Key}]", __kvp.Value, null);
                    }
                    else if (__xDict is not null && __yDict is not null)
                    {
                        foreach (var __kvp in __xDict)
                        {
                            if (!__yDict.TryGetValue(__kvp.Key, out var __yVal))
                                yield return (__path + $"Properties[{__kvp.Key}]", __kvp.Value, null);
                            else if (!global::System.Object.Equals(__kvp.Value, __yVal))
                                yield return (__path + $"Properties[{__kvp.Key}]", __kvp.Value, __yVal);
                        }
                        foreach (var __kvp in __yDict)
                        {
                            if (!__xDict.ContainsKey(__kvp.Key))
                                yield return (__path + $"Properties[{__kvp.Key}]", null, __kvp.Value);
                        }
                    }
                }
            }
        }
    }
}
