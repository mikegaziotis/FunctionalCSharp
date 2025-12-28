# Scan Extensions - Usage Guide

## Table of Contents
1. [Overview](#overview)
2. [Basic Scan](#basic-scan)
3. [Scan Right](#scan-right)
4. [Async Scan](#async-scan)
5. [Result Scan](#result-scan)
6. [Option Scan](#option-scan)
7. [Helper Methods](#helper-methods)
8. [Real-World Examples](#real-world-examples)
9. [Best Practices](#best-practices)

---

## Overview

**Scan** is like Fold (Aggregate) but returns all intermediate results instead of just the final value. It shows the progression of an accumulation operation, making it perfect for:

- **Cumulative calculations** - Running sums, products, balances
- **State tracking** - Event processing, state machines
- **Progress monitoring** - Watching values evolve over time
- **Validation with history** - Track where failures occur

### Key Concepts
- ✅ **Returns all steps** - Not just final result
- ✅ **Lazy evaluation** - Deferred execution like LINQ
- ✅ **Includes seed** - First element is the seed value
- ✅ **Short-circuit variants** - Result and Option variants stop on failure/None

### Scan vs Fold

```csharp
// Fold - Returns only final result
var sum = numbers.Fold(0, (acc, n) => acc + n);
// sum = 10

// Scan - Returns all intermediate results
var sums = numbers.Scan(0, (acc, n) => acc + n);
// sums = [0, 1, 3, 6, 10]
```

---

## Basic Scan

### Scan with Seed

Returns all intermediate results including the seed as the first element.

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var cumulative = numbers.Scan(0, (acc, n) => acc + n).ToArray();
// Returns: [0, 1, 3, 6, 10]
```

### Scan without Seed

Uses the first element as the initial accumulator, returns one fewer element.

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var cumulative = numbers.Scan((acc, n) => acc + n).ToArray();
// Returns: [1, 3, 6, 10]
```

### Common Operations

#### Sum Progression
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var sums = numbers.Scan(0, (acc, n) => acc + n);
// [0, 1, 3, 6, 10, 15]
```

#### Product Progression (Factorials)
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var products = numbers.Scan(1, (acc, n) => acc * n);
// [1, 1, 2, 6, 24, 120]
```

#### String Concatenation
```csharp
var words = new[] { "Hello", " ", "World", "!" };
var built = words.Scan("", (acc, w) => acc + w);
// ["", "Hello", "Hello ", "Hello World", "Hello World!"]
```

#### Maximum Tracking
```csharp
var numbers = new[] { 1, 5, 3, 9, 2 };
var maxes = numbers.Scan((max, n) => n > max ? n : max);
// [1, 5, 5, 9, 9]
```

### Empty Collections

```csharp
var empty = Array.Empty<int>();

// With seed - returns only seed
var result1 = empty.Scan(0, (acc, n) => acc + n);
// [0]

// Without seed - returns empty
var result2 = empty.Scan((acc, n) => acc + n);
// []
```

---

## Scan Right

Scans from right to left (right-associative).

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = numbers.ScanRight(0, (n, acc) => acc + n);
// [10, 9, 7, 4, 0]
```

### Understanding ScanRight

```csharp
// Left scan: ((((0 + 1) + 2) + 3) + 4)
var leftScan = numbers.Scan(0, (acc, n) => acc + n);
// [0, 1, 3, 6, 10]

// Right scan: (1 + (2 + (3 + (4 + 0))))
var rightScan = numbers.ScanRight(0, (n, acc) => n + acc);
// [10, 9, 7, 4, 0]
```

### Right Associativity Example

```csharp
var numbers = new[] { 1, 2, 3, 4 };

// Subtraction shows the difference
var leftSub = numbers.Scan(0, (acc, n) => acc - n);
// [0, -1, -3, -6, -10]

var rightSub = numbers.ScanRight(0, (n, acc) => n - acc);
// [-2, -1, 1, 4, 0]
// Computed as: 1 - (2 - (3 - (4 - 0)))
```

---

## Async Scan

### Basic Async

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = await numbers.ScanAsync(0, async (acc, n) =>
{
    await Task.Delay(10); // Simulate async work
    return acc + n;
});
// [0, 1, 3, 6, 10]
```

### Task Collection

```csharp
Task<IEnumerable<int>> taskNumbers = GetNumbersAsync();
var result = await taskNumbers.Scan(0, (acc, n) => acc + n);
```

### Task Collection with Async Function

```csharp
Task<IEnumerable<int>> taskNumbers = GetNumbersAsync();
var result = await taskNumbers.ScanAsync(0, async (acc, n) =>
{
    await ProcessAsync(n);
    return acc + n;
});
```

### ValueTask Variant

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = await numbers.ScanAsync(0, async (acc, n) =>
{
    if (n < 100)
        return acc + n; // Synchronous path

    await Task.Delay(10);
    return acc + n;
});
```

---

## Result Scan

Scan with Result-returning accumulator - stops on first failure.

### Basic Result Scan

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = numbers.ScanResult(0, (acc, n) =>
    Result.Success<int, string>(acc + n));

// result.IsSuccess = true
// result.Value = [0, 1, 3, 6, 10]
```

### With Validation

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var result = numbers.ScanResult(0, (acc, n) =>
{
    var sum = acc + n;
    return sum <= 10
        ? Result.Success<int, string>(sum)
        : Result.Failure<int, string>($"Sum {sum} exceeds limit");
});

// result.IsFailure = true
// result.Error = "Sum 15 exceeds limit"
```

### Accumulation with Constraints

```csharp
var transactions = new[] { 100, -50, 200, -30, -20 };
var result = transactions.ScanResult(0, (balance, transaction) =>
{
    var newBalance = balance + transaction;
    return newBalance >= 0
        ? Result.Success<int, string>(newBalance)
        : Result.Failure<int, string>($"Insufficient funds: {newBalance}");
});
```

### Async Result Scan

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = await numbers.ScanResultAsync(0, async (acc, n) =>
{
    await ValidateAsync(acc, n);
    var sum = acc + n;
    return sum <= 10
        ? Result.Success<int, string>(sum)
        : Result.Failure<int, string>("Limit exceeded");
});
```

### ValueTask Variant

```csharp
var result = await numbers.ScanResultAsync(0, async (acc, n) =>
{
    await Task.Delay(1);
    return Result.Success<int, string>(acc + n);
});
```

---

## Option Scan

Scan with Option-returning accumulator - stops on first None.

### Basic Option Scan

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = numbers.ScanOption(0, (acc, n) =>
    Option<int>.From(acc + n));

// result.HasValue = true
// result.Value = [0, 1, 3, 6, 10]
```

### With Conditional

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var result = numbers.ScanOption(0, (acc, n) =>
    acc + n < 10
        ? Option<int>.From(acc + n)
        : Option<int>.None);

// result.HasNoValue = true (stopped at sum = 10)
```

### Safe Division Example

```csharp
var numbers = new[] { 100, 2, 0, 5 };
var result = numbers.ScanOption(1000, (acc, n) =>
    n != 0
        ? Option<int>.From(acc / n)
        : Option<int>.None);

// result.HasNoValue = true (stopped at division by zero)
```

### Async Option Scan

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var result = await numbers.ScanOptionAsync(0, async (acc, n) =>
{
    await Task.Delay(1);
    return Option<int>.From(acc + n);
});
```

---

## Helper Methods

### CumulativeSum

Convenience method for cumulative sums.

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };

// Using Scan
var sums1 = numbers.Scan(0, (acc, n) => acc + n);

// Using CumulativeSum (more explicit)
var sums2 = numbers.CumulativeSum();

// Both return: [0, 1, 3, 6, 10, 15]
```

#### Multiple Types

```csharp
// int
var intSums = intArray.CumulativeSum();

// long
var longSums = longArray.CumulativeSum();

// decimal
var decimalSums = decimalArray.CumulativeSum();
```

### CumulativeProduct

Convenience method for cumulative products.

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var products = numbers.CumulativeProduct();
// [1, 1, 2, 6, 24, 120] (factorials!)
```

### RunningMax

Track the maximum value seen so far.

```csharp
var numbers = new[] { 1, 5, 3, 9, 2, 7 };
var maxes = numbers.RunningMax();
// [1, 5, 5, 9, 9, 9]
```

### RunningMin

Track the minimum value seen so far.

```csharp
var numbers = new[] { 5, 3, 7, 1, 9, 2 };
var mins = numbers.RunningMin();
// [5, 3, 3, 1, 1, 1]
```

---

## Real-World Examples

### Example 1: Account Balance Tracking

```csharp
public class AccountTracker
{
    public List<decimal> TrackBalance(IEnumerable<Transaction> transactions)
    {
        return transactions
            .Select(t => t.Amount)
            .Scan(0m, (balance, amount) => balance + amount)
            .ToList();
    }

    public Result<List<decimal>, string> TrackBalanceWithValidation(
        IEnumerable<Transaction> transactions)
    {
        return transactions
            .Select(t => t.Amount)
            .ScanResult(0m, (balance, amount) =>
            {
                var newBalance = balance + amount;
                return newBalance >= 0
                    ? Result.Success<decimal, string>(newBalance)
                    : Result.Failure<decimal, string>(
                        $"Insufficient funds: balance would be {newBalance:C}");
            });
    }
}
```

### Example 2: Bandwidth Usage Monitor

```csharp
public class BandwidthMonitor
{
    private const int MaxBandwidth = 1000; // MB

    public Result<IEnumerable<int>, string> MonitorUsage(
        IEnumerable<int> requestSizes)
    {
        return requestSizes.ScanResult(0, (used, size) =>
        {
            var newUsed = used + size;
            return newUsed <= MaxBandwidth
                ? Result.Success<int, string>(newUsed)
                : Result.Failure<int, string>(
                    $"Bandwidth limit exceeded: {newUsed}MB > {MaxBandwidth}MB");
        });
    }

    public (int peak, List<int> usage) GetPeakUsage(IEnumerable<int> requestSizes)
    {
        var usage = requestSizes.Scan(0, (acc, size) => acc + size).ToList();
        var peak = usage.Max();
        return (peak, usage);
    }
}
```

### Example 3: Event Processing with State

```csharp
public class StateMachine
{
    public enum State { Idle, Processing, Complete, Error }
    public enum Event { Start, Process, Finish, Fail }

    public List<State> ProcessEvents(IEnumerable<Event> events)
    {
        return events
            .Scan(State.Idle, (state, evt) => (state, evt) switch
            {
                (State.Idle, Event.Start) => State.Processing,
                (State.Processing, Event.Process) => State.Processing,
                (State.Processing, Event.Finish) => State.Complete,
                (_, Event.Fail) => State.Error,
                _ => state
            })
            .ToList();
    }
}
```

### Example 4: Stock Price Analysis

```csharp
public class StockAnalyzer
{
    public List<decimal> CalculateMovingAverage(
        IEnumerable<decimal> prices,
        int window)
    {
        var pricesArray = prices.ToArray();
        var sums = pricesArray.CumulativeSum().Skip(1).ToArray();

        return sums
            .Select((sum, i) =>
                i < window - 1
                    ? sum / (i + 1)
                    : (sum - sums[i - window]) / window)
            .ToList();
    }

    public List<decimal> TrackPeakPrices(IEnumerable<decimal> prices)
    {
        return prices.RunningMax().ToList();
    }

    public List<bool> DetectNewHighs(IEnumerable<decimal> prices)
    {
        var pricesArray = prices.ToArray();
        var peaks = pricesArray.RunningMax().ToArray();

        return pricesArray
            .Zip(peaks, (price, peak) => price == peak)
            .ToList();
    }
}
```

### Example 5: String Builder with Validation

```csharp
public class LimitedStringBuilder
{
    private readonly int _maxLength;

    public LimitedStringBuilder(int maxLength)
    {
        _maxLength = maxLength;
    }

    public Result<IEnumerable<string>, string> BuildWithSteps(
        IEnumerable<string> parts)
    {
        return parts.ScanResult("", (acc, part) =>
        {
            var newValue = acc + part;
            return newValue.Length <= _maxLength
                ? Result.Success<string, string>(newValue)
                : Result.Failure<string, string>(
                    $"Length {newValue.Length} exceeds max {_maxLength}");
        });
    }

    public string BuildUntilLimit(IEnumerable<string> parts)
    {
        return parts
            .Scan("", (acc, part) => acc + part)
            .TakeWhile(s => s.Length <= _maxLength)
            .LastOrDefault() ?? "";
    }
}
```

### Example 6: Factorial Calculator

```csharp
public class MathOperations
{
    public List<long> CalculateFactorials(int n)
    {
        return Enumerable.Range(1, n)
            .CumulativeProduct()
            .Skip(1) // Skip the initial 1
            .ToList();
    }

    public Result<long, string> CalculateFactorialSafe(int n)
    {
        if (n < 0)
            return Result.Failure<long, string>("n must be non-negative");

        if (n == 0 || n == 1)
            return Result.Success<long, string>(1);

        var result = Enumerable.Range(1, n)
            .ScanResult(1L, (acc, i) =>
            {
                var product = acc * i;
                return product > 0 // Check for overflow
                    ? Result.Success<long, string>(product)
                    : Result.Failure<long, string>($"Overflow at {i}!");
            });

        return result.IsSuccess
            ? Result.Success<long, string>(result.Value.Last())
            : result.MapError(e => e);
    }
}
```

### Example 7: Progress Tracking

```csharp
public class ProgressTracker
{
    public async Task<List<int>> TrackProgressAsync(
        IEnumerable<Func<Task>> tasks)
    {
        var progress = new List<int>();
        var completed = 0;

        var taskArray = tasks.ToArray();
        progress.Add(0);

        foreach (var task in taskArray)
        {
            await task();
            completed++;
            progress.Add(completed);
        }

        return progress;
    }

    public List<double> CalculatePercentages(int total, IEnumerable<int> completed)
    {
        return completed
            .Scan(0, (acc, n) => acc + n)
            .Select(n => (double)n / total * 100)
            .ToList();
    }
}
```

---

## Best Practices

### 1. Choose Between Scan and Fold

```csharp
// ✅ Use Fold when you only need the final result
var total = numbers.Fold(0, (acc, n) => acc + n);

// ✅ Use Scan when you need to track progression
var progression = numbers.Scan(0, (acc, n) => acc + n);
```

### 2. Use Helper Methods When Applicable

```csharp
// ❌ Verbose
var sums = numbers.Scan(0, (acc, n) => acc + n);

// ✅ Clear intent
var sums = numbers.CumulativeSum();
```

### 3. Leverage Lazy Evaluation

```csharp
// ✅ Lazy - Only compute what's needed
var scan = numbers.Scan(0, (acc, n) => acc + n);
var first5 = scan.Take(5).ToList();

// Only first 5 steps are computed
```

### 4. Use Result/Option Scan for Validation

```csharp
// ✅ Track where validation fails
var result = items.ScanResult(initialState, (state, item) =>
    ValidateTransition(state, item));

if (result.IsFailure)
{
    Console.WriteLine($"Failed at step: {result.Error}");
}
```

### 5. Combine with Other Operations

```csharp
// ✅ Chain with other LINQ operations
var analysis = prices
    .CumulativeSum()
    .Skip(1) // Remove initial 0
    .Select((sum, i) => sum / (i + 1)) // Calculate average
    .ToList();
```

### 6. Consider ScanRight for Different Associativity

```csharp
// For operations where order matters
var leftResult = items.Scan(seed, (acc, item) => Combine(acc, item));
var rightResult = items.ScanRight(seed, (item, acc) => Combine(item, acc));
```

---

## Performance Considerations

### Time Complexity
- **Scan**: O(n) - Single pass through collection
- **ScanRight**: O(n) - Must materialize to array first
- **Lazy evaluation** - Only computes when enumerated

### Space Complexity
- **Scan**: O(1) - Lazy enumeration (deferred)
- **ScanRight**: O(n) - Must store all results
- **Materialized**: O(n) - When calling ToList() or ToArray()

### Lazy Evaluation

```csharp
// ✅ Efficient - Only computes first 3
var first3 = numbers
    .Scan(0, (acc, n) => acc + n)
    .Take(3)
    .ToList();

// ❌ Inefficient - Computes all, then takes 3
var all = numbers.Scan(0, (acc, n) => acc + n).ToList();
var first3Bad = all.Take(3).ToList();
```

---

## Comparison with Alternatives

### vs Aggregate/Fold

```csharp
// Aggregate - Only final result
var sum = numbers.Aggregate(0, (acc, n) => acc + n);
// sum = 15

// Scan - All intermediate results
var sums = numbers.Scan(0, (acc, n) => acc + n);
// sums = [0, 1, 3, 6, 10, 15]
```

### vs Manual Loop

```csharp
// ❌ Manual - Verbose
var results = new List<int> { 0 };
var acc = 0;
foreach (var n in numbers)
{
    acc += n;
    results.Add(acc);
}

// ✅ Scan - Concise
var results = numbers.Scan(0, (acc, n) => acc + n).ToList();
```

### vs Select

```csharp
// Select - Independent transformations
var doubled = numbers.Select(n => n * 2);

// Scan - Dependent on previous results
var cumulative = numbers.Scan(0, (acc, n) => acc + n);
```

---

## Summary

**Scan** provides powerful fold-with-history functionality:

- ✅ **Shows progression** - All intermediate states
- ✅ **Lazy evaluation** - Deferred like LINQ
- ✅ **Short-circuit variants** - Result and Option support
- ✅ **Helper methods** - CumulativeSum, RunningMax, etc.
- ✅ **Async support** - Task and ValueTask variants
- ✅ **Left and right** - Both associativities supported

Use Scan whenever you need to track how a value evolves through a collection, not just the final result.

