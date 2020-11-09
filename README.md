# Generator.Equals
A source code generator for automatically implementing IEquatable&lt;T> using only attributes.

This is a work in progress and at moment only C# 9 records are implemented -- which is what I needed for my own use case! Support for classes and other functions will come soon.

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