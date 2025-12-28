using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Roufe;

/// <summary>
/// Provides memoization extensions for caching function results.
/// Memoization caches the results of expensive function calls and returns the cached result when the same inputs occur again.
/// </summary>
public static class MemoizationExtensions
{
    #region Synchronous Memoization

    /// <summary>
    /// Memoizes a function with one parameter. Results are cached indefinitely.
    /// Thread-safe using ConcurrentDictionary.
    /// </summary>
    /// <example>
    /// Func&lt;int, int&gt; fibonacci = n => n &lt;= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);
    /// var memoizedFib = fibonacci.Memoize();
    /// memoizedFib(40); // Fast after first call
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<T, TResult>();
        return arg => cache.GetOrAdd(arg, func);
    }

    /// <summary>
    /// Memoizes a function with two parameters. Results are cached indefinitely.
    /// Thread-safe using ConcurrentDictionary.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        where T1 : notnull
        where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<(T1, T2), TResult>();
        return (arg1, arg2) => cache.GetOrAdd((arg1, arg2), key => func(key.Item1, key.Item2));
    }

    /// <summary>
    /// Memoizes a function with three parameters. Results are cached indefinitely.
    /// Thread-safe using ConcurrentDictionary.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<(T1, T2, T3), TResult>();
        return (arg1, arg2, arg3) => cache.GetOrAdd((arg1, arg2, arg3), key => func(key.Item1, key.Item2, key.Item3));
    }

    /// <summary>
    /// Memoizes a function with four parameters. Results are cached indefinitely.
    /// Thread-safe using ConcurrentDictionary.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(
        this Func<T1, T2, T3, T4, TResult> func)
        where T1 : notnull
        where T2 : notnull
        where T3 : notnull
        where T4 : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<(T1, T2, T3, T4), TResult>();
        return (arg1, arg2, arg3, arg4) => cache.GetOrAdd(
            (arg1, arg2, arg3, arg4),
            key => func(key.Item1, key.Item2, key.Item3, key.Item4));
    }

    #endregion

    #region Memoization with Custom Comparer

    /// <summary>
    /// Memoizes a function with one parameter using a custom equality comparer.
    /// Useful when default equality comparison is not suitable.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T, TResult> Memoize<T, TResult>(
        this Func<T, TResult> func,
        IEqualityComparer<T> comparer) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        ArgumentNullException.ThrowIfNull(comparer);
        var cache = new ConcurrentDictionary<T, TResult>(comparer);
        return arg => cache.GetOrAdd(arg, func);
    }

    #endregion

    #region Memoization with Expiration

    /// <summary>
    /// Memoizes a function with time-based cache expiration.
    /// Cached values expire after the specified timeout and will be recomputed on next access.
    /// </summary>
    /// <example>
    /// var getStockPrice = ((string symbol) => FetchStockPrice(symbol))
    ///     .MemoizeWithTimeout(TimeSpan.FromMinutes(5));
    /// </example>
    public static Func<T, TResult> MemoizeWithTimeout<T, TResult>(
        this Func<T, TResult> func,
        TimeSpan timeout) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentException("Timeout must be positive", nameof(timeout));

        var cache = new ConcurrentDictionary<T, (TResult result, DateTime expiry)>();

        return arg =>
        {
            var now = DateTime.UtcNow;

            if (cache.TryGetValue(arg, out var cached))
            {
                if (cached.expiry > now)
                {
                    return cached.result;
                }
                // Expired, remove it
                cache.TryRemove(arg, out _);
            }

            var result = func(arg);
            var expiry = now.Add(timeout);
            cache[arg] = (result, expiry);
            return result;
        };
    }

    /// <summary>
    /// Memoizes a function with time-based cache expiration for two parameters.
    /// </summary>
    public static Func<T1, T2, TResult> MemoizeWithTimeout<T1, T2, TResult>(
        this Func<T1, T2, TResult> func,
        TimeSpan timeout)
        where T1 : notnull
        where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentException("Timeout must be positive", nameof(timeout));

        var cache = new ConcurrentDictionary<(T1, T2), (TResult result, DateTime expiry)>();

        return (arg1, arg2) =>
        {
            var key = (arg1, arg2);
            var now = DateTime.UtcNow;

            if (cache.TryGetValue(key, out var cached))
            {
                if (cached.expiry > now)
                {
                    return cached.result;
                }
                cache.TryRemove(key, out _);
            }

            var result = func(arg1, arg2);
            var expiry = now.Add(timeout);
            cache[key] = (result, expiry);
            return result;
        };
    }

    #endregion

    #region Memoization with Max Capacity (LRU-like)

    /// <summary>
    /// Memoizes a function with a maximum cache size.
    /// When capacity is reached, oldest entries are removed (approximate LRU behavior).
    /// Useful to prevent unbounded memory growth for functions with many different inputs.
    /// </summary>
    public static Func<T, TResult> MemoizeWithCapacity<T, TResult>(
        this Func<T, TResult> func,
        int maxCapacity) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        if (maxCapacity <= 0)
            throw new ArgumentException("Capacity must be positive", nameof(maxCapacity));

        var cache = new ConcurrentDictionary<T, (TResult result, long accessCount)>();
        long globalAccessCounter = 0;

        return arg =>
        {
            // Try to get existing value
            if (cache.TryGetValue(arg, out var cached))
            {
                var newCount = Interlocked.Increment(ref globalAccessCounter);
                cache[arg] = (cached.result, newCount);
                return cached.result;
            }

            // Compute new value
            var result = func(arg);
            var accessCount = Interlocked.Increment(ref globalAccessCounter);

            // Check capacity and evict if necessary
            if (cache.Count >= maxCapacity)
            {
                // Find and remove the least recently accessed item
                var oldestKey = default(T);
                var oldestAccess = long.MaxValue;

                foreach (var kvp in cache)
                {
                    if (kvp.Value.accessCount < oldestAccess)
                    {
                        oldestAccess = kvp.Value.accessCount;
                        oldestKey = kvp.Key;
                    }
                }

                if (oldestKey != null)
                {
                    cache.TryRemove(oldestKey, out _);
                }
            }

            cache[arg] = (result, accessCount);
            return result;
        };
    }

    #endregion

    #region Async Memoization

    /// <summary>
    /// Memoizes an async function. Results are cached indefinitely.
    /// Thread-safe - only one task per key will execute at a time.
    /// </summary>
    /// <example>
    /// Func&lt;int, Task&lt;User&gt;&gt; getUser = id => _db.Users.FindAsync(id);
    /// var memoizedGetUser = getUser.MemoizeAsync();
    /// await memoizedGetUser(42); // Fast after first call
    /// </example>
    public static Func<T, Task<TResult>> MemoizeAsync<T, TResult>(this Func<T, Task<TResult>> func)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<T, Task<TResult>>();

        return arg => cache.GetOrAdd(arg, func);
    }

    /// <summary>
    /// Memoizes an async function with two parameters.
    /// </summary>
    public static Func<T1, T2, Task<TResult>> MemoizeAsync<T1, T2, TResult>(
        this Func<T1, T2, Task<TResult>> func)
        where T1 : notnull
        where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<(T1, T2), Task<TResult>>();

        return (arg1, arg2) => cache.GetOrAdd((arg1, arg2), key => func(key.Item1, key.Item2));
    }

    /// <summary>
    /// Memoizes an async function with time-based cache expiration.
    /// </summary>
    public static Func<T, Task<TResult>> MemoizeAsyncWithTimeout<T, TResult>(
        this Func<T, Task<TResult>> func,
        TimeSpan timeout) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        if (timeout <= TimeSpan.Zero)
            throw new ArgumentException("Timeout must be positive", nameof(timeout));

        var cache = new ConcurrentDictionary<T, (Task<TResult> task, DateTime expiry)>();

        return async arg =>
        {
            var now = DateTime.UtcNow;

            if (cache.TryGetValue(arg, out var cached))
            {
                if (cached.expiry > now)
                {
                    return await cached.task.ConfigureAwait(DefaultConfigureAwait);
                }
                cache.TryRemove(arg, out _);
            }

            var task = func(arg);
            var expiry = now.Add(timeout);
            cache[arg] = (task, expiry);
            return await task.ConfigureAwait(DefaultConfigureAwait);
        };
    }

    #endregion

    #region ValueTask Memoization

    /// <summary>
    /// Memoizes a ValueTask-returning function. Results are cached indefinitely.
    /// Note: ValueTask results are converted to cached values, not cached as ValueTasks.
    /// </summary>
    public static Func<T, ValueTask<TResult>> MemoizeAsync<T, TResult>(this Func<T, ValueTask<TResult>> func)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<T, TResult>();

        return async arg =>
        {
            if (cache.TryGetValue(arg, out var cached))
            {
                return cached;
            }

            var result = await func(arg).ConfigureAwait(DefaultConfigureAwait);
            cache[arg] = result;
            return result;
        };
    }

    /// <summary>
    /// Memoizes a ValueTask-returning function with two parameters.
    /// </summary>
    public static Func<T1, T2, ValueTask<TResult>> MemoizeAsync<T1, T2, TResult>(
        this Func<T1, T2, ValueTask<TResult>> func)
        where T1 : notnull
        where T2 : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new ConcurrentDictionary<(T1, T2), TResult>();

        return async (arg1, arg2) =>
        {
            var key = (arg1, arg2);
            if (cache.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var result = await func(arg1, arg2).ConfigureAwait(DefaultConfigureAwait);
            cache[key] = result;
            return result;
        };
    }

    #endregion

    #region Cache Control

    /// <summary>
    /// Creates a memoized function that also returns a cache control object
    /// allowing manual cache inspection and clearing.
    /// </summary>
    public static (Func<T, TResult> memoized, IMemoizationCache<T, TResult> cache) MemoizeWithCache<T, TResult>(
        this Func<T, TResult> func) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(func);
        var cache = new MemoizationCache<T, TResult>();
        var memoized = new Func<T, TResult>(arg => cache.GetOrAdd(arg, func));
        return (memoized, cache);
    }

    #endregion
}

/// <summary>
/// Interface for controlling memoization cache.
/// </summary>
public interface IMemoizationCache<TKey, TValue> where TKey : notnull
{
    /// <summary>
    /// Gets the number of cached entries.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Clears all cached entries.
    /// </summary>
    void Clear();

    /// <summary>
    /// Removes a specific entry from the cache.
    /// </summary>
    bool Remove(TKey key);

    /// <summary>
    /// Checks if a key exists in the cache.
    /// </summary>
    bool Contains(TKey key);

    /// <summary>
    /// Tries to get a cached value without computing it.
    /// </summary>
    bool TryGetValue(TKey key, out TValue value);

    /// <summary>
    /// Gets or adds a value to the cache.
    /// </summary>
    TValue GetOrAdd(TKey key, Func<TKey, TValue> factory);
}

/// <summary>
/// Implementation of memoization cache with control methods.
/// </summary>
internal sealed class MemoizationCache<TKey, TValue> : IMemoizationCache<TKey, TValue> where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _cache = new();

    public int Count => _cache.Count;

    public void Clear() => _cache.Clear();

    public bool Remove(TKey key) => _cache.TryRemove(key, out _);

    public bool Contains(TKey key) => _cache.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => _cache.TryGetValue(key, out value!);

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory) => _cache.GetOrAdd(key, factory);
}

