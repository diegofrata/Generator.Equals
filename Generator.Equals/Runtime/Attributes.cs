using System;
using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Generator.Equals
{
    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    class EquatableAttribute : Attribute
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
    class DefaultEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class OrderedEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class IgnoreEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class UnorderedEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class ReferenceEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class SetEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    class CustomEqualityAttribute : Attribute
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