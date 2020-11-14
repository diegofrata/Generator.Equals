![Nuget](https://img.shields.io/nuget/v/Generator.Equals)
# Generator.Equals
A source code generator for automatically implementing IEquatable&lt;T&gt; using only attributes.

----------------
## Requirements

In order to use this library, you must:
* Use a target framework that supports .NET Standard >= 2.0 (eg. .NET Core 3.1 or .NET 5.0);
* Set your project's C# ```LangVersion``` property to 8.0 or higher.

## Usage

The below sample shows how to use Generator.Equals to override the default equality implementation for a C# record, enhancing it with the ability to determine the equality between the array contents of the record.

```c#
using Generator.Equals;

[Equatable]
partial record MyRecord(
    [property: SequenceEquality] string[] Fruits
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
Not using records? Generator.Equals also support classes.

```c#
using Generator.Equals;

[Equatable]
partial class MyClass
{
    [SequenceEquality] 
    public string[] Fruits { get; set; }
}
```

## Supported Comparers

Below is a list of all supported comparers. Would you like something else added? Let me know by raising an issue or sending a PR!

### Default

This is the comparer that's used when a property has no attributes indicating otherwise. The generated code will use 
```EqualityComparer<T>.Default``` for both equals and hashing operation.


### IgnoreEquality

```c#
[IgnoreEquality] 
public string Name { get; set; }
```

As the name implies, the property is ignored during Equals and GetHashCode calls!


### SequenceEquality

```c#
[SequenceEquality] 
public string[] Fruits { get; set; } // Fruits have to be in the same order for the array to be considered equal.
```

This equality comparer will compare properties as a sequence instead of a reference. This works just like ```Enumerable.SequenceEqual```, which assumes both lists are of the same size and same sort.

Bear in mind that the property has to implement IEnumerable<T> and the that the items themselves implement equality (you can use Generator.Equals in the items too!).

### UnorderedSequenceEquality

```c#
[UnorderedSequenceEquality] 
public string[] Fruits { get; set; } // Does not care about the order of the fruits!

[UnorderedSequenceEquality] 
public IDictionary<string, object> Properties { get; set; } // Works with dictionaries too!
```

This equality comparer will compare properties as an unordered sequence instead of a reference. This works just like ```Enumerable.SequenceEqual```, but it does not care about the order as long as the all values (including the repetitions) are present.

As with SequenceEquality, bear in mind that the property (or key and values if using a dictionary) has to implement IEnumerable<T> and the that the items themselves implement equality (you can use Generator.Equals in the items too!).


### ReferenceEquality

```c#
[ReferenceEquality] 
public string Name { get; set; } // Will only return true if strings are the same reference (eg. when used with string.Intern)
```

This will ignore whatever equality is implemented for a particular object and compare references instead.
