using NUnit.Framework;
using System;

namespace Generator.Equals.Tests.Classes
{
    [TestFixture]
    public partial class ObsoleteMembers
    {
        [Equatable]
        [Obsolete("Make sure the warning on the object model does not add warnings")]
        public partial class Data
        {
            public Data(string value)
            {
#pragma warning disable CS0612
                NoComment = value;
#pragma warning restore CS0612
#pragma warning disable CS0618
                Comment = value;
#pragma warning restore CS0618
                // Could not find a way to suppress this
// #pragma warning disable CS0619
                // CommentAndError = value;
// #pragma warning restore CS0619
            }

            [Obsolete]
            public string NoComment { get; }

            [Obsolete("Having a comment causes a different error code")]
            public string Comment { get; }

            // Could not find a way to suppress this
            // [Obsolete("Having a comment and IsError causes a different error code", true)]
            // public string CommentAndError { get; }
        }

#pragma warning disable CS0618
        [TestFixture]
        public class EqualsTest : EqualityTestCase
        {
            public override object Factory1() => new Data("Dave");
            public override object Factory2() => new Data("Dave");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }

        [TestFixture]
        public  class NotEqualsTest : EqualityTestCase
        {
            public override bool Expected => false;
            public override object Factory1() => new Data("Dave");
            public override object Factory2() => new Data("John");
            public override bool EqualsOperator(object value1, object value2) => (Data) value1 == (Data) value2;
            public override bool NotEqualsOperator(object value1, object value2) => (Data) value1 != (Data) value2;
        }
#pragma warning restore CS0618
    }
}