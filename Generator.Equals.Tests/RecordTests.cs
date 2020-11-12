using System.Diagnostics.CodeAnalysis;
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
            public class RecordsWithArray : EqualityTestCase
            {
                public record Data(string Name, int Age, string[] Addresses);

                public override EqualConstraint Constraint(object value) => Is.Not.EqualTo(value);

                public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
            }

            [TestFixture]
            public class RecordsWithPrimitives : EqualityTestCase
            {
                public record Data(string Name, int Age);

                public override object Factory() => new Data("Dave", 35);
            }

            [TestFixture]
            public class DistinctRecords : InequalityTestCase
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
            public partial class Equality : GeneratedBehavior
            {
                [TestFixture]
                public partial class RecordWithPrimitives : EqualityTestCase
                {
                    [Equatable]
                    public partial record Data(string Name, int Age);

                    public override object Factory() => new Data("Dave", 35);
                }

                [TestFixture]
                public partial class RecordWithArrayButNoEqualityAttribute : EqualityTestCase
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
                public partial class RecordWithSequenceEquality : EqualityTestCase
                {
                    [Equatable]
                    public partial record Data(
                        string Name,
                        int Age,
                        [property: SequenceEquality] string[] Addresses1 
                    );

                    public override object Factory() => new Data("Dave", 35, new[] {"10 Downing St"});
                }
            }

            [TestFixture]
            public partial class Inequality : GeneratedBehavior
            {
                [TestFixture]
                public partial class RecordWithPrimitives : InequalityTestCase
                {
                    [Equatable]
                    public partial record Data(string Name, int Age);

                    public override object Factory1() => new Data("Dave", 35);
                    public override object Factory2() => new Data("Joe", 77);
                }

                [TestFixture]
                public partial class RecordWithArray : InequalityTestCase
                {
                    [Equatable]
                    public partial record Data(string[] Addresses);

                    public override object Factory1() => new Data(new[] {"10 Downing St"});
                    public override object Factory2() => new Data(new[] {"Bricklane"});
                }
            }
        }
    }
}