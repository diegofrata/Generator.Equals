#if NET5_0
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
namespace Generator.Equals.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
    public partial class RecordsTests
    {
        [TestFixture]
        public class StandardBehavior : RecordsTests
        {
            [TestFixture]
            public class RecordsWithArray : SameDataTestCase
            {
                public record Data(string Name, int Age, string[] Addresses);

                public override EqualConstraint Constraint(object value) => Is.Not.EqualTo(value);

                public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
            }

            [TestFixture]
            public class RecordsWithPrimitives : SameDataTestCase
            {
                public record Data(string Name, int Age);

                public override object Factory() => new Data("Dave", 35);
            }

            [TestFixture]
            public class DistinctRecords : DifferentDataTestCase
            {
                public record Data(string Name, int Age);

                public override object Factory1() => new Data("Dave", 35);
                public override object Factory2() => new Data("John", 25);
            }
        }

        [TestFixture]
        public partial class GeneratedBehavior : RecordsTests
        {
            [TestFixture]
            public partial class SameData : GeneratedBehavior
            {
                [TestFixture]
                public partial class RecordWithPrimitives : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data(string Name, int Age);

                    public override object Factory() => new Data("Dave", 35);
                }

                [TestFixture]
                public partial class GenericRecord : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data<TName, TAge>(TName Name, TAge Age);

                    public override object Factory() => new Data<string, int>("Dave", 35);
                }

                [TestFixture]
                public partial class RecordWithArrayButNoEqualityAttribute : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data(
                        string Name,
                        int Age,
                        string[] Addresses
                    );

                    public override EqualConstraint Constraint(object value) => Is.Not.EqualTo(value);

                    public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
                }

                [TestFixture]
                public partial class SequenceEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data(
                        string Name,
                        int Age,
                        [property: SequenceEquality] string[] Addresses1
                    );

                    public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
                }

                [TestFixture]
                public partial class NullableSequenceEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data(
                        [property: SequenceEquality] string[]? Addresses = null
                    );

                    public override object Factory() => new Data();
                }

                [TestFixture]
                public partial class IgnoreEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data(string Name, [property: IgnoreEquality] int Age);

                    public override object Factory() => new Data("Dave", 35);
                }
                
                [TestFixture]
                public partial class DictionaryEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data
                    {
                        [DictionaryEquality]
                        public Dictionary<string, int>? Properties { get; init; }
                    }

                    public override object Factory()
                    {
                        var randomSort = new Random();
                        
                        // This should generate objects with the same contents, but different orders, thus proving
                        // that dictionary equality is being used instead of the normal sequence equality.
                        return new Data
                        {
                            Properties = Enumerable
                                .Range(1, 1000)
                                .OrderBy(x => randomSort.NextDouble())
                                .ToDictionary(x => x.ToString(), x => x)
                        };
                    }
                }

                [TestFixture]
                public partial class UnorderedSequenceEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial record Data
                    {
                        [UnorderedSequenceEquality] public List<int>? Properties { get; set; }
                    }

                    public override object Factory()
                    {
                        var randomSort = new Random();

                        // This should generate objects with the same contents, but different orders, thus proving
                        // that dictionary equality is being used instead of the normal sequence equality.
                        return new Data
                        {
                            Properties = Enumerable
                                .Range(1, 1000)
                                .OrderBy(_ => randomSort.NextDouble())
                                .ToList()
                        };
                    }
                }
            }

            [TestFixture]
            public partial class DifferentData : GeneratedBehavior
            {
                [TestFixture]
                public partial class RecordWithPrimitives : DifferentDataTestCase
                {
                    [Equatable]
                    public partial record Data(string Name, int Age);

                    public override object Factory1() => new Data("Dave", 35);
                    public override object Factory2() => new Data("Joe", 77);
                }

                [TestFixture]
                public partial class SequenceEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial record Data([property: SequenceEquality] string[] Addresses);

                    public override object Factory1() => new Data(new[] {"10 Downing St"});
                    public override object Factory2() => new Data(new[] {"Bricklane"});
                }

                [TestFixture]
                public partial class IgnoreEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial record Data(string Name, [property: IgnoreEquality] int Age);

                    public override EqualConstraint Constraint(object value) => Is.EqualTo(value);
                    public override object Factory1() => new Data("Dave", 35);
                    public override object Factory2() => new Data("Dave", 77);
                }

                [TestFixture]
                public partial class DictionaryEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial record Data
                    {
                        [DictionaryEquality] public Dictionary<string, int>? Properties { get; init; }
                    }

                    public override object Factory1() => new Data
                    {
                        Properties = Enumerable.Range(1, 1000).ToDictionary(x => x.ToString(), x => x)
                    };

                    public override object Factory2() => new Data
                    {
                        Properties = Enumerable.Range(2, 999).ToDictionary(x => x.ToString(), x => x)
                    };
                }

                [TestFixture]
                public partial class UnorderedSequenceEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial record Data
                    {
                        [UnorderedSequenceEquality] public List<int>? Properties { get; set; }
                    }

                    public override object Factory1() => new Data
                    {
                        Properties = Enumerable.Range(1, 1000).ToList()
                    };

                    public override object Factory2() => new Data
                    {
                        Properties = Enumerable.Range(1, 999).ToList()
                    };
                }
            }
        }
    }
}
#endif
