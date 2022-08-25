using System;
using System.Diagnostics;

namespace Generator.Equals
{
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EquatableAttribute : Attribute
    {
        /// <summary>
        /// IEquatable will only be generated for explicitly defined attributes.
        /// </summary>
        public bool Explicit { get; set; }
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultEqualityAttribute : Attribute
    {
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderedEqualityAttribute : Attribute
    {
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreEqualityAttribute : Attribute
    {
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class UnorderedEqualityAttribute : Attribute
    {
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class ReferenceEqualityAttribute : Attribute
    {
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class SetEqualityAttribute : Attribute
    {
    }

    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomEqualityAttribute: Attribute
    {
        public Type EqualityType { get; }
        public string FieldOrPropertyName { get; }

        public CustomEqualityAttribute(Type equalityType, string fieldOrPropertyName = "Default")
        {
            EqualityType = equalityType;
            FieldOrPropertyName = fieldOrPropertyName;
        }
    }
}
