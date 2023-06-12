using System;
using FluentAssertions;
using Xunit;

namespace Generator.Equals.Tests
{
    public abstract class EqualityTestCase
    {
        public abstract object Factory1();
        public virtual object Factory2() => Factory1();
        public virtual bool Expected => true;
        public abstract bool EqualsOperator(object value1, object value2);
        public abstract bool NotEqualsOperator(object value1, object value2);
        
        [Fact]
        public void TestEqualsObject()
        {
            var value1 = Factory1();
            var value2 = Factory2();
            var result = value1.Equals(value2);
            result.Should().Be(Expected);
        }

        [Fact]
        public void TestEqualsOperator()
        {
            var value1 = Factory1();
            var value2 = Factory2();
            var result = EqualsOperator(value1, value2);
            result.Should().Be(Expected);
        }

        [Fact]
        public void TestNotEqualsOperator()
        {
            var value1 = Factory1();
            var value2 = Factory2();
            var result = NotEqualsOperator(value1, value2);
            result.Should().NotBe(Expected);
        }

        [Fact]
        public virtual void TestHashCode()
        {
            var value1 = Factory1();
            var value2 = Factory2();
            var result = value1.GetHashCode() == value2.GetHashCode();
            result.Should().Be(Expected);
        }
    }

    public abstract class EqualityTestCase<T> : EqualityTestCase
    {
        public void TestEqualsEquatable()
        {
            var value1 = (IEquatable<T>)Factory1();
            var value2 = (IEquatable<T>)Factory2();
            var result = value1.Equals(value2);
            result.Should().Be(Expected);
        }
    }
}