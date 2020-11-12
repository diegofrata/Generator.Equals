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
            public class ClassWithPrimitives : EqualityTestCase
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
            public class DistinctInstances : InequalityTestCase
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
            public partial class Equality : GeneratedBehavior
            {
                [TestFixture]
                public partial class ClassWithPrimitives : EqualityTestCase
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
                public partial class ClassWithArrayButNoEqualityAttribute : EqualityTestCase
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
                public partial class ClassWithSequenceEquality : EqualityTestCase
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
            }

            [TestFixture]
            public partial class Inequality : GeneratedBehavior
            {
                [TestFixture]
                public partial class ClassWithPrimitives : InequalityTestCase
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
                public partial class ClassWithArray : InequalityTestCase
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
            }
        }
    }
}