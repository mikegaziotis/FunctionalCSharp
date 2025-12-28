using System;
using System.Runtime.CompilerServices;

namespace Roufe;

/// <summary>
/// Functional extensions for composing, transforming, and manipulating functions.
/// </summary>
public static class FunctionExtensions
{
    #region Function Composition

    /// <summary>
    /// Composes two functions (f ∘ g). The result is a function that applies g first, then f.
    /// Mathematical notation: (f ∘ g)(x) = f(g(x))
    /// </summary>
    /// <example>
    /// var addOne = (int x) => x + 1;
    /// var double = (int x) => x * 2;
    /// var doubleThenAddOne = addOne.Compose(double);
    /// doubleThenAddOne(5); // returns 11 (5 * 2 + 1)
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, TR> Compose<T, TK, TR>(this Func<TK, TR> f, Func<T, TK> g)
    {
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(g);
        return x => f(g(x));
    }

    /// <summary>
    /// Chains two functions (f then g). The result is a function that applies f first, then g.
    /// This is the reverse of Compose and often more intuitive.
    /// </summary>
    /// <example>
    /// var addOne = (int x) => x + 1;
    /// var double = (int x) => x * 2;
    /// var addOneThenDouble = addOne.AndThen(double);
    /// addOneThenDouble(5); // returns 12 ((5 + 1) * 2)
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, TR> AndThen<T, TK, TR>(this Func<T, TK> f, Func<TK, TR> g)
    {
        ArgumentNullException.ThrowIfNull(f);
        ArgumentNullException.ThrowIfNull(g);
        return x => g(f(x));
    }

    #endregion

    #region Currying

    /// <summary>
    /// Converts a function with two parameters into a curried form.
    /// </summary>
    /// <example>
    /// Func&lt;int, int, int&gt; add = (a, b) => a + b;
    /// var curriedAdd = add.Curry();
    /// var add5 = curriedAdd(5);
    /// add5(3); // returns 8
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return x => y => func(x, y);
    }

    /// <summary>
    /// Converts a function with three parameters into a curried form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return x => y => z => func(x, y, z);
    }

    /// <summary>
    /// Converts a function with four parameters into a curried form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return x => y => z => w => func(x, y, z, w);
    }

    /// <summary>
    /// Converts a curried function with two parameters back to uncurried form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T1, T2, TResult> Uncurry<T1, T2, TResult>(this Func<T1, Func<T2, TResult>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y) => func(x)(y);
    }

    /// <summary>
    /// Converts a curried function with three parameters back to uncurried form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T1, T2, T3, TResult> Uncurry<T1, T2, T3, TResult>(
        this Func<T1, Func<T2, Func<T3, TResult>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y, z) => func(x)(y)(z);
    }

    /// <summary>
    /// Converts a curried function with four parameters back to uncurried form.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T1, T2, T3, T4, TResult> Uncurry<T1, T2, T3, T4, TResult>(
        this Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y, z, w) => func(x)(y)(z)(w);
    }

    #endregion

    #region Partial Application

    /// <summary>
    /// Partially applies the first argument to a two-parameter function.
    /// </summary>
    /// <example>
    /// Func&lt;int, int, int&gt; add = (a, b) => a + b;
    /// var add5 = add.Partial(5);
    /// add5(3); // returns 8
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T2, TResult> Partial<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1)
    {
        ArgumentNullException.ThrowIfNull(func);
        return x => func(arg1, x);
    }

    /// <summary>
    /// Partially applies the first argument to a three-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T2, T3, TResult> Partial<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, TResult> func, T1 arg1)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y) => func(arg1, x, y);
    }

    /// <summary>
    /// Partially applies the first two arguments to a three-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T3, TResult> Partial<T1, T2, T3, TResult>(
        this Func<T1, T2, T3, TResult> func, T1 arg1, T2 arg2)
    {
        ArgumentNullException.ThrowIfNull(func);
        return x => func(arg1, arg2, x);
    }

    /// <summary>
    /// Partially applies the first argument to a four-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T2, T3, T4, TResult> Partial<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func, T1 arg1)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y, z) => func(arg1, x, y, z);
    }

    /// <summary>
    /// Partially applies the first two arguments to a four-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T3, T4, TResult> Partial<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y) => func(arg1, arg2, x, y);
    }

    /// <summary>
    /// Partially applies the first three arguments to a four-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T4, TResult> Partial<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func, T1 arg1, T2 arg2, T3 arg3)
    {
        ArgumentNullException.ThrowIfNull(func);
        return x => func(arg1, arg2, arg3, x);
    }

    #endregion

    #region Flip

    /// <summary>
    /// Flips the order of arguments in a two-parameter function.
    /// </summary>
    /// <example>
    /// Func&lt;int, int, int&gt; subtract = (a, b) => a - b;
    /// var flippedSubtract = subtract.Flip();
    /// subtract(10, 3); // returns 7
    /// flippedSubtract(10, 3); // returns -7 (3 - 10)
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T2, T1, TResult> Flip<T1, T2, TResult>(this Func<T1, T2, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y) => func(y, x);
    }

    /// <summary>
    /// Flips the order of the first two arguments in a three-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T2, T1, T3, TResult> Flip<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y, z) => func(y, x, z);
    }

    /// <summary>
    /// Flips the order of the first two arguments in a four-parameter function.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T2, T1, T3, T4, TResult> Flip<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return (x, y, z, w) => func(y, x, z, w);
    }

    #endregion

    #region Identity and Constant

    /// <summary>
    /// Returns the identity function that returns its input unchanged.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, T> Identity<T>() => x => x;

    /// <summary>
    /// Creates a constant function that always returns the same value regardless of input.
    /// </summary>
    /// <example>
    /// var alwaysFive = FunctionExtensions.Constant&lt;string, int&gt;(5);
    /// alwaysFive("anything"); // returns 5
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, TResult> Constant<T, TResult>(TResult value) => _ => value;

    /// <summary>
    /// Creates a parameterless function that always returns the same value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T> Constant<T>(T value) => () => value;

    #endregion

    #region Apply and Invoke helpers

    /// <summary>
    /// Applies a value to a function (reverse function application).
    /// Useful for chaining in fluent style.
    /// </summary>
    /// <example>
    /// var addOne = (int x) => x + 1;
    /// var result = 5.Apply(addOne); // returns 6
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static TResult Apply<T, TResult>(this T value, Func<T, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return func(value);
    }

    /// <summary>
    /// Invokes a parameterless function. Useful for delayed execution.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static T Invoke<T>(this Func<T> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        return func();
    }

    #endregion

    #region Function Predicates

    /// <summary>
    /// Negates a predicate function.
    /// </summary>
    /// <example>
    /// Func&lt;int, bool&gt; isEven = x => x % 2 == 0;
    /// var isOdd = isEven.Negate();
    /// isOdd(5); // returns true
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, bool> Negate<T>(this Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return x => !predicate(x);
    }

    /// <summary>
    /// Combines two predicates with logical AND.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, bool> And<T>(this Func<T, bool> left, Func<T, bool> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return x => left(x) && right(x);
    }

    /// <summary>
    /// Combines two predicates with logical OR.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, bool> Or<T>(this Func<T, bool> left, Func<T, bool> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return x => left(x) || right(x);
    }

    /// <summary>
    /// Combines two predicates with logical XOR.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, bool> Xor<T>(this Func<T, bool> left, Func<T, bool> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return x => left(x) ^ right(x);
    }

    #endregion

    #region Action Composition

    /// <summary>
    /// Chains two actions to execute in sequence.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Action<T> AndThen<T>(this Action<T> first, Action<T> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);
        return x =>
        {
            first(x);
            second(x);
        };
    }

    /// <summary>
    /// Converts an Action into a Func that returns Unit.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<T, Unit> ToFunc<T>(this Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return x =>
        {
            action(x);
            return Unit.Value;
        };
    }

    /// <summary>
    /// Converts a parameterless Action into a Func that returns Unit.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Func<Unit> ToFunc(this Action action)
    {
        ArgumentNullException.ThrowIfNull(action);
        return () =>
        {
            action();
            return Unit.Value;
        };
    }

    #endregion
}

