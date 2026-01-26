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
        /// When true, the generated Equals method will not call base.Equals(),
        /// treating this type as an equality root. Only properties defined
        /// directly on this type will be compared.
        /// </summary>
        public bool SkipBaseEquals { get; set; }
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class DefaultEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class OrderedEqualityAttribute : Attribute
    {
        public Type? ComparerType { get; }
        public string ComparerMemberName { get; }
        public StringComparison? StringComparison { get; }

        public OrderedEqualityAttribute()
        {
            ComparerMemberName = "Default";
        }

        public OrderedEqualityAttribute(StringComparison stringComparison)
        {
            StringComparison = stringComparison;
            ComparerMemberName = "Default";
        }

        public OrderedEqualityAttribute(Type comparerType, string comparerMemberName = "Default")
        {
            ComparerType = comparerType;
            ComparerMemberName = comparerMemberName;
        }
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class IgnoreEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class UnorderedEqualityAttribute : Attribute
    {
        public Type? ComparerType { get; }
        public string ComparerMemberName { get; }
        public StringComparison? StringComparison { get; }

        public UnorderedEqualityAttribute()
        {
            ComparerMemberName = "Default";
        }

        public UnorderedEqualityAttribute(StringComparison stringComparison)
        {
            StringComparison = stringComparison;
            ComparerMemberName = "Default";
        }

        public UnorderedEqualityAttribute(Type comparerType, string comparerMemberName = "Default")
        {
            ComparerType = comparerType;
            ComparerMemberName = comparerMemberName;
        }
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class ReferenceEqualityAttribute : Attribute
    {
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class SetEqualityAttribute : Attribute
    {
        public Type? ComparerType { get; }
        public string ComparerMemberName { get; }
        public StringComparison? StringComparison { get; }

        public SetEqualityAttribute()
        {
            ComparerMemberName = "Default";
        }

        public SetEqualityAttribute(StringComparison stringComparison)
        {
            StringComparison = stringComparison;
            ComparerMemberName = "Default";
        }

        public SetEqualityAttribute(Type comparerType, string comparerMemberName = "Default")
        {
            ComparerType = comparerType;
            ComparerMemberName = comparerMemberName;
        }
    }

    [GeneratedCode("Generator.Equals", "1.0.0.0")]
    [Conditional("GENERATOR_EQUALS")]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
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