using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;

namespace Generator.Equals.Tests.Infrastructure;

/// <summary>
/// Helper class to test equality (Equals, ==, !=, GetHashCode) using expression trees.
/// Eliminates the need for manual operator casts in test code.
/// </summary>
public static class EqualityAssert
{
    /// <summary>
    /// Verifies that two objects have the expected equality behavior.
    /// Tests Equals(), ==, !=, and GetHashCode() consistency.
    /// </summary>
    /// <typeparam name="T">The type being tested</typeparam>
    /// <param name="a">First object</param>
    /// <param name="b">Second object</param>
    /// <param name="expectedEqual">Whether the objects should be equal</param>
    public static void Verify<T>(T a, T b, bool expectedEqual) where T : class
    {
        // Test Equals method
        a.Equals(b).Should().Be(expectedEqual, "Equals(object) should return {0}", expectedEqual);

        // Test IEquatable<T>.Equals if implemented
        if (a is IEquatable<T> equatableA)
        {
            equatableA.Equals(b).Should().Be(expectedEqual, "IEquatable<T>.Equals should return {0}", expectedEqual);
        }

        // Test == operator using expression trees
        var equalityResult = InvokeEqualityOperator<T>(a, b);
        equalityResult.Should().Be(expectedEqual, "operator == should return {0}", expectedEqual);

        // Test != operator using expression trees
        var inequalityResult = InvokeInequalityOperator<T>(a, b);
        inequalityResult.Should().Be(!expectedEqual, "operator != should return {0}", !expectedEqual);

        // Test GetHashCode consistency
        if (expectedEqual)
        {
            a.GetHashCode().Should().Be(b.GetHashCode(), "GetHashCode should be equal for equal objects");
        }

        // Test Inequalities() consistency
        var inequalities = TryGetInequalities(typeof(T), a, b);
        if (inequalities != null)
        {
            if (expectedEqual)
                inequalities.Should().BeEmpty("Inequalities() should return empty when objects are equal");
            else if (HasCompleteInequalityCoverage(typeof(T)))
                inequalities.Should().NotBeEmpty("Inequalities() should return at least one inequality when objects are not equal");
        }
    }

    /// <summary>
    /// Verifies equality for struct types.
    /// </summary>
    /// <typeparam name="T">The struct type being tested</typeparam>
    /// <param name="a">First struct</param>
    /// <param name="b">Second struct</param>
    /// <param name="expectedEqual">Whether the structs should be equal</param>
    public static void VerifyStruct<T>(T a, T b, bool expectedEqual) where T : struct
    {
        // Test Equals method
        a.Equals(b).Should().Be(expectedEqual, "Equals(object) should return {0}", expectedEqual);

        // Test IEquatable<T>.Equals if implemented
        if (a is IEquatable<T> equatableA)
        {
            equatableA.Equals(b).Should().Be(expectedEqual, "IEquatable<T>.Equals should return {0}", expectedEqual);
        }

        // Test == operator using expression trees
        var equalityResult = InvokeStructEqualityOperator<T>(a, b);
        equalityResult.Should().Be(expectedEqual, "operator == should return {0}", expectedEqual);

        // Test != operator using expression trees
        var inequalityResult = InvokeStructInequalityOperator<T>(a, b);
        inequalityResult.Should().Be(!expectedEqual, "operator != should return {0}", !expectedEqual);

        // Test GetHashCode consistency
        if (expectedEqual)
        {
            a.GetHashCode().Should().Be(b.GetHashCode(), "GetHashCode should be equal for equal objects");
        }

        // Test Inequalities() consistency
        var inequalities = TryGetInequalities(typeof(T), a, b);
        if (inequalities != null)
        {
            if (expectedEqual)
                inequalities.Should().BeEmpty("Inequalities() should return empty when objects are equal");
            else if (HasCompleteInequalityCoverage(typeof(T)))
                inequalities.Should().NotBeEmpty("Inequalities() should return at least one inequality when objects are not equal");
        }
    }

    static bool InvokeEqualityOperator<T>(T? a, T? b) where T : class
    {
        var func = GetOrCreateEqualityOperator<T>();
        return func(a, b);
    }

    static bool InvokeInequalityOperator<T>(T? a, T? b) where T : class
    {
        var func = GetOrCreateInequalityOperator<T>();
        return func(a, b);
    }

    static bool InvokeStructEqualityOperator<T>(T a, T b) where T : struct
    {
        var func = GetOrCreateStructEqualityOperator<T>();
        return func(a, b);
    }

    static bool InvokeStructInequalityOperator<T>(T a, T b) where T : struct
    {
        var func = GetOrCreateStructInequalityOperator<T>();
        return func(a, b);
    }

    static Func<T?, T?, bool> GetOrCreateEqualityOperator<T>() where T : class
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public, null, [typeof(T), typeof(T)
        ], null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator ==");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T?, T?, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    static Func<T?, T?, bool> GetOrCreateInequalityOperator<T>() where T : class
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public, null, [typeof(T), typeof(T)
        ], null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator !=");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T?, T?, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    static Func<T, T, bool> GetOrCreateStructEqualityOperator<T>() where T : struct
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public, null, [typeof(T), typeof(T)
        ], null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator ==");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T, T, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    static Func<T, T, bool> GetOrCreateStructInequalityOperator<T>() where T : struct
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public, null, [typeof(T), typeof(T)
        ], null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator !=");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T, T, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    static bool HasCompleteInequalityCoverage(Type type)
    {
        // Walk base types: if any non-object base lacks [Equatable] (no EqualityComparer nested type),
        // the generated Inequalities() won't cover properties from that base, so we can't assert
        // that unequal objects always produce non-empty inequalities.
        var current = type.BaseType;
        while (current != null && current != typeof(object) && current != typeof(ValueType))
        {
            if (current.GetNestedType("EqualityComparer") == null)
                return false;
            current = current.BaseType;
        }
        return true;
    }

    static List<object>? TryGetInequalities(Type type, object? a, object? b)
    {
        try
        {
            var comparerType = type.GetNestedType("EqualityComparer");
            if (comparerType == null) return null;

            var defaultProp = comparerType.GetProperty("Default", BindingFlags.Static | BindingFlags.Public);
            if (defaultProp == null) return null;

            var comparerInstance = defaultProp.GetValue(null);
            var method = comparerType.GetMethod("Inequalities");
            if (method == null) return null;

            var pathParam = method.GetParameters()[2];
            var defaultPath = Activator.CreateInstance(pathParam.ParameterType);

            var result = (System.Collections.IEnumerable)method.Invoke(comparerInstance, [a, b, defaultPath])!;
            return result.Cast<object>().ToList();
        }
        catch
        {
            return null;
        }
    }
}
