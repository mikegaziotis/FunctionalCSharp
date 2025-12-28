using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class MemoizationExtensionsTests
{
    #region Basic Memoization Tests

    [Fact]
    public void Memoize_SingleParameter_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> expensiveFunc = x =>
        {
            callCount++;
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.Memoize();
        var result1 = memoized(5);
        var result2 = memoized(5);
        var result3 = memoized(10);
        var result4 = memoized(5);

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(10, result2);
        Assert.Equal(20, result3);
        Assert.Equal(10, result4);
        Assert.Equal(2, callCount); // Called only twice (for 5 and 10)
    }

    [Fact]
    public void Memoize_TwoParameters_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, int, int> add = (a, b) =>
        {
            callCount++;
            return a + b;
        };

        // Act
        var memoized = add.Memoize();
        var result1 = memoized(2, 3);
        var result2 = memoized(2, 3);
        var result3 = memoized(5, 7);

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
        Assert.Equal(12, result3);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void Memoize_ThreeParameters_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, int, int, int> addThree = (a, b, c) =>
        {
            callCount++;
            return a + b + c;
        };

        // Act
        var memoized = addThree.Memoize();
        var result1 = memoized(1, 2, 3);
        var result2 = memoized(1, 2, 3);
        var result3 = memoized(4, 5, 6);

        // Assert
        Assert.Equal(6, result1);
        Assert.Equal(6, result2);
        Assert.Equal(15, result3);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public void Memoize_FourParameters_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, int, int, int, string> format = (a, b, c, d) =>
        {
            callCount++;
            return $"{a},{b},{c},{d}";
        };

        // Act
        var memoized = format.Memoize();
        var result1 = memoized(1, 2, 3, 4);
        var result2 = memoized(1, 2, 3, 4);

        // Assert
        Assert.Equal("1,2,3,4", result1);
        Assert.Equal("1,2,3,4", result2);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void Memoize_DifferentInputs_ComputesSeparately()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> square = x =>
        {
            callCount++;
            return x * x;
        };

        // Act
        var memoized = square.Memoize();
        var result1 = memoized(2);
        var result2 = memoized(3);
        var result3 = memoized(2);
        var result4 = memoized(3);

        // Assert
        Assert.Equal(4, result1);
        Assert.Equal(9, result2);
        Assert.Equal(4, result3);
        Assert.Equal(9, result4);
        Assert.Equal(2, callCount); // Once for 2, once for 3
    }

    #endregion

    #region Custom Comparer Tests

    [Fact]
    public void Memoize_WithCustomComparer_UsesCaseInsensitiveComparison()
    {
        // Arrange
        var callCount = 0;
        Func<string, int> getLength = s =>
        {
            callCount++;
            return s.Length;
        };

        // Act
        var memoized = getLength.Memoize(StringComparer.OrdinalIgnoreCase);
        var result1 = memoized("Hello");
        var result2 = memoized("hello");
        var result3 = memoized("HELLO");

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
        Assert.Equal(5, result3);
        Assert.Equal(1, callCount); // All considered the same key
    }

    #endregion

    #region Timeout Tests

    [Fact]
    public async Task MemoizeWithTimeout_ExpiredEntry_Recomputes()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> expensiveFunc = x =>
        {
            callCount++;
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.MemoizeWithTimeout(TimeSpan.FromMilliseconds(100));
        var result1 = memoized(5);
        var result2 = memoized(5); // Should use cache

        await Task.Delay(150); // Wait for expiration

        var result3 = memoized(5); // Should recompute

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(10, result2);
        Assert.Equal(10, result3);
        Assert.Equal(2, callCount); // Called twice: initial and after expiration
    }

    [Fact]
    public void MemoizeWithTimeout_BeforeExpiration_UsesCache()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> expensiveFunc = x =>
        {
            callCount++;
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.MemoizeWithTimeout(TimeSpan.FromSeconds(10));
        var result1 = memoized(5);
        var result2 = memoized(5);
        var result3 = memoized(5);

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(10, result2);
        Assert.Equal(10, result3);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void MemoizeWithTimeout_TwoParameters_ExpiresCorrectly()
    {
        // Arrange
        var callCount = 0;
        Func<int, int, int> add = (a, b) =>
        {
            callCount++;
            return a + b;
        };

        // Act
        var memoized = add.MemoizeWithTimeout(TimeSpan.FromSeconds(1));
        var result1 = memoized(2, 3);
        var result2 = memoized(2, 3);

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void MemoizeWithTimeout_InvalidTimeout_ThrowsException()
    {
        // Arrange
        Func<int, int> func = x => x * 2;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => func.MemoizeWithTimeout(TimeSpan.Zero));
        Assert.Throws<ArgumentException>(() => func.MemoizeWithTimeout(TimeSpan.FromSeconds(-1)));
    }

    #endregion

    #region Capacity Tests

    [Fact]
    public void MemoizeWithCapacity_ExceedsCapacity_EvictsOldest()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> square = x =>
        {
            callCount++;
            return x * x;
        };

        // Act
        var memoized = square.MemoizeWithCapacity(2); // Only cache 2 items

        var result1 = memoized(1); // Cache: {1}
        var result2 = memoized(2); // Cache: {1, 2}
        var result3 = memoized(3); // Cache: {2, 3} (1 evicted)
        var result4 = memoized(1); // Cache: {3, 1} (2 evicted) - recompute 1

        // Assert
        Assert.Equal(1, result1);
        Assert.Equal(4, result2);
        Assert.Equal(9, result3);
        Assert.Equal(1, result4);
        Assert.Equal(4, callCount); // 1, 2, 3, then 1 again
    }

    [Fact]
    public void MemoizeWithCapacity_WithinCapacity_DoesNotEvict()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> square = x =>
        {
            callCount++;
            return x * x;
        };

        // Act
        var memoized = square.MemoizeWithCapacity(10);

        for (int i = 0; i < 5; i++)
        {
            memoized(i);
        }

        // Call again
        for (int i = 0; i < 5; i++)
        {
            memoized(i);
        }

        // Assert
        Assert.Equal(5, callCount); // Each called only once
    }

    [Fact]
    public void MemoizeWithCapacity_InvalidCapacity_ThrowsException()
    {
        // Arrange
        Func<int, int> func = x => x * 2;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => func.MemoizeWithCapacity(0));
        Assert.Throws<ArgumentException>(() => func.MemoizeWithCapacity(-1));
    }

    #endregion

    #region Async Memoization Tests

    [Fact]
    public async Task MemoizeAsync_Task_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, Task<int>> expensiveFunc = async x =>
        {
            callCount++;
            await Task.Delay(10);
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.MemoizeAsync();
        var result1 = await memoized(5);
        var result2 = await memoized(5);
        var result3 = await memoized(5);

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(10, result2);
        Assert.Equal(10, result3);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task MemoizeAsync_TwoParameters_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, int, Task<int>> add = async (a, b) =>
        {
            callCount++;
            await Task.Delay(10);
            return a + b;
        };

        // Act
        var memoized = add.MemoizeAsync();
        var result1 = await memoized(2, 3);
        var result2 = await memoized(2, 3);

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task MemoizeAsyncWithTimeout_Expires_Recomputes()
    {
        // Arrange
        var callCount = 0;
        Func<int, Task<int>> expensiveFunc = async x =>
        {
            callCount++;
            await Task.Delay(10);
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.MemoizeAsyncWithTimeout(TimeSpan.FromMilliseconds(100));
        var result1 = await memoized(5);
        var result2 = await memoized(5);

        await Task.Delay(150);

        var result3 = await memoized(5);

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(10, result2);
        Assert.Equal(10, result3);
        Assert.Equal(2, callCount);
    }

    [Fact]
    public async Task MemoizeAsync_ConcurrentCalls_ExecutesOnce()
    {
        // Arrange
        var callCount = 0;
        Func<int, Task<int>> expensiveFunc = async x =>
        {
            callCount++;
            await Task.Delay(100);
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.MemoizeAsync();

        var tasks = Enumerable.Range(0, 10)
            .Select(_ => memoized(5))
            .ToArray();

        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.All(results, r => Assert.Equal(10, r));
        Assert.Equal(1, callCount); // Called only once despite concurrent calls
    }

    #endregion

    #region ValueTask Memoization Tests

    [Fact]
    public async Task MemoizeAsync_ValueTask_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, ValueTask<int>> expensiveFunc = async x =>
        {
            callCount++;
            await Task.Delay(10);
            return x * 2;
        };

        // Act
        var memoized = expensiveFunc.MemoizeAsync();
        var result1 = await memoized(5);
        var result2 = await memoized(5);
        var result3 = await memoized(5);

        // Assert
        Assert.Equal(10, result1);
        Assert.Equal(10, result2);
        Assert.Equal(10, result3);
        Assert.Equal(1, callCount);
    }

    [Fact]
    public async Task MemoizeAsync_ValueTask_TwoParameters_CachesResult()
    {
        // Arrange
        var callCount = 0;
        Func<int, int, ValueTask<int>> add = async (a, b) =>
        {
            callCount++;
            await Task.Delay(10);
            return a + b;
        };

        // Act
        var memoized = add.MemoizeAsync();
        var result1 = await memoized(2, 3);
        var result2 = await memoized(2, 3);

        // Assert
        Assert.Equal(5, result1);
        Assert.Equal(5, result2);
        Assert.Equal(1, callCount);
    }

    #endregion

    #region Cache Control Tests

    [Fact]
    public void MemoizeWithCache_ProvidesCacheControl()
    {
        // Arrange
        var callCount = 0;
        Func<int, int> square = x =>
        {
            callCount++;
            return x * x;
        };

        // Act
        var (memoized, cache) = square.MemoizeWithCache();

        var result1 = memoized(5);
        Assert.Equal(1, cache.Count);
        Assert.True(cache.Contains(5));

        var result2 = memoized(10);
        Assert.Equal(2, cache.Count);

        cache.Remove(5);
        Assert.Equal(1, cache.Count);
        Assert.False(cache.Contains(5));

        var result3 = memoized(5); // Should recompute

        // Assert
        Assert.Equal(25, result1);
        Assert.Equal(100, result2);
        Assert.Equal(25, result3);
        Assert.Equal(3, callCount); // Initial 5, then 10, then 5 again after removal
    }

    [Fact]
    public void MemoizeWithCache_Clear_RemovesAllEntries()
    {
        // Arrange
        Func<int, int> square = x => x * x;
        var (memoized, cache) = square.MemoizeWithCache();

        // Act
        memoized(1);
        memoized(2);
        memoized(3);
        Assert.Equal(3, cache.Count);

        cache.Clear();

        // Assert
        Assert.Equal(0, cache.Count);
        Assert.False(cache.Contains(1));
    }

    [Fact]
    public void MemoizeWithCache_TryGetValue_RetrievesCachedValue()
    {
        // Arrange
        Func<int, int> square = x => x * x;
        var (memoized, cache) = square.MemoizeWithCache();

        // Act
        memoized(5);

        var exists = cache.TryGetValue(5, out var value);
        var notExists = cache.TryGetValue(10, out var _);

        // Assert
        Assert.True(exists);
        Assert.Equal(25, value);
        Assert.False(notExists);
    }

    #endregion

    #region Performance and Fibonacci Example

    [Fact]
    public void Memoize_Fibonacci_ImprovesPerformance()
    {
        // Arrange - Naive recursive fibonacci (very slow)
        Func<int, int> naiveFib = null!;
        naiveFib = n => n <= 1 ? n : naiveFib(n - 1) + naiveFib(n - 2);

        // Act - Memoized version
        var memoizedFib = naiveFib.Memoize();

        // Compute without timing for warmup
        memoizedFib(30);

        // Now time a larger computation
        var sw = Stopwatch.StartNew();
        var result = memoizedFib(35);
        sw.Stop();

        // Assert - Should be very fast due to memoization
        Assert.Equal(9227465, result);
        Assert.True(sw.ElapsedMilliseconds < 100, $"Took {sw.ElapsedMilliseconds}ms, should be instant with memoization");
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void Memoize_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        Func<int, int> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullFunc.Memoize());
    }

    [Fact]
    public void Memoize_WithComparer_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        Func<string, int> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullFunc.Memoize(StringComparer.Ordinal));
    }

    [Fact]
    public void Memoize_WithComparer_NullComparer_ThrowsArgumentNullException()
    {
        // Arrange
        Func<string, int> func = s => s.Length;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => func.Memoize(null!));
    }

    [Fact]
    public void MemoizeWithTimeout_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        Func<int, int> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullFunc.MemoizeWithTimeout(TimeSpan.FromSeconds(1)));
    }

    [Fact]
    public void MemoizeAsync_NullFunction_ThrowsArgumentNullException()
    {
        // Arrange
        Func<int, Task<int>> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullFunc.MemoizeAsync());
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Memoize_ExpensiveDatabaseLookup_Scenario()
    {
        // Simulate expensive database lookup
        var dbCallCount = 0;
        Func<int, User> getUserById = id =>
        {
            dbCallCount++;
            // Simulate database delay
            Task.Delay(50).Wait();
            return new User { Id = id, Name = $"User{id}" };
        };

        var memoized = getUserById.Memoize();

        // Multiple requests for same user
        var user1 = memoized(42);
        var user2 = memoized(42);
        var user3 = memoized(42);

        Assert.Equal(42, user1.Id);
        Assert.Equal(user1.Name, user2.Name);
        Assert.Equal(user1.Name, user3.Name);
        Assert.Equal(1, dbCallCount); // Only one DB call
    }

    [Fact]
    public void MemoizeWithTimeout_ConfigurationCache_Scenario()
    {
        // Simulate fetching configuration that changes periodically
        var fetchCount = 0;
        var configVersion = 1;

        Func<string, string> getConfig = key =>
        {
            fetchCount++;
            return $"{key}=value_v{configVersion}";
        };

        var memoized = getConfig.MemoizeWithTimeout(TimeSpan.FromMilliseconds(50));

        var config1 = memoized("setting");
        Assert.Equal("setting=value_v1", config1);

        // Change the "remote" config
        configVersion = 2;

        // Should still get cached version
        var config2 = memoized("setting");
        Assert.Equal("setting=value_v1", config2);

        // Wait for cache expiration
        Task.Delay(60).Wait();

        // Should get new version now
        var config3 = memoized("setting");
        Assert.Equal("setting=value_v2", config3);

        Assert.Equal(2, fetchCount);
    }

    #endregion

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

