using System.Collections.Generic;

namespace Generator.Equals.Tests.Records
{
    public partial class CustomEquality
    {
        [Equatable]
        public partial record Sample
        {
            public Sample(string name1, string name2, string name3)
            {
                Name1 = name1;
                Name2 = name2;
                Name3 = name3;
            }

            [CustomEquality(typeof(Comparer1))] public string Name1 { get; }
            [CustomEquality(typeof(Comparer2), nameof(Comparer2.Instance))] public string Name2 { get; }
            [CustomEquality(typeof(LengthEqualityComparer))] public string Name3 { get; }
        }
    
        class Comparer1
        {
            public static readonly LengthEqualityComparer Default = new();
        }

        class Comparer2
        {
            public static readonly LengthEqualityComparer Instance = new();
        }

        class LengthEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string? x, string? y) => x?.Length == y?.Length;

            public int GetHashCode(string obj) => obj.Length.GetHashCode();
        }

    }
}


//#nullable enable
//#pragma warning disable CS0612,CS0618
//#pragma warning disable CS0436

//namespace Generator.Equals.Tests.Records
//{
//    partial class CustomEquality
//    {
//        partial record Sample
//        {
//            /// <inheritdoc/>
//            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
//            public virtual bool Equals(global::Generator.Equals.Tests.Records.CustomEquality.Sample? other)
//            {
//                return
//                    !ReferenceEquals(other, null) && EqualityContract == other.EqualityContract
//                    && global::Generator.Equals.Tests.Records.CustomEquality.Comparer1.Default.Equals(this.Name1!, other.Name1!)
//                    && global::Generator.Equals.Tests.Records.CustomEquality.Comparer2.Instance.Equals(this.Name2!, other.Name2!)
//                    && global::Generator.Equals.Tests.Records.CustomEquality.LengthEqualityComparer.Default.Equals(this.Name3!, other.Name3!)
//                    ;
//            }

//            /// <inheritdoc/>
//            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Generator.Equals", "1.0.0.0")]
//            public override int GetHashCode()
//            {
//                var hashCode = new global::System.HashCode();

//                hashCode.Add(this.EqualityContract);
//                hashCode.Add(
//                    this.Name1!,
//                    global::Generator.Equals.Tests.Records.CustomEquality.Comparer1.Default
//                );
//                hashCode.Add(
//                    this.Name2!,
//                    global::Generator.Equals.Tests.Records.CustomEquality.Comparer2.Instance
//                );
//                hashCode.Add(
//                    this.Name3!,
//                    global::Generator.Equals.Tests.Records.CustomEquality.LengthEqualityComparer.Default
//                );

//                return hashCode.ToHashCode();
//            }
//        }
//    }
//}
