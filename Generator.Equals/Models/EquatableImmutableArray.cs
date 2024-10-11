using System;
using System.Collections.Immutable;
using System.Linq;

namespace Generator.Equals.Models;

internal struct EquatableImmutableArray<T>(ImmutableArray<T> items) : IEquatable<EquatableImmutableArray<T>>
{
    public ImmutableArray<T> Items { get; set; } = items;

    public EquatableImmutableArray() : this(ImmutableArray<T>.Empty)
    {
    }

    public bool Equals(EquatableImmutableArray<T> other)
    {
        return Items.SequenceEqual(other.Items);
    }

    public override bool Equals(object? obj)
    {
        return obj is EquatableImmutableArray<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var item in Items)
        {
            hash.Add(item);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(EquatableImmutableArray<T> left, EquatableImmutableArray<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(EquatableImmutableArray<T> left, EquatableImmutableArray<T> right)
    {
        return !left.Equals(right);
    }

    // Implicit to and from ImmutableArray<T>
    public static implicit operator EquatableImmutableArray<T>(ImmutableArray<T> items) => new(items);
    public static implicit operator ImmutableArray<T>(EquatableImmutableArray<T> items) => items.Items;
}