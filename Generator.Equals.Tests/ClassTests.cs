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
    public partial class ClassTests
    {
        [TestFixture]
        public class StandardBehavior : ClassTests
        {
            [TestFixture]
            public class ClassWithPrimitives : SameDataTestCase
            {
                public class Data
                {
                    public Data(string name, int age)
                    {
                        Name = name;
                        Age = age;
                    }

                    public string Name { get; }
                    public int Age { get; }
                }

                public override EqualConstraint Constraint(object value) => Is.Not.EqualTo(value);
                public override object Factory() => new Data("Dave", 35);
            }

            [TestFixture]
            public class DistinctInstances : DifferentDataTestCase
            {
                public class Data
                {
                    public Data(string name, int age)
                    {
                        Name = name;
                        Age = age;
                    }

                    public string Name { get; }
                    public int Age { get; }
                }

                public override object Factory1() => new Data("Dave", 35);
                public override object Factory2() => new Data("John", 25);
            }
        }

        [TestFixture]
        public partial class GeneratedBehavior : ClassTests
        {
            [TestFixture]
            public partial class SameData : GeneratedBehavior
            {
                [TestFixture]
                public partial class ClassWithPrimitives : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string name, int age, string? surname)
                        {
                            Name = name;
                            Age = age;
                            Surname = surname;
                        }

                        public string Name { get; }
                        public int Age { get; }
                        public string? Surname { get; }
                    }

                    public override object Factory() => new Data("Dave", 35, null);
                }

                [TestFixture]
                public partial class GenericClass : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data<TName, TAge>
                    {
                        public Data(TName name, TAge age)
                        {
                            Name = name;
                            Age = age;
                        }

                        public TName Name { get; }
                        public TAge Age { get; }
                    }

                    public override object Factory() => new Data<string, int>("Dave", 35);
                }

                [TestFixture]
                public partial class ClassWithArrayButNoEqualityAttribute : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string name, int age, string[] addresses)
                        {
                            Name = name;
                            Age = age;
                            Addresses = addresses;
                        }

                        public string Name { get; }
                        public int Age { get; }
                        public string[] Addresses { get; }
                    }

                    public override EqualConstraint Constraint(object value) => Is.Not.EqualTo(value);

                    public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
                }

                [TestFixture]
                public partial class SequenceEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string name, int age, string[] addresses)
                        {
                            Name = name;
                            Age = age;
                            Addresses = addresses;
                        }

                        public string Name { get; }
                        public int Age { get; }

                        [SequenceEquality] public string[] Addresses { get; }
                    }

                    public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
                }

                [TestFixture]
                public partial class NullableSequenceEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        [SequenceEquality] public string[]? Addresses { get; set; }
                    }

                    public override object Factory() => new Data();
                }

                [TestFixture]
                public partial class IgnoreEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string name, int age)
                        {
                            Name = name;
                            Age = age;
                        }

                        public string Name { get; }

                        [IgnoreEquality] public int Age { get; }
                    }

                    public override object Factory() => new Data("Dave", 35);
                }

                [TestFixture]
                public partial class DictionaryEquality : SameDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        [DictionaryEquality] public Dictionary<string, int>? Properties { get; set; }
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
                    public partial class Data
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
                public partial class ClassWithPrimitives : DifferentDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string name, int age)
                        {
                            Name = name;
                            Age = age;
                        }

                        public string Name { get; }
                        public int Age { get; }
                    }

                    public override object Factory1() => new Data("Dave", 35);
                    public override object Factory2() => new Data("Joe", 77);
                }

                [TestFixture]
                public partial class SequenceEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string[] addresses)
                        {
                            Addresses = addresses;
                        }

                        [SequenceEquality] public string[] Addresses { get; }
                    }

                    public override object Factory1() => new Data(new[] {"10 Downing St"});
                    public override object Factory2() => new Data(new[] {"Bricklane"});
                }


                [TestFixture]
                public partial class IgnoreEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        public Data(string name, int age)
                        {
                            Name = name;
                            Age = age;
                        }

                        public string Name { get; }

                        [IgnoreEquality] public int Age { get; }
                    }

                    public override EqualConstraint Constraint(object value) => Is.EqualTo(value);
                    public override object Factory1() => new Data("Dave", 35);
                    public override object Factory2() => new Data("Dave", 77);
                }

                [TestFixture]
                public partial class DictionaryEquality : DifferentDataTestCase
                {
                    [Equatable]
                    public partial class Data
                    {
                        [DictionaryEquality] public Dictionary<string, int>? Properties { get; set; }
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
                    public partial class Data
                    {
                        [UnorderedSequenceEquality] public List<int>? Properties { get; set; }
                    }

                    public override object Factory1() => new Data
                    {
                        Properties = Enumerable.Range(1, 1000).ToList()
                    };

                    public override object Factory2() => new Data
                    {
                        Properties = Enumerable.Range(1, 1001).ToList()
                    };
                }
            }
        }
    }
}
