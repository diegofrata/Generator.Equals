using NUnit.Framework;

namespace Generator.Equals.Tests
{
    public abstract class EqualityTestCase
    {
        public abstract object Factory1();
        public virtual object Factory2() => Factory1();
        public virtual bool Expected => true;
        public abstract bool EqualsOperator(object value1, object value2);
        public abstract bool NotEqualsOperator(object value1, object value2);
        
        [Test]
        public void EqualsObject()
        {
            object value1 = Factory1();
            object value2 = Factory2();

            Assert.That(value1.Equals(value2), Is.EqualTo(Expected));
        }

        [Test]
        public void EqualsOperator()
        {
            var value1 = Factory1();
            var value2 = Factory2();

            Assert.That(EqualsOperator(value1, value2), Is.EqualTo(Expected));
        }


        [Test]
        public void NotEqualsOperator()
        {
            var value1 = Factory1();
            var value2 = Factory2();

            Assert.That(NotEqualsOperator(value1, value2), Is.Not.EqualTo(Expected));
        }


        [Test]
        public virtual void HashCode()
        {
            var value1 = Factory1();
            var value2 = Factory2();

            Assert.That(value1.GetHashCode() == value2.GetHashCode(), Is.EqualTo(Expected));
        }
    }
}
