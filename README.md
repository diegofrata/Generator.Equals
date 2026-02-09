[![Nuget](https://img.shields.io/nuget/v/Generator.Equals)](https://www.nuget.org/packages/Generator.Equals/)
# Generator.Equals

A C# source generator that automatically implements `IEquatable<T>`, `Equals`, and `GetHashCode` — using just attributes.

Writing correct equality logic in C# is tedious, error-prone, and easy to forget when adding new properties. Generator.Equals eliminates that boilerplate by generating efficient, best-practice equality code at compile time.

### Why use Generator.Equals?

- **Zero boilerplate** — Mark your type with `[Equatable]` and the generator does the rest. No hand-written `Equals`, `GetHashCode`, or `==`/`!=` operators.
- **Collection-aware** — Compare arrays, lists, dictionaries, and sets by value out of the box with `[OrderedEquality]`, `[UnorderedEquality]`, and `[SetEquality]`.
- **Highly customizable** — Use `[CustomEquality]`, `[StringEquality]`, `[PrecisionEquality]`, `[ReferenceEquality]`, or `[IgnoreEquality]` to control comparison per-property.
- **Works everywhere** — Supports classes, structs, records, and record structs.
- **Inheritance-friendly** — Correctly chains `base.Equals()` across deep inheritance hierarchies and inherits equality attributes from overridden properties.
- **Compile-time only** — No runtime dependencies. The generator emits plain C# source code with no reflection or allocations.

----------------
## Requirements

In order to use this library, you must:
* Use a target framework that supports .NET Standard >= 2.0
* Set your project's C# ```LangVersion``` property to 9.0 or higher.

## Installation

Simply add the package `Generator.Equals` to your project. Keep reading to learn how to add the attributes to your types.

## Usage

The below sample shows how to use Generator.Equals to override the default equality implementation for a C# record, enhancing it with the ability to determine the equality between the array contents of the record.

```c#
using Generator.Equals;

[Equatable]
partial record MyRecord(
    [property: OrderedEquality] string[] Fruits
);

class Program
{
    static void Main(string[] args)
    {
        var record1 = new MyRecord(new[] {"banana", "apple"});
        var record2 = new MyRecord(new[] {"banana", "apple"});

        Console.WriteLine(record1 == record2);
    }
}
```
Need more than records? Generator.Equals supports properties (and fields) also across classes, structs and record structs.

```c#
using Generator.Equals;

[Equatable]
partial class MyClass
{
    [DefaultEquality] 
    private int _secretNumber = 42;

    [OrderedEquality] 
    public string[] Fruits { get; set; }
}

[Equatable]
partial struct MyStruct
{
    [OrderedEquality] 
    public string[] Fruits { get; set; }
}

[Equatable]
partial record struct MyRecordStruct(
    [property: OrderedEquality] string[] Fruits
);
```

## Supported Comparers

Below is a list of all supported comparers. Would you like something else added? Let me know by raising an issue or sending a PR!

### Default

This is the comparer that's used when a property has no attributes indicating otherwise. The generated code will use 
```EqualityComparer<T>.Default``` for both equals and hashing operation.

> _Fields are not used in comparison unless explicitly annotated. To enable the default comparison for a field, annotate it with the `DefaultEquality` attribute._

### IgnoreEquality

```c#
[IgnoreEquality] 
public string Name { get; set; }
```

As the name implies, the property is ignored during Equals and GetHashCode calls!


### OrderedEquality

```c#
[OrderedEquality]
public string[] Fruits { get; set; } // Fruits have to be in the same order for the array to be considered equal.
```

This equality comparer will compare properties as a sequence instead of a reference. This works just like ```Enumerable.SequenceEqual```, which assumes both lists are of the same size and same sort.

Bear in mind that the property has to implement IEnumerable<T> and the that the items themselves implement equality (you can use Generator.Equals in the items too!).

You can also specify a custom comparer for the elements:

```c#
// Using StringComparison for string collections
[OrderedEquality(StringComparison.OrdinalIgnoreCase)]
public string[] Tags { get; set; }

// Using a custom comparer type
[OrderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
public string[] Names { get; set; }

// Using a custom IEqualityComparer<T> with a Default static member
[OrderedEquality(typeof(MyCustomComparer))]
public string[] Values { get; set; }
```

### UnorderedEquality

```c#
[UnorderedEquality]
public string[] Fruits { get; set; } // Does not care about the order of the fruits!

[UnorderedEquality]
public IDictionary<string, object> Properties { get; set; } // Works with dictionaries too!
```

This equality comparer will compare properties as an unordered sequence instead of a reference. This works just like ```Enumerable.SequenceEqual```, but it does not care about the order as long as the all values (including the repetitions) are present.

As with OrderedEquality, bear in mind that the property (or key and values if using a dictionary) has to implement IEnumerable<T> and the that the items themselves implement equality (you can use Generator.Equals in the items too!).

You can also specify a custom comparer for the elements:

```c#
// Using StringComparison for string collections
[UnorderedEquality(StringComparison.OrdinalIgnoreCase)]
public List<string> Tags { get; set; }

// Using a custom comparer type
[UnorderedEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
public List<string> Names { get; set; }
```

### SetEquality

```c#
[SetEquality]
public HashSet<string> Fruits { get; set; } // Fruits can be in any order and duplicates are ignored
```

This equality comparer will do a set comparison, using ```SetEquals``` whenever the underlying collection implements `ISet<T>`, otherwise falling back to manually comparing both collections, which can be expensive for large collections.

You can also specify a custom comparer for the elements:

```c#
// Using StringComparison for string collections
[SetEquality(StringComparison.OrdinalIgnoreCase)]
public HashSet<string> Tags { get; set; }

// Using a custom comparer type
[SetEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))]
public HashSet<string> Names { get; set; }
```

Hashing always returns 0 for this type of equality.

### ReferenceEquality

```c#
[ReferenceEquality] 
public string Name { get; set; } // Will only return true if strings are the same reference (eg. when used with string.Intern)
```

This will ignore whatever equality is implemented for a particular object and compare references instead.

### StringEquality

```c#
[StringEquality(StringComparison.CurrentCulture | CurrentCultureIgnoreCase | InvariantCulture | InvariantCultureIgnoreCase | Ordinal | OrdinalIgnoreCase)]
public string Title { get; set; } // Will use the StringComparison set in constructor when comparing strings
```

### PrecisionEquality

```c#
[PrecisionEquality(0.001)]
public double Temperature { get; set; } // Equal if Math.Abs(a - b) < 0.001

[PrecisionEquality(5)]
public int Score { get; set; } // Equal if Math.Abs(a - b) < 5

[PrecisionEquality(0.001)]
public double? NullableValue { get; set; } // Handles nulls: both null = equal, one null = not equal
```

This equality comparer uses a tolerance (epsilon) to compare numeric values. Two values are considered equal when their absolute difference is less than the specified precision. This is excluded from `GetHashCode` since there is no stable bucketing under tolerance.

Supported types: `float`, `double`, `decimal`, `int`, `long`, `short`, `sbyte`, and their nullable variants.

### CustomEquality

```c#
class LengthEqualityComparer : IEqualityComparer<string>
{
    public static readonly LengthEqualityComparer Default = new();

    public bool Equals(string? x, string? y) => x?.Length == y?.Length;

    public int GetHashCode(string obj) => obj.Length.GetHashCode();
}

class NameEqualityComparer 
{
    public static readonly IEqualityComparer<string> Default = new SomeCustomComparer();
}


[CustomEquality(typeof(LengthEqualityComparer))] 
public string Name1 { get; set; } // Will use LengthEqualityComparer to compare the values of Name1.

[CustomEquality(typeof(NameEqualityComparer))] 
public string Name2 { get; set; } // Will use NameEqualityComparer.Default to compare values of Name2.

[CustomEquality(typeof(StringComparer), nameof(StringComparer.OrdinalIgnoreCase))] 
public string Name2 { get; set; } // Will use StringComparer.OrdinalIgnoreCase to compare values of Name2.
```

This attribute allows you to specify a custom comparer for a particular property. For it to work, the type passed as an
argument to CustomEqualityAttribute should fulfill AT LEAST one of the following:

* Have a static field/property named Default returning a valid IEqualityComparer<T> instance for the target type;
* Have a static field/property with the same name passed to the CustomComparerAttribute returning a valid IEqualityComparer<T> instance for the target type;
* Implement IEqualityComparer<T> and expose a parameterless constructor.

## Advanced Options

### Explicit Mode

The generator allows you to explicitly specify which properties are used to generate the `IEquatable`.  

To do this, set the `Explicit` property of `EquatableAttribute` to `true` and specify the required properties using `DefaultEqualityAttribute` or other attributes.
```cs
using Generator.Equals;

[Equatable(Explicit = true)]
partial class MyClass
{
    // Only this property will be used for equality!
    [DefaultEquality] 
    public string Name { get; set; } = "Konstantin"; 
    
    public string Description { get; set; } = "";
}
```


### Ignore Inherited Members

By default (`IgnoreInheritedMembers = false`), the generated code handles inherited members as follows:
- If any ancestor has `[Equatable]` or a custom `Equals` override, `base.Equals()` is called to delegate equality
- If NO ancestor has `[Equatable]`, all inherited properties from the entire chain are explicitly compared

Set `IgnoreInheritedMembers = true` to skip calling `base.Equals()` and ignore all inherited properties.
This is useful when you want to completely redefine equality for a derived class without considering
the base class's properties.

```cs
using Generator.Equals;

[Equatable]
partial class Person
{
    public string Name { get; set; }
}

[Equatable(IgnoreInheritedMembers = true)]
partial class Doctor : Person
{
    // Will NOT call base.Equals(), so Person.Name is not compared.
    // Only Id and Specialization are used for equality.
    public string Id { get; set; }
    public string Specialization { get; set; }
}
```

## Migrating from version 3

### Inherited Equality Attributes

Version 4 introduces support for inherited equality attributes on overridden properties, making repeating attributes
unnecessary. When a child class overrides a virtual property from a parent class, it now automatically inherits the
equality attribute (e.g., `[OrderedEquality]`) from the parent. You no longer need to redeclare attributes on overriding
properties.

```c#
[Equatable]
public partial class Parent
{
    [OrderedEquality]
    public virtual int[] Values { get; set; }
}

[Equatable]
public partial class Child : Parent
{
    // Automatically inherits [OrderedEquality] from Parent
    public override int[] Values { get; set; }
}
```

### Improved Inheritance Chain Detection

Version 4 improves how `base.Equals()` is called in inheritance hierarchies. Previously, generated code would only
call `base.Equals()` if the **immediate** base class had `[Equatable]`. Now, the generator walks the **entire**
inheritance chain and calls `base.Equals()` if:

1. **Any ancestor** has the `[Equatable]` attribute, OR
2. **Any ancestor** has manually overridden `Equals(object)`

This fixes scenarios where equality was incorrectly skipped in multi-level inheritance:

```c#
[Equatable]
public partial class GrandParent
{
    public string Name { get; set; }
}

// No [Equatable] - inherits GrandParent's Equals
public class Parent : GrandParent
{
    public int Age { get; set; }
}

[Equatable]
public partial class Child : Parent
{
    public string School { get; set; }
}
```

**Before (v3):** `Child.Equals()` did NOT call `base.Equals()` because `Parent` lacks `[Equatable]`.
Only `School` was compared, ignoring `Name`.

**After (v4):** `Child.Equals()` calls `base.Equals()` because `GrandParent` has `[Equatable]`.
Both `School` and `Name` are compared correctly.

### Ignore Inherited Members

The `IgnoreInheritedMembers` property controls how inherited members are handled:

| IgnoreInheritedMembers | Any Ancestor has [Equatable] | Behavior |
|------------------------|------------------------------|----------|
| `true`  | N/A | Compare only declared members, type check, no `base.Equals()` |
| `false` | Yes | Call `base.Equals()`, let ancestor handle its members |
| `false` | No  | Type check + compare ALL inherited properties from entire chain |

```c#
[Equatable(IgnoreInheritedMembers = true)]
public partial class Child : Parent
{
    // Will NOT call base.Equals() even if Parent has [Equatable]
    // Only properties defined in Child are compared
    public string School { get; set; }
}
```

## Migrating from version 2

Migrating to version 3 is very straightforward.

1. Ensure projects are targeting C# 9.0 or latter using the MSBuild property `LangVersion`.
2. Be aware that `IEquatable<T>` for classes is now implemented explicitly in order to support deep equality. As a result, the method `Equals(T)` method is no longer marked as public. Most code should still work, requiring only to be recompiled as the ABI has changed.

If you have an existing project using `Generator.Equals` and don't need any of the new features, you can still use version 2.x. The differences are minimal between both major versions.
