using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class ScanExtensionsTests
{
    #region Basic Scan Tests

    [Fact]
    public void Scan_WithSeed_ReturnsAllIntermediateResults()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = numbers.Scan(0, (acc, n) => acc + n).ToArray();

        // Assert
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result);
    }

    [Fact]
    public void Scan_WithoutSeed_UsesFirstElement()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = numbers.Scan((acc, n) => acc + n).ToArray();

        // Assert
        Assert.Equal(new[] { 1, 3, 6, 10 }, result);
    }

    [Fact]
    public void Scan_EmptyCollection_ReturnsOnlySeed()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var result = numbers.Scan(0, (acc, n) => acc + n).ToArray();

        // Assert
        Assert.Equal(new[] { 0 }, result);
    }

    [Fact]
    public void Scan_EmptyCollectionWithoutSeed_ReturnsEmpty()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var result = numbers.Scan((acc, n) => acc + n).ToArray();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Scan_SingleElement_ReturnsSeedAndElement()
    {
        // Arrange
        var numbers = new[] { 5 };

        // Act
        var result = numbers.Scan(0, (acc, n) => acc + n).ToArray();

        // Assert
        Assert.Equal(new[] { 0, 5 }, result);
    }

    [Fact]
    public void Scan_Multiplication_ReturnsFactorials()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.Scan(1, (acc, n) => acc * n).ToArray();

        // Assert
        Assert.Equal(new[] { 1, 1, 2, 6, 24, 120 }, result);
    }

    [Fact]
    public void Scan_StringConcatenation_ReturnsProgression()
    {
        // Arrange
        var words = new[] { "Hello", " ", "World", "!" };

        // Act
        var result = words.Scan("", (acc, w) => acc + w).ToArray();

        // Assert
        Assert.Equal(new[] { "", "Hello", "Hello ", "Hello World", "Hello World!" }, result);
    }

    #endregion

    #region Scan Right Tests

    [Fact]
    public void ScanRight_ReturnsIntermediateResultsFromRight()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = numbers.ScanRight(0, (n, acc) => acc + n).ToArray();

        // Assert
        Assert.Equal(new[] { 10, 9, 7, 4, 0 }, result);
    }

    [Fact]
    public void ScanRight_EmptyCollection_ReturnsOnlySeed()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var result = numbers.ScanRight(0, (n, acc) => acc + n).ToArray();

        // Assert
        Assert.Equal(new[] { 0 }, result);
    }

    [Fact]
    public void ScanRight_ListSubtraction_ShowsRightAssociativity()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = numbers.ScanRight(0, (n, acc) => n - acc).ToArray();

        // Assert
        // 1 - (2 - (3 - (4 - 0))) = 1 - (2 - (3 - 4)) = 1 - (2 - (-1)) = 1 - 3 = -2
        Assert.Equal(new[] { -2, -1, 1, 4, 0 }, result);
    }

    #endregion

    #region Async Scan Tests

    [Fact]
    public async Task ScanAsync_WithAsyncFunction_ReturnsAllIntermediateResults()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = await numbers.ScanAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return acc + n;
        });

        // Assert
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result);
    }

    [Fact]
    public async Task Scan_TaskCollection_ReturnsAllIntermediateResults()
    {
        // Arrange
        var taskNumbers = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3, 4 });

        // Act
        var result = await taskNumbers.Scan(0, (acc, n) => acc + n);

        // Assert
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result);
    }

    [Fact]
    public async Task ScanAsync_TaskCollection_WithAsyncFunction_Works()
    {
        // Arrange
        var taskNumbers = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3, 4 });

        // Act
        var result = await taskNumbers.ScanAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return acc + n;
        });

        // Assert
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result);
    }

    [Fact]
    public async Task ScanAsync_ValueTask_ReturnsAllIntermediateResults()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = await numbers.ScanAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return acc + n;
        });

        // Assert
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result);
    }

    #endregion

    #region Result Scan Tests

    [Fact]
    public void ScanResult_AllSucceed_ReturnsSuccessWithAllResults()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = numbers.ScanResult(0, (acc, n) =>
            Result.Success<int, string>(acc + n));

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result.Value);
    }

    [Fact]
    public void ScanResult_OneFailure_ReturnsFailure()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.ScanResult(0, (acc, n) =>
            acc + n < 10
                ? Result.Success<int, string>(acc + n)
                : Result.Failure<int, string>("Sum too large"));

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Sum too large", result.Error);
    }

    [Fact]
    public void ScanResult_ValidationScenario()
    {
        // Arrange
        var increments = new[] { 1, 2, 3, 4 };

        // Act
        var result = increments.ScanResult(0, (acc, increment) =>
        {
            var newValue = acc + increment;
            return newValue <= 6
                ? Result.Success<int, string>(newValue)
                : Result.Failure<int, string>($"Value {newValue} exceeds limit");
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Value 7 exceeds limit", result.Error);
    }

    [Fact]
    public async Task ScanResultAsync_AllSucceed_ReturnsSuccess()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = await numbers.ScanResultAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return Result.Success<int, string>(acc + n);
        });

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result.Value);
    }

    [Fact]
    public async Task ScanResultAsync_OneFailure_ReturnsFailure()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = await numbers.ScanResultAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return acc + n < 10
                ? Result.Success<int, string>(acc + n)
                : Result.Failure<int, string>("Sum too large");
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Sum too large", result.Error);
    }

    [Fact]
    public async Task ScanResultAsync_ValueTask_AllSucceed_ReturnsSuccess()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = await numbers.ScanResultAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return Result.Success<int, string>(acc + n);
        });

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result.Value);
    }

    #endregion

    #region Option Scan Tests

    [Fact]
    public void ScanOption_AllSome_ReturnsSomeWithAllResults()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = numbers.ScanOption(0, (acc, n) =>
            Option<int>.From(acc + n));

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result.GetValueOrThrow());
    }

    [Fact]
    public void ScanOption_OneNone_ReturnsNone()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.ScanOption(0, (acc, n) =>
            acc + n < 10
                ? Option<int>.From(acc + n)
                : Option<int>.None);

        // Assert
        Assert.True(result.HasNoValue);
    }

    [Fact]
    public async Task ScanOptionAsync_AllSome_ReturnsSome()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = await numbers.ScanOptionAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return Option<int>.From(acc + n);
        });

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result.GetValueOrThrow());
    }

    [Fact]
    public async Task ScanOptionAsync_OneNone_ReturnsNone()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = await numbers.ScanOptionAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return acc + n < 10
                ? Option<int>.From(acc + n)
                : Option<int>.None;
        });

        // Assert
        Assert.True(result.HasNoValue);
    }

    [Fact]
    public async Task ScanOptionAsync_ValueTask_AllSome_ReturnsSome()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var result = await numbers.ScanOptionAsync(0, async (acc, n) =>
        {
            await Task.Delay(1);
            return Option<int>.From(acc + n);
        });

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(new[] { 0, 1, 3, 6, 10 }, result.GetValueOrThrow());
    }

    #endregion

    #region Helper Method Tests

    [Fact]
    public void CumulativeSum_Int_ReturnsPartialSums()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.CumulativeSum().ToArray();

        // Assert
        Assert.Equal(new[] { 0, 1, 3, 6, 10, 15 }, result);
    }

    [Fact]
    public void CumulativeSum_Long_ReturnsPartialSums()
    {
        // Arrange
        var numbers = new long[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.CumulativeSum().ToArray();

        // Assert
        Assert.Equal(new long[] { 0, 1, 3, 6, 10, 15 }, result);
    }

    [Fact]
    public void CumulativeSum_Decimal_ReturnsPartialSums()
    {
        // Arrange
        var numbers = new decimal[] { 1.5m, 2.5m, 3.0m };

        // Act
        var result = numbers.CumulativeSum().ToArray();

        // Assert
        Assert.Equal(new decimal[] { 0m, 1.5m, 4.0m, 7.0m }, result);
    }

    [Fact]
    public void CumulativeProduct_Int_ReturnsPartialProducts()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.CumulativeProduct().ToArray();

        // Assert
        Assert.Equal(new[] { 1, 1, 2, 6, 24, 120 }, result);
    }

    [Fact]
    public void CumulativeProduct_Long_ReturnsPartialProducts()
    {
        // Arrange
        var numbers = new long[] { 1, 2, 3, 4, 5 };

        // Act
        var result = numbers.CumulativeProduct().ToArray();

        // Assert
        Assert.Equal(new long[] { 1, 1, 2, 6, 24, 120 }, result);
    }

    [Fact]
    public void RunningMax_ReturnsMaximumAtEachStep()
    {
        // Arrange
        var numbers = new[] { 1, 5, 3, 9, 2, 7 };

        // Act
        var result = numbers.RunningMax().ToArray();

        // Assert
        Assert.Equal(new[] { 1, 5, 5, 9, 9, 9 }, result);
    }

    [Fact]
    public void RunningMin_ReturnsMinimumAtEachStep()
    {
        // Arrange
        var numbers = new[] { 5, 3, 7, 1, 9, 2 };

        // Act
        var result = numbers.RunningMin().ToArray();

        // Assert
        Assert.Equal(new[] { 5, 3, 3, 1, 1, 1 }, result);
    }

    [Fact]
    public void RunningMax_EmptyCollection_ReturnsEmpty()
    {
        // Arrange
        var numbers = Array.Empty<int>();

        // Act
        var result = numbers.RunningMax().ToArray();

        // Assert
        Assert.Empty(result);
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void Scan_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Scan(0, (acc, n) => acc + n).ToList());
    }

    [Fact]
    public void Scan_NullFunc_ThrowsArgumentNullException()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, int, int> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => numbers.Scan(0, nullFunc).ToList());
    }

    [Fact]
    public void ScanResult_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            nullSource.ScanResult(0, (acc, n) => Result.Success<int, string>(acc + n)));
    }

    [Fact]
    public void CumulativeSum_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.CumulativeSum().ToList());
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_AccountBalance_TrackProgressionOverTime()
    {
        // Arrange
        var transactions = new[] { 100, -50, 200, -30, -20 };

        // Act
        var balances = transactions.Scan(0, (balance, transaction) => balance + transaction).ToArray();

        // Assert
        Assert.Equal(new[] { 0, 100, 50, 250, 220, 200 }, balances);
        Assert.Equal(200, balances.Last()); // Final balance
    }

    [Fact]
    public void Scenario_ValidationWithLimit_StopsOnExcess()
    {
        // Arrange
        var requestSizes = new[] { 10, 20, 30, 40, 50 };
        var maxBandwidth = 80;

        // Act
        var result = requestSizes.ScanResult(0, (used, size) =>
        {
            var newUsed = used + size;
            return newUsed <= maxBandwidth
                ? Result.Success<int, string>(newUsed)
                : Result.Failure<int, string>($"Bandwidth limit exceeded: {newUsed} > {maxBandwidth}");
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("exceeded", result.Error);
    }

    [Fact]
    public void Scenario_StockPriceChanges_CalculateDailyReturns()
    {
        // Arrange
        var prices = new[] { 100m, 102m, 98m, 105m, 103m };

        // Act - Calculate percentage changes
        var returns = prices
            .Scan((prev, current) => current)
            .Skip(1) // Skip initial price
            .Zip(prices, (current, prev) => (current - prev) / prev * 100)
            .ToArray();

        // Assert
        Assert.Equal(5, prices.Length);
        Assert.Equal(4, returns.Length); // One less than prices
    }

    [Fact]
    public void Scenario_BuildStringWithLength_Validation()
    {
        // Arrange
        var words = new[] { "Hello", " ", "World", "!", " ", "Testing" };
        var maxLength = 15;

        // Act
        var result = words.ScanResult("", (acc, word) =>
        {
            var newValue = acc + word;
            return newValue.Length <= maxLength
                ? Result.Success<string, string>(newValue)
                : Result.Failure<string, string>($"Length {newValue.Length} exceeds max {maxLength}");
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("exceeds", result.Error);
    }

    [Fact]
    public void Scenario_RollingAverage_UsingRunningSum()
    {
        // Arrange
        var values = new[] { 10, 20, 30, 40, 50 };

        // Act - Calculate cumulative sums, then derive averages
        var cumulativeSums = values.CumulativeSum().Skip(1).ToArray(); // Skip initial 0
        var averages = cumulativeSums.Select((sum, i) => sum / (i + 1)).ToArray();

        // Assert
        Assert.Equal(new[] { 10, 15, 20, 25, 30 }, averages);
    }

    [Fact]
    public void Scenario_EventProcessing_StateTracking()
    {
        // Arrange
        var events = new[] { "START", "INCREMENT", "INCREMENT", "DECREMENT", "STOP" };

        // Act
        var states = events.Scan(0, (state, evt) => evt switch
        {
            "START" => 0,
            "INCREMENT" => state + 1,
            "DECREMENT" => state - 1,
            "STOP" => state,
            _ => state
        }).ToArray();

        // Assert
        Assert.Equal(new[] { 0, 0, 1, 2, 1, 1 }, states);
    }

    [Fact]
    public void Scenario_FactorialCalculation_UsingProduct()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };

        // Act
        var factorials = numbers.CumulativeProduct().Skip(1).ToArray(); // Skip initial 1

        // Assert
        Assert.Equal(new[] { 1, 2, 6, 24, 120 }, factorials);
        Assert.Equal(120, factorials.Last()); // 5!
    }

    [Fact]
    public void Scenario_PeakDetection_UsingRunningMax()
    {
        // Arrange
        var values = new[] { 1, 5, 3, 9, 2, 10, 7 };

        // Act
        var runningMax = values.RunningMax().ToArray();
        var isPeak = values.Zip(runningMax, (val, max) => val == max).ToArray();

        // Assert
        Assert.True(isPeak[0]); // 1 is peak (first)
        Assert.True(isPeak[1]); // 5 is peak
        Assert.False(isPeak[2]); // 3 is not peak
        Assert.True(isPeak[3]); // 9 is peak
        Assert.False(isPeak[4]); // 2 is not peak
        Assert.True(isPeak[5]); // 10 is peak
        Assert.False(isPeak[6]); // 7 is not peak
    }

    #endregion

    #region Lazy Evaluation Tests

    [Fact]
    public void Scan_LazyEvaluation_DoesNotEnumerateUntilNeeded()
    {
        // Arrange
        var callCount = 0;
        var numbers = Enumerable.Range(1, 10).Select(n =>
        {
            callCount++;
            return n;
        });

        // Act - Just create the scan, don't enumerate
        var scan = numbers.Scan(0, (acc, n) => acc + n);

        // Assert - Not enumerated yet
        Assert.Equal(0, callCount);

        // Now enumerate
        var result = scan.ToList();
        Assert.Equal(10, callCount);
    }

    #endregion
}

