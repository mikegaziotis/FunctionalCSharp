using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Roufe;

/// <summary>
/// Provides Scan operations for collections - similar to Fold but returns intermediate results.
/// Scan shows the progression of a fold operation, useful for cumulative calculations and state tracking.
/// </summary>
public static class ScanExtensions
{
    #region Basic Scan

    /// <summary>
    /// Performs a scan (cumulative fold) on a collection, returning all intermediate results.
    /// Like Fold/Aggregate, but returns a sequence of all intermediate states.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var cumulative = numbers.Scan(0, (acc, n) => acc + n);
    /// // Returns: [0, 1, 3, 6, 10]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<TAccumulator> Scan<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, TAccumulator> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var accumulator = seed;
        yield return accumulator;

        foreach (var item in source)
        {
            accumulator = func(accumulator, item);
            yield return accumulator;
        }
    }

    /// <summary>
    /// Performs a scan without a seed value, using the first element as the initial accumulator.
    /// Returns one fewer element than the input collection.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var cumulative = numbers.Scan((acc, n) => acc + n);
    /// // Returns: [1, 3, 6, 10]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<T> Scan<T>(
        this IEnumerable<T> source,
        Func<T, T, T> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            yield break;

        var accumulator = enumerator.Current;
        yield return accumulator;

        while (enumerator.MoveNext())
        {
            accumulator = func(accumulator, enumerator.Current);
            yield return accumulator;
        }
    }

    #endregion

    #region Scan Right (from right to left)

    /// <summary>
    /// Performs a right scan (cumulative fold from right to left) on a collection.
    /// The function is applied from the end of the collection to the beginning.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var result = numbers.ScanRight(0, (n, acc) => acc + n);
    /// // Returns: [10, 9, 7, 4, 0]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<TAccumulator> ScanRight<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<T, TAccumulator, TAccumulator> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = source as IList<T> ?? source.ToSafeArray();
        var results = new TAccumulator[items.Count + 1];
        results[items.Count] = seed;

        for (var i = items.Count - 1; i >= 0; i--)
        {
            results[i] = func(items[i], results[i + 1]);
        }

        return results;
    }

    #endregion

    #region Async Scan

    /// <summary>
    /// Async variant: Performs a scan with an async accumulator function.
    /// </summary>
    public static async Task<IEnumerable<TAccumulator>> ScanAsync<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, Task<TAccumulator>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            accumulator = await func(accumulator, item).ConfigureAwait(DefaultConfigureAwait);
            results.Add(accumulator);
        }

        return results;
    }

    /// <summary>
    /// Async variant: Performs a scan on a Task collection.
    /// </summary>
    public static async Task<IEnumerable<TAccumulator>> Scan<T, TAccumulator>(
        this Task<IEnumerable<T>> source,
        TAccumulator seed,
        Func<TAccumulator, T, TAccumulator> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = await source.ConfigureAwait(DefaultConfigureAwait);
        return items.Scan(seed, func);
    }

    /// <summary>
    /// Async variant: Performs a scan on a Task collection with an async accumulator function.
    /// </summary>
    public static async Task<IEnumerable<TAccumulator>> ScanAsync<T, TAccumulator>(
        this Task<IEnumerable<T>> source,
        TAccumulator seed,
        Func<TAccumulator, T, Task<TAccumulator>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = await source.ConfigureAwait(DefaultConfigureAwait);
        return await items.ScanAsync(seed, func).ConfigureAwait(DefaultConfigureAwait);
    }

    /// <summary>
    /// ValueTask variant: Performs a scan with an async accumulator function.
    /// </summary>
    public static async ValueTask<IEnumerable<TAccumulator>> ScanAsync<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, ValueTask<TAccumulator>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            accumulator = await func(accumulator, item).ConfigureAwait(DefaultConfigureAwait);
            results.Add(accumulator);
        }

        return results;
    }

    #endregion

    #region Result Scan

    /// <summary>
    /// Performs a scan with a Result-returning accumulator function.
    /// Stops and returns the error if the accumulator function returns a failure.
    /// Returns Success with all intermediate results if all operations succeed.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var result = numbers.ScanResult(0, (acc, n) =>
    ///     acc + n &lt; 10
    ///         ? Result.Success&lt;int, string&gt;(acc + n)
    ///         : Result.Failure&lt;int, string&gt;("Sum too large"));
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Result<IEnumerable<TAccumulator>, TE> ScanResult<T, TAccumulator, TE>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, Result<TAccumulator, TE>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            var result = func(accumulator, item);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TAccumulator>, TE>(result.Error);

            accumulator = result.Value;
            results.Add(accumulator);
        }

        return Result.Success<IEnumerable<TAccumulator>, TE>(results);
    }

    /// <summary>
    /// Async variant: Performs a scan with an async Result-returning accumulator function.
    /// </summary>
    public static async Task<Result<IEnumerable<TAccumulator>, TE>> ScanResultAsync<T, TAccumulator, TE>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, Task<Result<TAccumulator, TE>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            var result = await func(accumulator, item).ConfigureAwait(DefaultConfigureAwait);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TAccumulator>, TE>(result.Error);

            accumulator = result.Value;
            results.Add(accumulator);
        }

        return Result.Success<IEnumerable<TAccumulator>, TE>(results);
    }

    /// <summary>
    /// ValueTask variant: Performs a scan with an async Result-returning accumulator function.
    /// </summary>
    public static async ValueTask<Result<IEnumerable<TAccumulator>, TE>> ScanResultAsync<T, TAccumulator, TE>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, ValueTask<Result<TAccumulator, TE>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            var result = await func(accumulator, item).ConfigureAwait(DefaultConfigureAwait);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TAccumulator>, TE>(result.Error);

            accumulator = result.Value;
            results.Add(accumulator);
        }

        return Result.Success<IEnumerable<TAccumulator>, TE>(results);
    }

    #endregion

    #region Option Scan

    /// <summary>
    /// Performs a scan with an Option-returning accumulator function.
    /// Stops and returns None if the accumulator function returns None.
    /// Returns Some with all intermediate results if all operations return Some.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var result = numbers.ScanOption(0, (acc, n) =>
    ///     acc + n &lt; 10
    ///         ? Option&lt;int&gt;.From(acc + n)
    ///         : Option&lt;int&gt;.None);
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Option<IEnumerable<TAccumulator>> ScanOption<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, Option<TAccumulator>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            var option = func(accumulator, item);
            if (option.HasNoValue)
                return Option<IEnumerable<TAccumulator>>.None;

            accumulator = option.GetValueOrThrow();
            results.Add(accumulator);
        }

        return Option<IEnumerable<TAccumulator>>.From(results);
    }

    /// <summary>
    /// Async variant: Performs a scan with an async Option-returning accumulator function.
    /// </summary>
    public static async Task<Option<IEnumerable<TAccumulator>>> ScanOptionAsync<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, Task<Option<TAccumulator>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            var option = await func(accumulator, item).ConfigureAwait(DefaultConfigureAwait);
            if (option.HasNoValue)
                return Option<IEnumerable<TAccumulator>>.None;

            accumulator = option.GetValueOrThrow();
            results.Add(accumulator);
        }

        return Option<IEnumerable<TAccumulator>>.From(results);
    }

    /// <summary>
    /// ValueTask variant: Performs a scan with an async Option-returning accumulator function.
    /// </summary>
    public static async ValueTask<Option<IEnumerable<TAccumulator>>> ScanOptionAsync<T, TAccumulator>(
        this IEnumerable<T> source,
        TAccumulator seed,
        Func<TAccumulator, T, ValueTask<Option<TAccumulator>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TAccumulator>();
        var accumulator = seed;
        results.Add(accumulator);

        foreach (var item in source)
        {
            var option = await func(accumulator, item).ConfigureAwait(DefaultConfigureAwait);
            if (option.HasNoValue)
                return Option<IEnumerable<TAccumulator>>.None;

            accumulator = option.GetValueOrThrow();
            results.Add(accumulator);
        }

        return Option<IEnumerable<TAccumulator>>.From(results);
    }

    #endregion

    #region Helper Extensions

    /// <summary>
    /// Returns all partial sums of a collection (cumulative sum).
    /// Convenience method for Scan with addition.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var cumulative = numbers.CumulativeSum();
    /// // Returns: [0, 1, 3, 6, 10]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<int> CumulativeSum(this IEnumerable<int> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan(0, (acc, n) => acc + n);
    }

    /// <summary>
    /// Returns all partial sums of a long collection (cumulative sum).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<long> CumulativeSum(this IEnumerable<long> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan(0L, (acc, n) => acc + n);
    }

    /// <summary>
    /// Returns all partial sums of a decimal collection (cumulative sum).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<decimal> CumulativeSum(this IEnumerable<decimal> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan(0m, (acc, n) => acc + n);
    }

    /// <summary>
    /// Returns all partial products of a collection (cumulative product).
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4 };
    /// var cumulative = numbers.CumulativeProduct();
    /// // Returns: [1, 1, 2, 6, 24]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<int> CumulativeProduct(this IEnumerable<int> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan(1, (acc, n) => acc * n);
    }

    /// <summary>
    /// Returns all partial products of a long collection (cumulative product).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<long> CumulativeProduct(this IEnumerable<long> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan(1L, (acc, n) => acc * n);
    }

    /// <summary>
    /// Returns the running maximum of a collection.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 5, 3, 9, 2 };
    /// var runningMax = numbers.RunningMax();
    /// // Returns: [1, 5, 5, 9, 9]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<T> RunningMax<T>(this IEnumerable<T> source) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan((max, current) => current.CompareTo(max) > 0 ? current : max);
    }

    /// <summary>
    /// Returns the running minimum of a collection.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<T> RunningMin<T>(this IEnumerable<T> source) where T : IComparable<T>
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Scan((min, current) => current.CompareTo(min) < 0 ? current : min);
    }

    #endregion
}

