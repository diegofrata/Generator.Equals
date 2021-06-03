using System;

namespace Generator.Equals
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EquatableAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OrderedEqualityAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreEqualityAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class UnorderedEqualityAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ReferenceEqualityAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class SetEqualityAttribute : Attribute
    {
    }


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
