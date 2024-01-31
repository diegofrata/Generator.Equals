using System;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EquatableAttribute : Attribute
    {
        /// <summary>
        /// IEquatable will only be generated for explicitly defined attributes.
        /// </summary>
        public bool Explicit { get; set; }

        /// <summary>
        /// Equality and hash code do not consider members of base classes.
        /// </summary>
        public bool IgnoreInheritedMembers { get; set; }
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class DefaultEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class OrderedEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UnorderedEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ReferenceEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SetEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringEqualityAttribute : Attribute
    {
        public StringComparison ComparisonType { get; }

        public StringEqualityAttribute(StringComparison comparisonType)
        {
            ComparisonType = comparisonType;
        }
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CustomEqualityAttribute : Attribute
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