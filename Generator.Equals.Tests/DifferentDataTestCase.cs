using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Generator.Equals.Tests
{
    public abstract class DifferentDataTestCase
    {
        public abstract object Factory1();
        public abstract object Factory2();

        public virtual EqualConstraint Constraint(object value) => Is.Not.EqualTo(value);

        [Test]
        public virtual void Equality()
        {
            var value1 = Factory1();
            var value2 = Factory2();
            
            Assert.That(value1, Constraint(value2));
        }
        
        [Test]
        public virtual void HashCode()
        {
            var value1 = Factory1();
            var value2 = Factory2();
            
            Assert.That(value1.GetHashCode(), Constraint(value2.GetHashCode()));
        }
    }
}