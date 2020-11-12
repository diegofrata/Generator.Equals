using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Generator.Equals.Tests
{
    public abstract class SameDataTestCase
    {
        public abstract object Factory();

        public virtual EqualConstraint Constraint(object value) => Is.EqualTo(value);

        [Test]
        public void Equality()
        {
            var value1 = Factory();
            var value2 = Factory();
            
            Assert.That(value1, Constraint(value2));
        }
        
        [Test]
        public void HashCode()
        {
            var value1 = Factory();
            var value2 = Factory();
            
            Assert.That(value1.GetHashCode(), Constraint(value2.GetHashCode()));
        }
    }
}