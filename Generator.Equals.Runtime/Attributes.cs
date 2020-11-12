using System;

namespace Generator.Equals
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EquatableAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SequenceEqualityAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreEqualityAttribute : Attribute
    {
    }
}