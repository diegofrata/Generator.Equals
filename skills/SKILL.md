---
name: generator.equals
description: Guidance for using Generator.Equals — a C# source generator for auto-generating Equals, GetHashCode, operators, and Diff/Inequalities methods via attributes.
packages: Generator.Equals
---

# Generator.Equals

C# source generator that auto-implements `IEquatable<T>`, `Equals()`, `GetHashCode()`, `==`/`!=` operators, and an `Inequalities()` diff method at compile time using attributes. No runtime reflection.

## Setup

Add both packages:

```xml
<PackageReference Include="Generator.Equals" PrivateAssets="all" />
<PackageReference Include="Generator.Equals.Runtime" />
```

Requires C# 9.0+. The type **must be `partial`**.

## Attributes Quick Reference

### Type-level

| Attribute | Purpose |
|-----------|---------|
| `[Equatable]` | Generates equality members for the type |
| `[Equatable(Explicit = true)]` | Only compare properties with explicit equality attributes |
| `[Equatable(IgnoreInheritedMembers = true)]` | Skip base class members entirely |

### Property/Field-level

| Attribute | Use When |
|-----------|----------|
| `[DefaultEquality]` | Default comparer. **Required on fields** (fields are excluded by default). |
| `[IgnoreEquality]` | Skip this member |
| `[OrderedEquality]` | Compare collection elements in order (like `SequenceEqual`) |
| `[UnorderedEquality]` | Compare collection elements ignoring order |
| `[SetEquality]` | Compare as sets (duplicates ignored) |
| `[ReferenceEquality]` | Compare by reference only |
| `[StringEquality(StringComparison.X)]` | String-specific comparison (string props only) |
| `[PrecisionEquality(0.001)]` | Tolerance-based numeric comparison |
| `[CustomEquality(typeof(MyComparer))]` | Custom `IEqualityComparer<T>` |

### Comparer constructor patterns (for collection and custom attributes)

```csharp
[OrderedEquality]                                                          // Default comparer
[OrderedEquality(typeof(MyComparer))]                                     // Type with static Default member
[OrderedEquality(typeof(StringComparer), nameof(StringComparer.Ordinal))] // Named static member
[OrderedEquality(StringComparison.OrdinalIgnoreCase)]                     // StringComparison shorthand
```

## Best Practices

### Always annotate collection properties

```csharp
// WRONG - produces diagnostic GE001
public List<int> Items { get; set; }

// RIGHT
[OrderedEquality]
public List<int> Items { get; set; }
```

### Use Explicit mode when only a few properties matter

```csharp
[Equatable(Explicit = true)]
partial class User
{
    [DefaultEquality] public string Id { get; set; }     // Compared
    public string DisplayName { get; set; }               // Ignored
    public DateTime LastLogin { get; set; }               // Ignored
}
```

### Mark fields explicitly

Fields are **not** included by default. You must annotate them:

```csharp
[DefaultEquality]
private int _version;
```

### Use the generated EqualityComparer

Every `[Equatable]` type gets a nested `EqualityComparer` class:

```csharp
// Use in dictionaries, HashSets, LINQ
var dict = new Dictionary<MyType, string>(MyType.EqualityComparer.Default);
var distinct = items.Distinct(MyType.EqualityComparer.Default);
```

### Use Inequalities for diff/audit

```csharp
foreach (var diff in MyType.EqualityComparer.Default.Inequalities(oldObj, newObj))
    Console.WriteLine(diff);
// Output: Name: John -> Jane
// Output: Addresses["home"].Street: 123 Main St -> 456 Oak Ave
```

Nested `[Equatable]` objects in collections are auto-drilled — you get per-field diffs, not "entire object changed".

## Common Pitfalls

1. **Non-partial type** (GE006) — The type MUST be declared `partial`.

2. **Manual Equals/GetHashCode** (GE005) — Do NOT override `Equals()` or `GetHashCode()` manually on an `[Equatable]` type. The generator owns these.

3. **Conflicting collection attributes** (GE007) — Only ONE of `[OrderedEquality]`, `[UnorderedEquality]`, `[SetEquality]` per property.

4. **Hash code instability** — `[UnorderedEquality]`, `[SetEquality]`, and `[PrecisionEquality]` return hash code 0 or exclude from hashing. Types using these are poor dictionary keys.

5. **Inheritance with [Equatable]** — The generator walks the full inheritance chain. If any ancestor has `[Equatable]` or overrides `Equals()`, it calls `base.Equals()`. Use `IgnoreInheritedMembers = true` to opt out.

6. **Overriding properties inherits attributes** — A `Child` overriding a `Parent`'s `[OrderedEquality] virtual int[] Values` automatically inherits `[OrderedEquality]`. Do NOT re-annotate unless you want to change behavior.

7. **`[StringEquality]` on non-string** (GE008) — Only valid on `string` properties.

8. **`[PrecisionEquality]` types** (GE010) — Only `float`, `double`, `decimal`, `int`, `long`, `short`, `sbyte` and their nullable variants.

## Examples

### Basic class

```csharp
[Equatable]
partial class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    [IgnoreEquality]
    public DateTime LastUpdated { get; set; }
}
```

### Record with collections

```csharp
[Equatable]
partial record Customer(
    string Id,
    string Name,
    [property: OrderedEquality] ImmutableArray<Address> Addresses,
    [property: UnorderedEquality] ImmutableDictionary<string, string> Tags
);
```

### Struct with custom comparer

```csharp
[Equatable]
partial struct Coordinate
{
    [PrecisionEquality(0.0001)]
    public double Latitude { get; set; }

    [PrecisionEquality(0.0001)]
    public double Longitude { get; set; }
}
```

### Inheritance

```csharp
[Equatable]
partial class Animal
{
    public string Species { get; set; }
}

[Equatable]
partial class Pet : Animal
{
    public string Name { get; set; }
    // Species is also compared via base.Equals()
}
```

### Diff / Inequalities

```csharp
var diffs = Customer.EqualityComparer.Default.Inequalities(before, after);
foreach (var d in diffs)
{
    // d.Path  — e.g., "Addresses[0].Street"
    // d.Left  — old value
    // d.Right — new value
    Console.WriteLine(d);
}
```

## Diagnostics

| Code | Description |
|------|-------------|
| GE001 | Collection missing equality attribute |
| GE002 | Complex property missing `[Equatable]` |
| GE003 | Collection element missing `[Equatable]` |
| GE005 | Manual Equals/GetHashCode with `[Equatable]` |
| GE006 | `[Equatable]` on non-partial type |
| GE007 | Conflicting equality attributes |
| GE008 | `[StringEquality]` on non-string |
| GE009 | Collection attribute on non-collection |
| GE010 | `[PrecisionEquality]` on unsupported type |

All diagnostics have automatic code fixes.
