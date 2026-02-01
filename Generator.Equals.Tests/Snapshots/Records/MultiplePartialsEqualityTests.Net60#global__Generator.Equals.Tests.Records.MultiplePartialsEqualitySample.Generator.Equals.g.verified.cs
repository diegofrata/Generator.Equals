//HintName: global__Generator.Equals.Tests.Records.MultiplePartialsEqualitySample.Generator.Equals.g.cs

#nullable enable
#pragma warning disable CS0612,CS0618
#pragma warning disable CS0436

namespace Generator.Equals.Tests.Records
{
    partial record MultiplePartialsEqualitySample
    {
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public virtual bool Equals(global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample? other)
        {
            return
                !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
                && global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default.Equals(this.Addresses!, other.Addresses!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.FirstName!, other.FirstName!)
                && global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(this.LastName!, other.LastName!)
                ;
        }
        
        /// <inheritdoc/>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public override int GetHashCode()
        {
            var hashCode = new global::System.HashCode();
            
            hashCode.Add(this.EqualityContract);
            hashCode.Add(
                this.Addresses!,
                global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default
            );
            hashCode.Add(
                this.FirstName!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            hashCode.Add(
                this.LastName!,
                global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default
            );
            
            return hashCode.ToHashCode();
        }
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
        public sealed class EqualityComparer : global::System.Collections.Generic.IEqualityComparer<global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample>
        {
            public static EqualityComparer Default { get; } = new EqualityComparer();
            
            /// <inheritdoc/>
            public bool Equals(global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample? x, global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x is null || y is null) return false;
                
                return x.Equals(y);
            }
            
            /// <inheritdoc/>
            public int GetHashCode(global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample obj)
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
            public global::System.Collections.Generic.IEnumerable<(string Path, object? Left, object? Right)> Diff(global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample? x, global::Generator.Equals.Tests.Records.MultiplePartialsEqualitySample? y, string? path = null)
            {
                if (ReferenceEquals(x, y)) yield break;
                if (x is null || y is null)
                {
                    yield return (path ?? "", x, y);
                    yield break;
                }
                
                var __path = string.IsNullOrEmpty(path) ? "" : path + ".";
                
                if (!global::Generator.Equals.OrderedEqualityComparer<global::System.String>.Default.Equals(x.Addresses!, y.Addresses!))
                {
                    var __xList = x.Addresses is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(x.Addresses);
                    var __yList = y.Addresses is null ? new global::System.Collections.Generic.List<global::System.String>() : new global::System.Collections.Generic.List<global::System.String>(y.Addresses);
                    var __maxLen = global::System.Math.Max(__xList.Count, __yList.Count);
                    
                    for (var __i = 0; __i < __maxLen; __i++)
                    {
                        var __xVal = __i < __xList.Count ? (object?)__xList[__i] : null;
                        var __yVal = __i < __yList.Count ? (object?)__yList[__i] : null;
                        if (!global::System.Object.Equals(__xVal, __yVal))
                            yield return (__path + $"Addresses[{__i}]", __xVal, __yVal);
                    }
                }
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(x.FirstName!, y.FirstName!))
                    yield return (__path + "FirstName", x.FirstName, y.FirstName);
                if (!global::Generator.Equals.DefaultEqualityComparer<global::System.String>.Default.Equals(x.LastName!, y.LastName!))
                    yield return (__path + "LastName", x.LastName, y.LastName);
            }
        }
    }
}
