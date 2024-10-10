using System;
using System.Collections.Immutable;

namespace Generator.Equals.Models;

internal class EquatableImmutableDictionary<TKey, TValue>(ImmutableDictionary<TKey, TValue> Items)
    : IEquatable<EquatableImmutableDictionary<TKey, TValue>>
    where TKey : notnull
{
    public ImmutableDictionary<TKey, TValue> Items { get; set; } = Items;

    public EquatableImmutableDictionary() : this(ImmutableDictionary<TKey, TValue>.Empty)
    {
    }
    
    //TryGetValue
    public bool TryGetValue(TKey key, out TValue value) => Items.TryGetValue(key, out value);
    
    // Key access
    public TValue this[TKey key] => Items[key];

    public bool Equals(EquatableImmutableDictionary<TKey, TValue>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        foreach (var item in Items)
        {
            if (!other.Items.TryGetValue(item.Key, out var otherValue))
            {
                return false;
            }

            if (!Equals(item.Value, otherValue))
            {
                return false;
            }
        }

        return Items.Count == other.Items.Count;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EquatableImmutableDictionary<TKey, TValue>)obj);
    }

    public override int GetHashCode()
    {
        return Items.GetHashCode();
    }

    public static bool operator ==(
        EquatableImmutableDictionary<TKey, TValue>? left,
        EquatableImmutableDictionary<TKey, TValue>? right
    )
    {
        return Equals(left, right);
    }

    public static bool operator !=(
        EquatableImmutableDictionary<TKey, TValue>? left,
        EquatableImmutableDictionary<TKey, TValue>? right
    )
    {
        return !Equals(left, right);
    }

    // Implicit to and from ImmutableDictionary<TKey, TValue>
    public static implicit operator EquatableImmutableDictionary<TKey, TValue>(ImmutableDictionary<TKey, TValue> items) =>
        new(items);

    public static implicit operator ImmutableDictionary<TKey, TValue>(EquatableImmutableDictionary<TKey, TValue> items) =>
        items.Items;
}