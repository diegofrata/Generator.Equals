![Nuget](https://img.shields.io/nuget/v/Generator.Equals)
# Generator.Equals
A source code generator for automatically implementing IEquatable&lt;T&gt; using only attributes.

----------------
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
);
```

## Supported Comparers

Below is a list of all supported comparers. Would you like something else added? Let me know by raising an issue or sending a PR!

###Default

This is the comparer that's used when a property has no attributes indicating otherwise. The generated code will use 
```EqualityComparer<T>.Default``` for both equals and hashing operation.


###IgnoreEquality

```c#
[IgnoreEquality] 
public string Name { get; set; }
```

As the name implies, the property is ignored during Equals and GetHashCode calls!


###SequenceEquality

```c#
[SequenceEquality] 
public string[] Fruits { get; set; }
```

This equality comparer will compare properties based as a sequence instead of a reference. This works just like ```Enumerable.SequenceEqual```, which assumes both lists are of the same size and same sort.

Bear in mind that the property has to implement IEnumerable<T> and the that the items themselves implement equality (you can use Generator.Equals in the items too!).