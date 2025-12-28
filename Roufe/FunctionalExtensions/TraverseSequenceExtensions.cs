using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Roufe;

/// <summary>
/// Provides Traverse and Sequence operations for transforming collections through monadic contexts.
/// These operations "flip" the structure from IEnumerable&lt;Monad&lt;T&gt;&gt; to Monad&lt;IEnumerable&lt;T&gt;&gt;.
/// </summary>
public static class TraverseSequenceExtensions
{
    #region Result Sequence

    /// <summary>
    /// Sequences a collection of Results into a Result of collection.
    /// Returns Success with all values if all Results are successful.
    /// Returns Failure with the first error encountered if any Result fails.
    /// </summary>
    /// <example>
    /// var results = new[] { Result.Success(1), Result.Success(2), Result.Success(3) };
    /// Result&lt;IEnumerable&lt;int&gt;, string&gt; sequenced = results.Sequence();
    /// // sequenced.IsSuccess = true, Value = [1, 2, 3]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Result<IEnumerable<T>, TE> Sequence<T, TE>(this IEnumerable<Result<T, TE>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var results = new List<T>();
        foreach (var result in source)
        {
            if (result.IsFailure)
                return Result.Failure<IEnumerable<T>, TE>(result.Error);

            results.Add(result.Value);
        }

        return Result.Success<IEnumerable<T>, TE>(results);
    }

    /// <summary>
    /// Async variant: Sequences a collection of Task Results into a Task Result of collection.
    /// </summary>
    public static async Task<Result<IEnumerable<T>, TE>> Sequence<T, TE>(
        this IEnumerable<Task<Result<T, TE>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var results = new List<T>();
        foreach (var taskResult in source)
        {
            var result = await taskResult.ConfigureAwait(DefaultConfigureAwait);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<T>, TE>(result.Error);

            results.Add(result.Value);
        }

        return Result.Success<IEnumerable<T>, TE>(results);
    }

    /// <summary>
    /// Async variant: Sequences a Task of collection of Results into a Task Result of collection.
    /// </summary>
    public static async Task<Result<IEnumerable<T>, TE>> Sequence<T, TE>(
        this Task<IEnumerable<Result<T, TE>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var results = await source.ConfigureAwait(DefaultConfigureAwait);
        return results.Sequence();
    }

    /// <summary>
    /// ValueTask variant: Sequences a collection of ValueTask Results.
    /// </summary>
    public static async ValueTask<Result<IEnumerable<T>, TE>> Sequence<T, TE>(
        this IEnumerable<ValueTask<Result<T, TE>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var results = new List<T>();
        foreach (var taskResult in source)
        {
            var result = await taskResult.ConfigureAwait(DefaultConfigureAwait);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<T>, TE>(result.Error);

            results.Add(result.Value);
        }

        return Result.Success<IEnumerable<T>, TE>(results);
    }

    #endregion

    #region Result Traverse

    /// <summary>
    /// Traverses a collection by applying a function that returns a Result to each element.
    /// Returns Success with all transformed values if all operations succeed.
    /// Returns Failure with the first error encountered if any operation fails.
    /// This is equivalent to map followed by sequence.
    /// </summary>
    /// <example>
    /// var numbers = new[] { "1", "2", "3" };
    /// Result&lt;IEnumerable&lt;int&gt;, string&gt; parsed = numbers.Traverse(s =>
    ///     int.TryParse(s, out var n)
    ///         ? Result.Success&lt;int, string&gt;(n)
    ///         : Result.Failure&lt;int, string&gt;("Invalid"));
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Result<IEnumerable<TR>, TE> Traverse<T, TR, TE>(
        this IEnumerable<T> source,
        Func<T, Result<TR, TE>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TR>();
        foreach (var item in source)
        {
            var result = func(item);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TR>, TE>(result.Error);

            results.Add(result.Value);
        }

        return Result.Success<IEnumerable<TR>, TE>(results);
    }

    /// <summary>
    /// Async variant: Traverses a collection with an async function returning Task Result.
    /// </summary>
    public static async Task<Result<IEnumerable<TR>, TE>> TraverseAsync<T, TR, TE>(
        this IEnumerable<T> source,
        Func<T, Task<Result<TR, TE>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TR>();
        foreach (var item in source)
        {
            var result = await func(item).ConfigureAwait(DefaultConfigureAwait);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TR>, TE>(result.Error);

            results.Add(result.Value);
        }

        return Result.Success<IEnumerable<TR>, TE>(results);
    }

    /// <summary>
    /// Async variant: Traverses a Task collection with a synchronous function.
    /// </summary>
    public static async Task<Result<IEnumerable<TR>, TE>> Traverse<T, TR, TE>(
        this Task<IEnumerable<T>> source,
        Func<T, Result<TR, TE>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = await source.ConfigureAwait(DefaultConfigureAwait);
        return items.Traverse(func);
    }

    /// <summary>
    /// Async variant: Traverses a Task collection with an async function.
    /// </summary>
    public static async Task<Result<IEnumerable<TR>, TE>> TraverseAsync<T, TR, TE>(
        this Task<IEnumerable<T>> source,
        Func<T, Task<Result<TR, TE>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = await source.ConfigureAwait(DefaultConfigureAwait);
        return await items.TraverseAsync(func).ConfigureAwait(DefaultConfigureAwait);
    }

    /// <summary>
    /// ValueTask variant: Traverses a collection with an async function returning ValueTask Result.
    /// </summary>
    public static async ValueTask<Result<IEnumerable<TR>, TE>> TraverseAsync<T, TR, TE>(
        this IEnumerable<T> source,
        Func<T, ValueTask<Result<TR, TE>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TR>();
        foreach (var item in source)
        {
            var result = await func(item).ConfigureAwait(DefaultConfigureAwait);
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TR>, TE>(result.Error);

            results.Add(result.Value);
        }

        return Result.Success<IEnumerable<TR>, TE>(results);
    }

    #endregion

    #region Option Sequence

    /// <summary>
    /// Sequences a collection of Options into an Option of collection.
    /// Returns Some with all values if all Options have values.
    /// Returns None if any Option is None.
    /// </summary>
    /// <example>
    /// var options = new[] { Option.From(1), Option.From(2), Option.From(3) };
    /// Option&lt;IEnumerable&lt;int&gt;&gt; sequenced = options.Sequence();
    /// // sequenced.HasValue = true, Value = [1, 2, 3]
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Option<IEnumerable<T>> Sequence<T>(this IEnumerable<Option<T>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var results = new List<T>();
        foreach (var option in source)
        {
            if (option.HasNoValue)
                return Option<IEnumerable<T>>.None;

            results.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<T>>.From(results);
    }

    /// <summary>
    /// Async variant: Sequences a collection of Task Options into a Task Option of collection.
    /// </summary>
    public static async Task<Option<IEnumerable<T>>> Sequence<T>(
        this IEnumerable<Task<Option<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var results = new List<T>();
        foreach (var taskOption in source)
        {
            var option = await taskOption.ConfigureAwait(DefaultConfigureAwait);
            if (option.HasNoValue)
                return Option<IEnumerable<T>>.None;

            results.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<T>>.From(results);
    }

    /// <summary>
    /// Async variant: Sequences a Task of collection of Options.
    /// </summary>
    public static async Task<Option<IEnumerable<T>>> Sequence<T>(
        this Task<IEnumerable<Option<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        var options = await source.ConfigureAwait(DefaultConfigureAwait);
        return options.Sequence();
    }

    /// <summary>
    /// ValueTask variant: Sequences a collection of ValueTask Options.
    /// </summary>
    public static async ValueTask<Option<IEnumerable<T>>> Sequence<T>(
        this IEnumerable<ValueTask<Option<T>>> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var results = new List<T>();
        foreach (var taskOption in source)
        {
            var option = await taskOption.ConfigureAwait(DefaultConfigureAwait);
            if (option.HasNoValue)
                return Option<IEnumerable<T>>.None;

            results.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<T>>.From(results);
    }

    #endregion

    #region Option Traverse

    /// <summary>
    /// Traverses a collection by applying a function that returns an Option to each element.
    /// Returns Some with all transformed values if all operations return Some.
    /// Returns None if any operation returns None.
    /// This is equivalent to map followed by sequence.
    /// </summary>
    /// <example>
    /// var numbers = new[] { "1", "2", "3" };
    /// Option&lt;IEnumerable&lt;int&gt;&gt; parsed = numbers.Traverse(s =>
    ///     int.TryParse(s, out var n) ? Option.From(n) : Option&lt;int&gt;.None);
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Option<IEnumerable<TR>> Traverse<T, TR>(
        this IEnumerable<T> source,
        Func<T, Option<TR>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TR>();
        foreach (var item in source)
        {
            var option = func(item);
            if (option.HasNoValue)
                return Option<IEnumerable<TR>>.None;

            results.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<TR>>.From(results);
    }

    /// <summary>
    /// Async variant: Traverses a collection with an async function returning Task Option.
    /// </summary>
    public static async Task<Option<IEnumerable<TR>>> TraverseAsync<T, TR>(
        this IEnumerable<T> source,
        Func<T, Task<Option<TR>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TR>();
        foreach (var item in source)
        {
            var option = await func(item).ConfigureAwait(DefaultConfigureAwait);
            if (option.HasNoValue)
                return Option<IEnumerable<TR>>.None;

            results.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<TR>>.From(results);
    }

    /// <summary>
    /// Async variant: Traverses a Task collection with a synchronous function.
    /// </summary>
    public static async Task<Option<IEnumerable<TR>>> Traverse<T, TR>(
        this Task<IEnumerable<T>> source,
        Func<T, Option<TR>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = await source.ConfigureAwait(DefaultConfigureAwait);
        return items.Traverse(func);
    }

    /// <summary>
    /// Async variant: Traverses a Task collection with an async function.
    /// </summary>
    public static async Task<Option<IEnumerable<TR>>> TraverseAsync<T, TR>(
        this Task<IEnumerable<T>> source,
        Func<T, Task<Option<TR>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = await source.ConfigureAwait(DefaultConfigureAwait);
        return await items.TraverseAsync(func).ConfigureAwait(DefaultConfigureAwait);
    }

    /// <summary>
    /// ValueTask variant: Traverses a collection with an async function returning ValueTask Option.
    /// </summary>
    public static async ValueTask<Option<IEnumerable<TR>>> TraverseAsync<T, TR>(
        this IEnumerable<T> source,
        Func<T, ValueTask<Option<TR>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var results = new List<TR>();
        foreach (var item in source)
        {
            var option = await func(item).ConfigureAwait(DefaultConfigureAwait);
            if (option.HasNoValue)
                return Option<IEnumerable<TR>>.None;

            results.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<TR>>.From(results);
    }

    #endregion

    #region Parallel Traverse (Task only)

    /// <summary>
    /// Traverses a collection in parallel by applying an async function to each element.
    /// Returns Success with all transformed values if all operations succeed.
    /// Returns Failure with the first error encountered if any operation fails.
    /// Note: Operations execute concurrently, so order is preserved but execution is parallel.
    /// </summary>
    public static async Task<Result<IEnumerable<TR>, TE>> TraverseParallel<T, TR, TE>(
        this IEnumerable<T> source,
        Func<T, Task<Result<TR, TE>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = source.ToArray();
        var tasks = items.Select(func).ToArray();
        var results = await Task.WhenAll(tasks).ConfigureAwait(DefaultConfigureAwait);

        var output = new List<TR>(results.Length);
        foreach (var result in results)
        {
            if (result.IsFailure)
                return Result.Failure<IEnumerable<TR>, TE>(result.Error);

            output.Add(result.Value);
        }

        return Result.Success<IEnumerable<TR>, TE>(output);
    }

    /// <summary>
    /// Traverses a collection in parallel with Options.
    /// Returns Some with all transformed values if all operations return Some.
    /// Returns None if any operation returns None.
    /// </summary>
    public static async Task<Option<IEnumerable<TR>>> TraverseParallel<T, TR>(
        this IEnumerable<T> source,
        Func<T, Task<Option<TR>>> func)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(func);

        var items = source.ToArray();
        var tasks = items.Select(func).ToArray();
        var options = await Task.WhenAll(tasks).ConfigureAwait(DefaultConfigureAwait);

        var output = new List<TR>(options.Length);
        foreach (var option in options)
        {
            if (option.HasNoValue)
                return Option<IEnumerable<TR>>.None;

            output.Add(option.GetValueOrThrow());
        }

        return Option<IEnumerable<TR>>.From(output);
    }

    #endregion
}

