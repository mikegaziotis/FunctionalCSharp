# Memoization Extensions - Quick Reference

## Overview
Cache function results to avoid recomputation. Thread-safe using ConcurrentDictionary.

## Basic Memoization

| Signature | Description |
|-----------|-------------|
| `Func<T, TResult> Memoize()` | Cache 1-param function results indefinitely |
| `Func<T1, T2, TResult> Memoize()` | Cache 2-param function results indefinitely |
| `Func<T1, T2, T3, TResult> Memoize()` | Cache 3-param function results indefinitely |
| `Func<T1, T2, T3, T4, TResult> Memoize()` | Cache 4-param function results indefinitely |

## Custom Comparer

| Signature | Description |
|-----------|-------------|
| `Func<T, TResult> Memoize(IEqualityComparer<T>)` | Cache with custom key comparison |

## Time-Based Expiration

| Signature | Description |
|-----------|-------------|
| `Func<T, TResult> MemoizeWithTimeout(TimeSpan)` | Cache expires after timeout |
| `Func<T1, T2, TResult> MemoizeWithTimeout(TimeSpan)` | Cache 2-param with timeout |

## Capacity-Limited

| Signature | Description |
|-----------|-------------|
| `Func<T, TResult> MemoizeWithCapacity(int)` | LRU-like eviction at max capacity |

## Async Memoization

| Signature | Description |
|-----------|-------------|
| `Func<T, Task<TResult>> MemoizeAsync()` | Cache async Task results |
| `Func<T1, T2, Task<TResult>> MemoizeAsync()` | Cache 2-param async results |
| `Func<T, Task<TResult>> MemoizeAsyncWithTimeout(TimeSpan)` | Async cache with expiration |
| `Func<T, ValueTask<TResult>> MemoizeAsync()` | Cache async ValueTask results |
| `Func<T1, T2, ValueTask<TResult>> MemoizeAsync()` | Cache 2-param ValueTask results |

## Cache Control

| Signature | Description |
|-----------|-------------|
| `(Func<T, TResult>, IMemoizationCache<T, TResult>) MemoizeWithCache()` | Returns memoized func + cache control |

### IMemoizationCache Interface

| Method | Description |
|--------|-------------|
| `int Count` | Number of cached entries |
| `void Clear()` | Remove all entries |
| `bool Remove(TKey key)` | Remove specific entry |
| `bool Contains(TKey key)` | Check if key exists |
| `bool TryGetValue(TKey key, out TValue value)` | Get cached value |
| `TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)` | Get or compute value |

## Quick Examples

### Basic
```csharp
var memoized = expensiveFunc.Memoize();
```

### With Timeout
```csharp
var memoized = getConfig.MemoizeWithTimeout(TimeSpan.FromMinutes(5));
```

### With Capacity
```csharp
var memoized = processImage.MemoizeWithCapacity(100);
```

### Async
```csharp
var memoized = getUser.MemoizeAsync();
var user = await memoized(42);
```

### Cache Control
```csharp
var (memoized, cache) = func.MemoizeWithCache();
cache.Clear(); // Manual clear
cache.Remove(key); // Invalidate entry
```

### Custom Comparer
```csharp
var memoized = getLength.Memoize(StringComparer.OrdinalIgnoreCase);
```

## When to Use

✅ **Good**:
- Pure functions (no side effects)
- Expensive computations
- Database lookups
- API calls
- Recursive algorithms
- Frequently called with same inputs

❌ **Avoid**:
- Functions with side effects
- Large return values (memory)
- Rarely repeated inputs
- Real-time data requirements

## Performance Notes

- **Thread-safe**: Uses ConcurrentDictionary
- **First call**: Original time + dictionary insert
- **Cache hit**: ~O(1) dictionary lookup
- **Async**: Concurrent calls share same Task
- **Memory**: Consider using capacity or timeout

## Common Patterns

### Fibonacci
```csharp
Func<int, int> fib = null!;
fib = n => n <= 1 ? n : fib(n - 1) + fib(n - 2);
var memoized = fib.Memoize();
```

### Database Cache
```csharp
Func<int, Task<User>> getUser = id => _db.Users.FindAsync(id);
var cached = getUser.MemoizeAsyncWithTimeout(TimeSpan.FromMinutes(5));
```

### API Rate Limiting
```csharp
Func<string, ApiResponse> callApi = endpoint => FetchFromApi(endpoint);
var cached = callApi.MemoizeWithCapacity(100);
```

### Configuration
```csharp
var (memoized, cache) = getConfig.MemoizeWithCache();
// Invalidate on update
void OnConfigChanged() => cache.Clear();
```

## Troubleshooting

### Problem: Cache not working
**Cause**: Different object instances
**Solution**: Use value types or records with value equality

### Problem: Memory leak
**Cause**: Unbounded cache
**Solution**: Use `MemoizeWithCapacity(n)` or `MemoizeWithTimeout(ts)`

### Problem: Stale data
**Cause**: Cache doesn't know data changed
**Solution**: Use timeout or manual cache invalidation

## Type Constraints

All keys must be non-null (`where T : notnull`):
```csharp
✅ Func<string, int> - OK
✅ Func<int, int> - OK
❌ Func<string?, int> - Won't compile
```

## Thread Safety

All methods are thread-safe:
```csharp
var memoized = func.Memoize();

Parallel.For(0, 100, i =>
{
    memoized(i % 10); // Safe!
});
```

## Integration

Works with other Roufe features:
```csharp
// With Result
Func<int, Result<User, string>> getUser = ...;
var memoized = getUser.Memoize();

// With function composition
var pipeline = parse
    .AndThen(validate.Memoize())
    .AndThen(transform);
```

