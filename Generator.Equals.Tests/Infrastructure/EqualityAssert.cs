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
    }

    private static bool InvokeEqualityOperator<T>(T? a, T? b) where T : class
    {
        var func = GetOrCreateEqualityOperator<T>();
        return func(a, b);
    }

    private static bool InvokeInequalityOperator<T>(T? a, T? b) where T : class
    {
        var func = GetOrCreateInequalityOperator<T>();
        return func(a, b);
    }

    private static bool InvokeStructEqualityOperator<T>(T a, T b) where T : struct
    {
        var func = GetOrCreateStructEqualityOperator<T>();
        return func(a, b);
    }

    private static bool InvokeStructInequalityOperator<T>(T a, T b) where T : struct
    {
        var func = GetOrCreateStructInequalityOperator<T>();
        return func(a, b);
    }

    private static Func<T?, T?, bool> GetOrCreateEqualityOperator<T>() where T : class
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(T), typeof(T) }, null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator ==");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T?, T?, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    private static Func<T?, T?, bool> GetOrCreateInequalityOperator<T>() where T : class
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(T), typeof(T) }, null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator !=");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T?, T?, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    private static Func<T, T, bool> GetOrCreateStructEqualityOperator<T>() where T : struct
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(T), typeof(T) }, null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator ==");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T, T, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }

    private static Func<T, T, bool> GetOrCreateStructInequalityOperator<T>() where T : struct
    {
        var paramA = Expression.Parameter(typeof(T), "a");
        var paramB = Expression.Parameter(typeof(T), "b");

        var method = typeof(T).GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(T), typeof(T) }, null);
        if (method == null)
        {
            throw new InvalidOperationException($"Type {typeof(T).Name} does not define operator !=");
        }

        var call = Expression.Call(method, paramA, paramB);
        var lambda = Expression.Lambda<Func<T, T, bool>>(call, paramA, paramB);
        return lambda.Compile();
    }
}
