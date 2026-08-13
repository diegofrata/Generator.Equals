//HintName: global__Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.RecordStructs
{
    partial record struct DictionaryEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public bool Equals(global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample other)
        {
            return true
                && global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default.Equals(this.Properties!, other.Properties!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(
                this.Properties!,
                global::Generator.Equals.DictionaryEqualityComparer<global::System.String, global::System.Int32>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample x, global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample y)
            {
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample obj)
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
            public global::System.Collections.Generic.IEnumerable<global::Generator.Equals.Inequality> Inequalities(global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample x, global::Generator.Equals.Tests.RecordStructs.DictionaryEqualitySample y, global::Generator.Equals.MemberPath path = default)
            {
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
