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
}
