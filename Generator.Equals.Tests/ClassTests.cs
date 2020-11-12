using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Generator.Equals.Tests
{
    [TestFixture]
    [SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
    public partial class ClassTests
    {
        [TestFixture]
        public class StandardBehavior : RecordsTests
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
        public partial class GeneratedBehavior : RecordsTests
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
                        public Data(string name, int age)
                        {
                            Name = name;
                            Age = age;
                        }

                        public string Name { get; }
                        public int Age { get; }
                    }

                    public override object Factory() => new Data("Dave", 35);
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
                        
                        [SequenceEquality]
                        public string[] Addresses { get; }
                    }

                    public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
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
                        
                        [IgnoreEquality]
                        public int Age { get; }
                    }

                    public override object Factory() => new Data("Dave", 35);
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
                        
                        [SequenceEquality]
                        public string[] Addresses { get; }
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
                        
                        [IgnoreEquality]
                        public int Age { get; }
                    }

                    public override EqualConstraint Constraint(object value) => Is.EqualTo(value);
                    public override object Factory1() => new Data("Dave", 35);
                    public override object Factory2() => new Data("Dave", 77);
                }
            }
        }
    }
}
