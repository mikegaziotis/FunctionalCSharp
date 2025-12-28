# Memoization Extensions - Usage Guide

This document demonstrates the usage of memoization extensions added to Roufe.

## Table of Contents
1. [Overview](#overview)
2. [Basic Memoization](#basic-memoization)
3. [Custom Comparers](#custom-comparers)
4. [Time-Based Expiration](#time-based-expiration)
5. [Capacity-Limited Caching](#capacity-limited-caching)
6. [Async Memoization](#async-memoization)
7. [Cache Control](#cache-control)
8. [Real-World Examples](#real-world-examples)
9. [Performance Considerations](#performance-considerations)
10. [Best Practices](#best-practices)

---

## Overview

**Memoization** is an optimization technique that caches the results of expensive function calls and returns the cached result when the same inputs occur again. This is particularly useful for:

- **Pure functions** - Functions with no side effects
- **Expensive computations** - Recursive algorithms, mathematical calculations
- **Database lookups** - Caching frequently accessed data
- **API calls** - Reducing network requests
- **Configuration loading** - Caching settings

### Key Features
- ✅ Thread-safe using `ConcurrentDictionary`
- ✅ Support for 1-4 parameters
- ✅ Custom equality comparers
- ✅ Time-based expiration
- ✅ Capacity-limited LRU-like eviction
- ✅ Async support (Task and ValueTask)
- ✅ Manual cache control
- ✅ Zero external dependencies

---

## Basic Memoization

### Single Parameter

```csharp
// Expensive computation
Func<int, int> fibonacci = null!;
fibonacci = n => n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);

// Memoize it
var memoizedFib = fibonacci.Memoize();

// First call computes
var result1 = memoizedFib(35); // Slow

// Subsequent calls are instant
var result2 = memoizedFib(35); // Instant (cached)
var result3 = memoizedFib(35); // Instant (cached)
```

### Two Parameters

```csharp
Func<int, int, int> add = (a, b) =>
{
    Console.WriteLine("Computing...");
    return a + b;
};

var memoized = add.Memoize();

memoized(2, 3); // Prints "Computing...", returns 5
memoized(2, 3); // Returns 5 instantly (cached)
memoized(5, 7); // Prints "Computing...", returns 12
```

### Three and Four Parameters

```csharp
// Three parameters
Func<int, int, int, int> addThree = (a, b, c) => a + b + c;
var memoizedAddThree = addThree.Memoize();

// Four parameters
Func<int, int, int, int, string> format = (a, b, c, d) => $"{a},{b},{c},{d}";
var memoizedFormat = format.Memoize();
```

---

## Custom Comparers

Use custom equality comparers when default comparison is not suitable.

### Case-Insensitive String Keys

```csharp
Func<string, int> getLength = s =>
{
    Console.WriteLine($"Computing for '{s}'");
    return s.Length;
};

var memoized = getLength.Memoize(StringComparer.OrdinalIgnoreCase);

memoized("Hello"); // Prints "Computing for 'Hello'", returns 5
memoized("hello"); // Returns 5 instantly (same key)
memoized("HELLO"); // Returns 5 instantly (same key)
```

### Custom Object Comparer

```csharp
public class UserIdComparer : IEqualityComparer<User>
{
    public bool Equals(User x, User y) => x?.Id == y?.Id;
    public int GetHashCode(User obj) => obj.Id.GetHashCode();
}

Func<User, Profile> getProfile = user => LoadProfileFromDb(user);
var memoized = getProfile.Memoize(new UserIdComparer());
```

---

## Time-Based Expiration

Cache entries expire after a specified timeout, useful for data that changes over time.

### Basic Timeout

```csharp
Func<string, StockPrice> getStockPrice = symbol =>
{
    Console.WriteLine($"Fetching {symbol}...");
    return FetchFromApi(symbol);
};

// Cache for 5 minutes
var memoized = getStockPrice.MemoizeWithTimeout(TimeSpan.FromMinutes(5));

memoized("AAPL"); // Fetches from API
memoized("AAPL"); // Returns cached value (within 5 minutes)

// After 5 minutes...
memoized("AAPL"); // Fetches from API again
```

### Configuration Caching

```csharp
Func<string, string> getConfig = key =>
{
    return LoadFromConfigServer(key);
};

// Cache for 1 minute
var memoized = getConfig.MemoizeWithTimeout(TimeSpan.FromMinutes(1));

var setting = memoized("MaxConnections"); // Fresh fetch
// Within 1 minute, returns cached value
// After 1 minute, fetches again
```

### Two Parameters with Timeout

```csharp
Func<string, string, WeatherData> getWeather = (city, date) =>
{
    return FetchWeatherData(city, date);
};

var memoized = getWeather.MemoizeWithTimeout(TimeSpan.FromHours(1));

var weather = memoized("London", "2025-12-27"); // Cached for 1 hour
```

---

## Capacity-Limited Caching

Prevents unbounded memory growth by limiting the number of cached entries.

### LRU-Like Eviction

```csharp
Func<int, byte[]> loadImage = id =>
{
    Console.WriteLine($"Loading image {id}");
    return LoadLargeImage(id);
};

// Cache only 100 images
var memoized = loadImage.MemoizeWithCapacity(100);

// As you process images, oldest ones are evicted when capacity is reached
for (int i = 0; i < 200; i++)
{
    var image = memoized(i);
    // First 100 are cached
    // After that, oldest entries are evicted
}
```

### Practical Example - User Sessions

```csharp
Func<string, UserSession> getSession = sessionId =>
{
    return LoadSessionFromStore(sessionId);
};

// Keep only 1000 active sessions in memory
var memoized = getSession.MemoizeWithCapacity(1000);

// Oldest/least-recently-used sessions are evicted automatically
```

---

## Async Memoization

### Task-Based Memoization

```csharp
Func<int, Task<User>> getUserById = async id =>
{
    Console.WriteLine($"Fetching user {id}...");
    await Task.Delay(1000); // Simulate network delay
    return await _dbContext.Users.FindAsync(id);
};

var memoized = getUserById.MemoizeAsync();

var user1 = await memoized(42); // Takes 1 second
var user2 = await memoized(42); // Instant (cached)
var user3 = await memoized(42); // Instant (cached)
```

### Concurrent Calls - Single Execution

Memoization ensures that concurrent calls for the same key only execute once:

```csharp
Func<string, Task<ApiResponse>> callApi = async endpoint =>
{
    Console.WriteLine($"Calling {endpoint}...");
    await Task.Delay(1000);
    return await _httpClient.GetAsync(endpoint);
};

var memoized = callApi.MemoizeAsync();

// Multiple concurrent calls
var tasks = Enumerable.Range(0, 10)
    .Select(_ => memoized("/api/data"))
    .ToArray();

var results = await Task.WhenAll(tasks);
// Only prints "Calling /api/data..." once
// All 10 tasks get the same result
```

### Two Parameters Async

```csharp
Func<string, int, Task<List<Product>>> searchProducts = async (query, page) =>
{
    await Task.Delay(500);
    return await _db.Products
        .Where(p => p.Name.Contains(query))
        .Skip(page * 20)
        .Take(20)
        .ToListAsync();
};

var memoized = searchProducts.MemoizeAsync();

var products = await memoized("laptop", 0); // Cached per query+page
```

### Async with Timeout

```csharp
Func<string, Task<ExchangeRate>> getExchangeRate = async currency =>
{
    return await FetchFromCurrencyApi(currency);
};

// Cache exchange rates for 10 minutes
var memoized = getExchangeRate.MemoizeAsyncWithTimeout(TimeSpan.FromMinutes(10));

var rate = await memoized("USD"); // Cached for 10 minutes
```

### ValueTask Memoization

```csharp
Func<int, ValueTask<int>> compute = async x =>
{
    if (x < 100)
        return x * 2; // Synchronous path

    await Task.Delay(10);
    return x * 2;
};

var memoized = compute.MemoizeAsync();

var result = await memoized(50); // Cached
```

---

## Cache Control

Get explicit control over the cache with `MemoizeWithCache`.

### Basic Cache Control

```csharp
Func<int, int> square = x => x * x;

var (memoized, cache) = square.MemoizeWithCache();

// Use the memoized function
var result1 = memoized(5); // 25

// Inspect cache
Console.WriteLine($"Cache size: {cache.Count}"); // 1
Console.WriteLine($"Contains 5: {cache.Contains(5)}"); // true

// Manually evict an entry
cache.Remove(5);

// Clear entire cache
cache.Clear();
```

### Check Cache Before Computing

```csharp
var (memoized, cache) = expensiveFunc.MemoizeWithCache();

if (cache.TryGetValue(key, out var cachedValue))
{
    Console.WriteLine("Using cached value");
    return cachedValue;
}
else
{
    Console.WriteLine("Computing...");
    return memoized(key);
}
```

### Programmatic Cache Management

```csharp
var (memoized, cache) = getUser.MemoizeWithCache();

// Warm up cache
foreach (var id in frequentUserIds)
{
    memoized(id);
}

// Periodically clear cache
Timer timer = new Timer(_ => cache.Clear(), null, TimeSpan.Zero, TimeSpan.FromHours(1));

// Selective invalidation
void InvalidateUser(int userId)
{
    cache.Remove(userId);
}
```

---

## Real-World Examples

### Example 1: Database Lookup Cache

```csharp
public class UserRepository
{
    private readonly DbContext _db;
    private readonly Func<int, Task<User>> _getUserMemoized;

    public UserRepository(DbContext db)
    {
        _db = db;

        Func<int, Task<User>> getUser = async id =>
            await _db.Users.FindAsync(id);

        // Cache for 5 minutes
        _getUserMemoized = getUser.MemoizeAsyncWithTimeout(TimeSpan.FromMinutes(5));
    }

    public Task<User> GetUserAsync(int id) => _getUserMemoized(id);
}
```

### Example 2: API Rate Limiting

```csharp
public class WeatherService
{
    private readonly Func<string, Task<Weather>> _getWeatherMemoized;

    public WeatherService()
    {
        Func<string, Task<Weather>> getWeather = async city =>
            await _httpClient.GetFromJsonAsync<Weather>($"/weather/{city}");

        // Cache for 30 minutes to avoid hitting rate limits
        _getWeatherMemoized = getWeather.MemoizeAsyncWithTimeout(TimeSpan.FromMinutes(30));
    }

    public Task<Weather> GetWeatherAsync(string city) => _getWeatherMemoized(city);
}
```

### Example 3: Expensive Calculation Cache

```csharp
public class MathService
{
    private static readonly Func<int, BigInteger> _factorialMemoized;

    static MathService()
    {
        Func<int, BigInteger> factorial = null!;
        factorial = n => n <= 1 ? 1 : n * factorial(n - 1);

        _factorialMemoized = factorial.Memoize();
    }

    public BigInteger Factorial(int n) => _factorialMemoized(n);
}
```

### Example 4: Configuration Service

```csharp
public class ConfigurationService
{
    private readonly Func<string, string> _getConfigMemoized;

    public ConfigurationService()
    {
        Func<string, string> getConfig = key =>
            _configProvider.GetValue(key);

        // Cache config for 1 minute, limit to 100 entries
        var memoizedWithTimeout = getConfig.MemoizeWithTimeout(TimeSpan.FromMinutes(1));
        _getConfigMemoized = memoizedWithTimeout;
    }

    public string GetSetting(string key) => _getConfigMemoized(key);
}
```

### Example 5: Image Processing Pipeline

```csharp
public class ImageProcessor
{
    private readonly Func<string, int, int, Task<byte[]>> _resizeMemoized;

    public ImageProcessor()
    {
        Func<string, int, int, Task<byte[]>> resize = async (path, width, height) =>
        {
            var image = await File.ReadAllBytesAsync(path);
            return await ResizeImageAsync(image, width, height);
        };

        // Cache up to 50 resized images
        var memoized = resize.MemoizeAsync();
        // Wrap with capacity limit
        _resizeMemoized = memoized;
    }

    public Task<byte[]> ResizeImageAsync(string path, int width, int height)
        => _resizeMemoized(path, width, height);
}
```

### Example 6: Dependency Injection Pattern

```csharp
public static class MemoizationServiceExtensions
{
    public static IServiceCollection AddMemoizedService<TService, TImpl>(
        this IServiceCollection services,
        TimeSpan cacheTimeout)
        where TService : class
        where TImpl : class, TService
    {
        // Register as singleton with memoization wrapper
        services.AddSingleton<TService>(provider =>
        {
            var impl = ActivatorUtilities.CreateInstance<TImpl>(provider);
            return CreateMemoizedProxy(impl, cacheTimeout);
        });
        return services;
    }
}
```

---

## Performance Considerations

### When to Use Memoization

✅ **Good Use Cases**:
- Pure functions (no side effects)
- Expensive computations (CPU-intensive)
- Slow I/O operations (database, API calls)
- Recursive algorithms
- Frequently called with same inputs

❌ **Bad Use Cases**:
- Functions with side effects
- Operations that should always execute fresh
- Functions that return large objects (memory concern)
- Rarely repeated inputs (cache miss overhead)

### Memory Management

```csharp
// ❌ Bad - Unbounded cache can cause memory issues
var memoized = expensiveFunc.Memoize();

// ✅ Good - Use capacity limit for unbounded input space
var memoized = expensiveFunc.MemoizeWithCapacity(1000);

// ✅ Good - Use timeout for time-sensitive data
var memoized = expensiveFunc.MemoizeWithTimeout(TimeSpan.FromMinutes(5));
```

### Thread Safety

All memoization methods are **thread-safe** using `ConcurrentDictionary`. Multiple threads can safely call memoized functions concurrently.

```csharp
var memoized = expensiveFunc.Memoize();

// Safe from multiple threads
Parallel.For(0, 100, i =>
{
    var result = memoized(i % 10); // Thread-safe
});
```

### Async Memoization Gotchas

```csharp
// ✅ Task memoization caches the Task itself
var taskMemoized = asyncFunc.MemoizeAsync();
// Concurrent calls share the same Task

// ✅ ValueTask memoization caches the result value
var valueTaskMemoized = asyncFunc.MemoizeAsync();
// ValueTask is not cached directly (by design)
```

---

## Best Practices

### 1. Choose Appropriate Cache Strategy

```csharp
// Short-lived data - use timeout
var stockPrices = getStockPrice.MemoizeWithTimeout(TimeSpan.FromMinutes(5));

// Stable data - use basic memoization
var fibonacci = fib.Memoize();

// Large input space - use capacity limit
var imageCache = processImage.MemoizeWithCapacity(100);
```

### 2. Combine with Cache Control

```csharp
var (memoized, cache) = getUser.MemoizeWithCache();

// Invalidate on update
public void UpdateUser(User user)
{
    _db.Update(user);
    cache.Remove(user.Id); // Invalidate cache entry
}
```

### 3. Monitor Cache Performance

```csharp
var (memoized, cache) = expensiveFunc.MemoizeWithCache();

// Periodically log cache stats
Timer timer = new Timer(_ =>
{
    _logger.LogInformation($"Cache size: {cache.Count}");
}, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
```

### 4. Use Type Constraints

```csharp
// ✅ Keys must be non-null
Func<string, int> func = s => s.Length;
var memoized = func.Memoize(); // string is notnull

// ⚠️ Won't compile - nullable types not supported
// Func<string?, int> func2 = s => s?.Length ?? 0;
// var memoized2 = func2.Memoize(); // Compiler error
```

### 5. Separate Concerns

```csharp
// ✅ Good - Memoization as cross-cutting concern
public class UserService
{
    private readonly Func<int, Task<User>> _getUser;

    public UserService(DbContext db)
    {
        // Core logic
        Func<int, Task<User>> getUser = id => db.Users.FindAsync(id);

        // Memoization layer
        _getUser = getUser.MemoizeAsyncWithTimeout(TimeSpan.FromMinutes(5));
    }
}
```

### 6. Document Cache Behavior

```csharp
/// <summary>
/// Gets a user by ID. Results are cached for 5 minutes.
/// </summary>
/// <remarks>
/// Cache is thread-safe and uses ConcurrentDictionary internally.
/// Use InvalidateUser() to manually clear cache entries.
/// </remarks>
public Task<User> GetUserAsync(int id) => _getUserMemoized(id);
```

---

## Comparison with Other Caching Solutions

### vs. In-Memory Cache (IMemoryCache)

| Feature | Memoization | IMemoryCache |
|---------|-------------|--------------|
| Setup | Zero config | DI registration |
| Scope | Function-level | Application-level |
| Expiration | Built-in options | Manual policies |
| Type Safety | Strong | Object-based |
| Overhead | Minimal | Moderate |
| Use Case | Pure functions | General caching |

### vs. Distributed Cache (Redis)

| Feature | Memoization | Redis |
|---------|-------------|-------|
| Storage | In-memory | External |
| Persistence | No | Yes |
| Shared | Single app | Multi-app |
| Latency | Nanoseconds | Milliseconds |
| Serialization | None | Required |
| Use Case | Hot path | Shared data |

---

## Troubleshooting

### Problem: Cache Not Working

```csharp
// ❌ Different object instances are different keys
var memoized = func.Memoize();
memoized(new User { Id = 1 }); // Computed
memoized(new User { Id = 1 }); // Computed again! (different instance)

// ✅ Use value types or implement proper equality
record User(int Id); // Records have value equality
var memoized = func.Memoize();
memoized(new User(1)); // Computed
memoized(new User(1)); // Cached!
```

### Problem: Memory Leak

```csharp
// ❌ Unbounded cache with infinite input space
var memoized = hash.Memoize();
foreach (var largeFile in Directory.GetFiles("/"))
{
    memoized(largeFile); // Caches everything forever
}

// ✅ Use capacity limit
var memoized = hash.MemoizeWithCapacity(1000);
```

### Problem: Stale Data

```csharp
// ❌ Cache doesn't know data changed
var memoized = getUser.Memoize();
var user = memoized(1); // Returns old data after update

// ✅ Use timeout
var memoized = getUser.MemoizeWithTimeout(TimeSpan.FromMinutes(5));

// ✅ Or use cache control
var (memoized, cache) = getUser.MemoizeWithCache();
UpdateUser(user);
cache.Remove(user.Id); // Manual invalidation
```

---

## Summary

Memoization is a powerful optimization technique for caching pure function results. The Roufe implementation provides:

- **Thread-safe** caching with ConcurrentDictionary
- **Flexible strategies** (basic, timeout, capacity)
- **Async support** for Task and ValueTask
- **Manual control** via cache interface
- **Zero dependencies** - pure .NET

Use memoization for expensive, pure functions called repeatedly with the same inputs to dramatically improve performance.

