using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Roufe;

/// <summary>
/// Provides Partition operations for splitting collections of Results and Options.
/// Partition separates successes from failures, or values from nones.
/// </summary>
public static class PartitionExtensions
{
    #region Result Partition

    /// <summary>
    /// Partitions a collection of Results into successes and failures.
    /// Returns a tuple with two collections: (successes, failures).
    /// </summary>
    /// <example>
    /// var results = new[]
    /// {
    ///     Result.Success&lt;int, string&gt;(1),
    ///     Result.Failure&lt;int, string&gt;("error"),
    ///     Result.Success&lt;int, string&gt;(3)
    /// };
    /// var (successes, failures) = results.Partition();
    /// // successes = [1, 3]
    /// // failures = ["error"]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (IEnumerable<T> successes, IEnumerable<TE> failures) Partition<T, TE>(
        this IEnumerable<Result<T, TE>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var result in source)
        {
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes, failures);
    }

    /// <summary>
    /// Partitions a collection of Results into successes and failures.
    /// Returns separate lists instead of a tuple for more convenient access.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static PartitionResult<T, TE> PartitionToLists<T, TE>(
        this IEnumerable<Result<T, TE>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var result in source)
        {
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return new PartitionResult<T, TE>(successes, failures);
    }

    /// <summary>
    /// Async variant: Partitions a collection of Task Results.
    /// </summary>
    public static async Task<(IEnumerable<T> successes, IEnumerable<TE> failures)> Partition<T, TE>(
        this IEnumerable<Task<Result<T, TE>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var taskResult in source)
        {
            var result = await taskResult.ConfigureAwait(DefaultConfigureAwait);
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes, failures);
    }

    /// <summary>
    /// Async variant: Partitions a Task collection of Results.
    /// </summary>
    public static async Task<(IEnumerable<T> successes, IEnumerable<TE> failures)> Partition<T, TE>(
        this Task<IEnumerable<Result<T, TE>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var results = await source.ConfigureAwait(DefaultConfigureAwait);
        return results.Partition();
    }

    /// <summary>
    /// ValueTask variant: Partitions a collection of ValueTask Results.
    /// </summary>
    public static async ValueTask<(IEnumerable<T> successes, IEnumerable<TE> failures)> Partition<T, TE>(
        this IEnumerable<ValueTask<Result<T, TE>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var taskResult in source)
        {
            var result = await taskResult.ConfigureAwait(DefaultConfigureAwait);
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes, failures);
    }

    #endregion

    #region Option Partition

    /// <summary>
    /// Partitions a collection of Options into values and nones.
    /// Returns a tuple with the values collection and the count of nones.
    /// </summary>
    /// <example>
    /// var options = new[]
    /// {
    ///     Option&lt;int&gt;.From(1),
    ///     Option&lt;int&gt;.None,
    ///     Option&lt;int&gt;.From(3)
    /// };
    /// var (values, noneCount) = options.Partition();
    /// // values = [1, 3]
    /// // noneCount = 1
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (IEnumerable<T> values, int noneCount) Partition<T>(
        this IEnumerable<Option<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var option in source)
        {
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return (values, noneCount);
    }

    /// <summary>
    /// Partitions a collection of Options into values and nones.
    /// Returns a PartitionOption object for more convenient access.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static PartitionOption<T> PartitionToResult<T>(
        this IEnumerable<Option<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var option in source)
        {
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return new PartitionOption<T>(values, noneCount);
    }

    /// <summary>
    /// Async variant: Partitions a collection of Task Options.
    /// </summary>
    public static async Task<(IEnumerable<T> values, int noneCount)> Partition<T>(
        this IEnumerable<Task<Option<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var taskOption in source)
        {
            var option = await taskOption.ConfigureAwait(DefaultConfigureAwait);
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return (values, noneCount);
    }

    /// <summary>
    /// Async variant: Partitions a Task collection of Options.
    /// </summary>
    public static async Task<(IEnumerable<T> values, int noneCount)> Partition<T>(
        this Task<IEnumerable<Option<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var options = await source.ConfigureAwait(DefaultConfigureAwait);
        return options.Partition();
    }

    /// <summary>
    /// ValueTask variant: Partitions a collection of ValueTask Options.
    /// </summary>
    public static async ValueTask<(IEnumerable<T> values, int noneCount)> Partition<T>(
        this IEnumerable<ValueTask<Option<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var taskOption in source)
        {
            var option = await taskOption.ConfigureAwait(DefaultConfigureAwait);
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return (values, noneCount);
    }

    #endregion

    #region Conditional Partition (Result)

    /// <summary>
    /// Partitions a collection based on a predicate that returns a Result.
    /// Items that pass the predicate become successes, failures become errors.
    /// </summary>
    /// <example>
    /// var numbers = new[] { 1, 2, 3, 4, 5 };
    /// var (evens, odds) = numbers.PartitionWith(n =>
    ///     n % 2 == 0
    ///         ? Result.Success&lt;int, int&gt;(n)
    ///         : Result.Failure&lt;int, int&gt;(n));
    /// // evens = [2, 4]
    /// // odds = [1, 3, 5]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (IEnumerable<T> successes, IEnumerable<TE> failures) PartitionWith<TSource, T, TE>(
        this IEnumerable<TSource> source,
        Func<TSource, Result<T, TE>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var item in source)
        {
            var result = selector(item);
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes, failures);
    }

    /// <summary>
    /// Async variant: Partitions a collection with an async Result-returning predicate.
    /// </summary>
    public static async Task<(IEnumerable<T> successes, IEnumerable<TE> failures)> PartitionWithAsync<TSource, T, TE>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<Result<T, TE>>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var item in source)
        {
            var result = await selector(item).ConfigureAwait(DefaultConfigureAwait);
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes, failures);
    }

    /// <summary>
    /// ValueTask variant: Partitions a collection with an async Result-returning predicate.
    /// </summary>
    public static async ValueTask<(IEnumerable<T> successes, IEnumerable<TE> failures)> PartitionWithAsync<TSource, T, TE>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<Result<T, TE>>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var successes = new List<T>();
        var failures = new List<TE>();

        foreach (var item in source)
        {
            var result = await selector(item).ConfigureAwait(DefaultConfigureAwait);
            if (result.IsSuccess)
                successes.Add(result.Value);
            else
                failures.Add(result.Error);
        }

        return (successes, failures);
    }

    #endregion

    #region Conditional Partition (Option)

    /// <summary>
    /// Partitions a collection based on a predicate that returns an Option.
    /// Items that return Some become values, None become counted.
    /// </summary>
    /// <example>
    /// var strings = new[] { "1", "2", "invalid", "3" };
    /// var (numbers, invalidCount) = strings.PartitionWith(s =>
    ///     int.TryParse(s, out var n)
    ///         ? Option&lt;int&gt;.From(n)
    ///         : Option&lt;int&gt;.None);
    /// // numbers = [1, 2, 3]
    /// // invalidCount = 1
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (IEnumerable<T> values, int noneCount) PartitionWith<TSource, T>(
        this IEnumerable<TSource> source,
        Func<TSource, Option<T>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var item in source)
        {
            var option = selector(item);
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return (values, noneCount);
    }

    /// <summary>
    /// Async variant: Partitions a collection with an async Option-returning predicate.
    /// </summary>
    public static async Task<(IEnumerable<T> values, int noneCount)> PartitionWithAsync<TSource, T>(
        this IEnumerable<TSource> source,
        Func<TSource, Task<Option<T>>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var item in source)
        {
            var option = await selector(item).ConfigureAwait(DefaultConfigureAwait);
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return (values, noneCount);
    }

    /// <summary>
    /// ValueTask variant: Partitions a collection with an async Option-returning predicate.
    /// </summary>
    public static async ValueTask<(IEnumerable<T> values, int noneCount)> PartitionWithAsync<TSource, T>(
        this IEnumerable<TSource> source,
        Func<TSource, ValueTask<Option<T>>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        var values = new List<T>();
        var noneCount = 0;

        foreach (var item in source)
        {
            var option = await selector(item).ConfigureAwait(DefaultConfigureAwait);
            if (option.HasValue)
                values.Add(option.GetValueOrThrow());
            else
                noneCount++;
        }

        return (values, noneCount);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Extracts only the successful values from a collection of Results.
    /// Equivalent to Partition().successes but more efficient.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<T> ChooseSuccesses<T, TE>(this IEnumerable<Result<T, TE>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var result in source)
        {
            if (result.IsSuccess)
                yield return result.Value;
        }
    }

    /// <summary>
    /// Extracts only the failures from a collection of Results.
    /// Equivalent to Partition().failures but more efficient.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<TE> ChooseFailures<T, TE>(this IEnumerable<Result<T, TE>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var result in source)
        {
            if (result.IsFailure)
                yield return result.Error;
        }
    }

    /// <summary>
    /// Extracts only the values from a collection of Options.
    /// Equivalent to Option.Choose() - filters out Nones and unwraps values.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static IEnumerable<T> ChooseValues<T>(this IEnumerable<Option<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        foreach (var option in source)
        {
            if (option.HasValue)
                yield return option.GetValueOrThrow();
        }
    }

    #endregion
}

/// <summary>
/// Result of partitioning a collection of Results.
/// Provides convenient access to successes and failures.
/// </summary>
public sealed class PartitionResult<T, TE>
{
    public IReadOnlyList<T> Successes { get; }
    public IReadOnlyList<TE> Failures { get; }

    public int SuccessCount => Successes.Count;
    public int FailureCount => Failures.Count;
    public int TotalCount => SuccessCount + FailureCount;

    public bool HasSuccesses => SuccessCount > 0;
    public bool HasFailures => FailureCount > 0;
    public bool AllSucceeded => FailureCount == 0 && SuccessCount > 0;
    public bool AllFailed => SuccessCount == 0 && FailureCount > 0;

    internal PartitionResult(List<T> successes, List<TE> failures)
    {
        Successes = successes;
        Failures = failures;
    }

    public void Deconstruct(out IReadOnlyList<T> successes, out IReadOnlyList<TE> failures)
    {
        successes = Successes;
        failures = Failures;
    }
}

/// <summary>
/// Result of partitioning a collection of Options.
/// Provides convenient access to values and none count.
/// </summary>
public sealed class PartitionOption<T>
{
    public IReadOnlyList<T> Values { get; }
    public int NoneCount { get; }

    public int ValueCount => Values.Count;
    public int TotalCount => ValueCount + NoneCount;

    public bool HasValues => ValueCount > 0;
    public bool HasNones => NoneCount > 0;
    public bool AllHaveValues => NoneCount == 0 && ValueCount > 0;
    public bool AllNone => ValueCount == 0 && NoneCount > 0;

    internal PartitionOption(List<T> values, int noneCount)
    {
        Values = values;
        NoneCount = noneCount;
    }

    public void Deconstruct(out IReadOnlyList<T> values, out int noneCount)
    {
        values = Values;
        noneCount = NoneCount;
    }
}

