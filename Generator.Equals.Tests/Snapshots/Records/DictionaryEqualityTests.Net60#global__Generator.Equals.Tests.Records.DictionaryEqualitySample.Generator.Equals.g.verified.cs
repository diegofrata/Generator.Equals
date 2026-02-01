//HintName: global__Generator.Equals.Tests.Records.DictionaryEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record DictionaryEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.DictionaryEqualitySample? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default.Equals(this.Properties!, other.Properties!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Properties!,
                global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.DictionaryEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.DictionaryEqualitySample? x, global::Generator.Equals.Tests.Records.DictionaryEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.DictionaryEqualitySample obj)
            {
                return obj.GetHashCode();
            }
            
            /// <summary>
            /// Returns the differences between two instances.
            /// </summary>
            /// <param name="x">The first instance to compare.</param>
            /// <param name="y">The second instance to compare.</param>
            /// <param name="path">The base path for difference reporting.</param>
            /// <returns>An enumerable of differences, where each difference contains the path, left value, and right value.</returns>
            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
            public global::System.Collections.Generic.IEnumerable<(string Path, object? Left, object? Right)> Diff(global::Generator.Equals.Tests.Records.DictionaryEqualitySample? x, global::Generator.Equals.Tests.Records.DictionaryEqualitySample? y, string? path = null)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return (path ?? "", x, y);
                    yield break;
                }
                
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
