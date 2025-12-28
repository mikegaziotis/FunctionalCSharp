# Partition Extensions - Usage Guide

## Table of Contents
1. [Overview](#overview)
2. [Result Partition](#result-partition)
3. [Option Partition](#option-partition)
4. [Conditional Partition](#conditional-partition)
5. [Helper Methods](#helper-methods)
6. [Real-World Examples](#real-world-examples)
7. [Best Practices](#best-practices)

---

## Overview

**Partition** operations split collections of Results or Options into their constituent parts:
- **Result Partition**: Separates successes from failures
- **Option Partition**: Separates values from nones

### Key Benefits
- ✅ **Efficient** - Single pass through collection
- ✅ **Type-safe** - Returns strongly-typed tuples or objects
- ✅ **Convenient** - Multiple return formats (tuple, object, helpers)
- ✅ **Async support** - Full Task and ValueTask variants
- ✅ **Flexible** - Partition existing collections or with selectors

---

## Result Partition

### Basic Partition

Separates a collection of Results into successes and failures.

```csharp
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Failure<int, string>("error1"),
    Result.Success<int, string>(2),
    Result.Failure<int, string>("error2"),
    Result.Success<int, string>(3)
};

var (successes, failures) = results.Partition();
// successes = [1, 2, 3]
// failures = ["error1", "error2"]
```

### All Successes

```csharp
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Success<int, string>(2),
    Result.Success<int, string>(3)
};

var (successes, failures) = results.Partition();
// successes = [1, 2, 3]
// failures = [] (empty)
```

### All Failures

```csharp
var results = new[]
{
    Result.Failure<int, string>("error1"),
    Result.Failure<int, string>("error2"),
    Result.Failure<int, string>("error3")
};

var (successes, failures) = results.Partition();
// successes = [] (empty)
// failures = ["error1", "error2", "error3"]
```

### Using PartitionToLists

Returns a `PartitionResult<T, TE>` object with additional properties:

```csharp
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Failure<int, string>("error"),
    Result.Success<int, string>(2)
};

var partition = results.PartitionToLists();

Console.WriteLine($"Success count: {partition.SuccessCount}");     // 2
Console.WriteLine($"Failure count: {partition.FailureCount}");     // 1
Console.WriteLine($"Total count: {partition.TotalCount}");         // 3
Console.WriteLine($"Has successes: {partition.HasSuccesses}");     // true
Console.WriteLine($"Has failures: {partition.HasFailures}");       // true
Console.WriteLine($"All succeeded: {partition.AllSucceeded}");     // false
Console.WriteLine($"All failed: {partition.AllFailed}");           // false

// Access the collections
var sum = partition.Successes.Sum();
var errorMessages = string.Join(", ", partition.Failures);

// Deconstruct
var (successes, failures) = partition;
```

### Async Variants

```csharp
// Collection of Task Results
var tasks = new[]
{
    FetchDataAsync(1),
    FetchDataAsync(2),
    FetchDataAsync(3)
};

var (successes, failures) = await tasks.Partition();
```

```csharp
// Task of collection
Task<IEnumerable<Result<int, string>>> taskResults = GetResultsAsync();
var (successes, failures) = await taskResults.Partition();
```

```csharp
// ValueTask variant
var valueTasks = new[]
{
    ValueTask.FromResult(Result.Success<int, string>(1)),
    ValueTask.FromResult(Result.Failure<int, string>("error"))
};

var (successes, failures) = await valueTasks.Partition();
```

---

## Option Partition

### Basic Partition

Separates a collection of Options into values and counts of nones.

```csharp
var options = new[]
{
    Option<int>.From(1),
    Option<int>.None,
    Option<int>.From(2),
    Option<int>.None,
    Option<int>.From(3)
};

var (values, noneCount) = options.Partition();
// values = [1, 2, 3]
// noneCount = 2
```

### All Some

```csharp
var options = new[]
{
    Option<int>.From(1),
    Option<int>.From(2),
    Option<int>.From(3)
};

var (values, noneCount) = options.Partition();
// values = [1, 2, 3]
// noneCount = 0
```

### All None

```csharp
var options = new[]
{
    Option<int>.None,
    Option<int>.None,
    Option<int>.None
};

var (values, noneCount) = options.Partition();
// values = [] (empty)
// noneCount = 3
```

### Using PartitionToResult

Returns a `PartitionOption<T>` object with additional properties:

```csharp
var options = new[]
{
    Option<int>.From(1),
    Option<int>.None,
    Option<int>.From(2)
};

var partition = options.PartitionToResult();

Console.WriteLine($"Value count: {partition.ValueCount}");         // 2
Console.WriteLine($"None count: {partition.NoneCount}");           // 1
Console.WriteLine($"Total count: {partition.TotalCount}");         // 3
Console.WriteLine($"Has values: {partition.HasValues}");           // true
Console.WriteLine($"Has nones: {partition.HasNones}");             // true
Console.WriteLine($"All have values: {partition.AllHaveValues}");  // false
Console.WriteLine($"All none: {partition.AllNone}");               // false

// Access the collection
var sum = partition.Values.Sum();

// Deconstruct
var (values, noneCount) = partition;
```

### Async Variants

```csharp
// Collection of Task Options
var tasks = new[]
{
    FetchOptionalDataAsync(1),
    FetchOptionalDataAsync(2),
    FetchOptionalDataAsync(3)
};

var (values, noneCount) = await tasks.Partition();
```

```csharp
// Task of collection
Task<IEnumerable<Option<int>>> taskOptions = GetOptionsAsync();
var (values, noneCount) = await taskOptions.Partition();
```

```csharp
// ValueTask variant
var valueTasks = new[]
{
    ValueTask.FromResult(Option<int>.From(1)),
    ValueTask.FromResult(Option<int>.None)
};

var (values, noneCount) = await valueTasks.Partition();
```

---

## Conditional Partition

### PartitionWith - Result

Partition based on a selector that returns a Result:

```csharp
var numbers = new[] { 1, 2, 3, 4, 5, 6 };

var (evens, odds) = numbers.PartitionWith(n =>
    n % 2 == 0
        ? Result.Success<int, int>(n)
        : Result.Failure<int, int>(n));

// evens = [2, 4, 6]
// odds = [1, 3, 5]
```

#### Validation Example

```csharp
var strings = new[] { "1", "2", "invalid", "3", "bad" };

var (numbers, errors) = strings.PartitionWith(s =>
    int.TryParse(s, out var n)
        ? Result.Success<int, string>(n)
        : Result.Failure<int, string>(s));

// numbers = [1, 2, 3]
// errors = ["invalid", "bad"]
```

#### With Transformation

```csharp
var inputs = new[] { "apple", "banana", "x", "cherry" };

var (valid, invalid) = inputs.PartitionWith(s =>
    s.Length >= 3
        ? Result.Success<string, string>(s.ToUpper())
        : Result.Failure<string, string>(s));

// valid = ["APPLE", "BANANA", "CHERRY"]
// invalid = ["x"]
```

### PartitionWith - Option

Partition based on a selector that returns an Option:

```csharp
var strings = new[] { "1", "2", "invalid", "3", "bad" };

var (numbers, invalidCount) = strings.PartitionWith(s =>
    int.TryParse(s, out var n)
        ? Option<int>.From(n)
        : Option<int>.None);

// numbers = [1, 2, 3]
// invalidCount = 2
```

#### Dictionary Lookup Example

```csharp
var keys = new[] { "a", "b", "missing", "c", "invalid" };
var dictionary = new Dictionary<string, int>
{
    ["a"] = 1,
    ["b"] = 2,
    ["c"] = 3
};

var (values, missingCount) = keys.PartitionWith(key =>
    dictionary.TryGetValue(key, out var value)
        ? Option<int>.From(value)
        : Option<int>.None);

// values = [1, 2, 3]
// missingCount = 2
```

### Async PartitionWith

```csharp
// Task variant
var userIds = new[] { 1, 2, 3, 4, 5 };

var (validUsers, errors) = await userIds.PartitionWithAsync(async id =>
{
    var user = await FetchUserAsync(id);
    return user != null
        ? Result.Success<User, string>(user)
        : Result.Failure<User, string>($"User {id} not found");
});
```

```csharp
// ValueTask variant
var strings = new[] { "1", "2", "invalid", "3" };

var (numbers, invalidCount) = await strings.PartitionWithAsync(async s =>
{
    await ValidateAsync(s);
    return int.TryParse(s, out var n)
        ? Option<int>.From(n)
        : Option<int>.None;
});
```

---

## Helper Methods

### ChooseSuccesses

Extract only successful values (lazy evaluation):

```csharp
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Failure<int, string>("error"),
    Result.Success<int, string>(2),
    Result.Failure<int, string>("error2"),
    Result.Success<int, string>(3)
};

IEnumerable<int> successes = results.ChooseSuccesses();
// Lazy evaluation - only enumerates when needed
var list = successes.ToList(); // [1, 2, 3]
```

Benefits over Partition:
- ✅ Lazy evaluation (doesn't materialize immediately)
- ✅ More efficient if you only need successes
- ✅ Can be used in LINQ chains

```csharp
var sum = results
    .ChooseSuccesses()
    .Where(n => n > 5)
    .Sum();
```

### ChooseFailures

Extract only failures (lazy evaluation):

```csharp
var results = GetResults();

IEnumerable<string> errors = results.ChooseFailures();
var errorList = errors.ToList();
```

### ChooseValues

Extract only values from Options (lazy evaluation):

```csharp
var options = new[]
{
    Option<int>.From(1),
    Option<int>.None,
    Option<int>.From(2),
    Option<int>.None,
    Option<int>.From(3)
};

IEnumerable<int> values = options.ChooseValues();
var list = values.ToList(); // [1, 2, 3]
```

Equivalent to existing `Choose()` method but more explicit name.

---

## Real-World Examples

### Example 1: Form Validation

```csharp
public class FormValidator
{
    public (List<ValidatedField> valid, List<string> errors) ValidateForm(FormData form)
    {
        var validations = new[]
        {
            ValidateEmail(form.Email),
            ValidatePassword(form.Password),
            ValidateAge(form.Age),
            ValidatePhone(form.Phone)
        };

        var (valid, errors) = validations.Partition();
        return (valid.ToList(), errors.ToList());
    }

    private Result<ValidatedField, string> ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<ValidatedField, string>("Email is required");

        if (!email.Contains("@"))
            return Result.Failure<ValidatedField, string>("Invalid email format");

        return Result.Success<ValidatedField, string>(
            new ValidatedField("Email", email));
    }

    // Similar for other fields...
}
```

### Example 2: Batch Processing with Statistics

```csharp
public async Task<BatchResult> ProcessBatch(IEnumerable<OrderDto> orders)
{
    var results = await orders.PartitionWithAsync(async order =>
    {
        try
        {
            var processed = await ProcessOrderAsync(order);
            return Result.Success<Order, string>(processed);
        }
        catch (Exception ex)
        {
            return Result.Failure<Order, string>(
                $"Order {order.Id}: {ex.Message}");
        }
    });

    var partition = results.ToList().PartitionToLists();

    return new BatchResult
    {
        SuccessCount = partition.SuccessCount,
        FailureCount = partition.FailureCount,
        TotalCount = partition.TotalCount,
        ProcessedOrders = partition.Successes.ToList(),
        Errors = partition.Failures.ToList()
    };
}
```

### Example 3: Data Import with Validation

```csharp
public ImportResult ImportUsers(IEnumerable<string> csvLines)
{
    var (validUsers, errors) = csvLines
        .Skip(1) // Skip header
        .PartitionWith(line =>
        {
            var parts = line.Split(',');

            if (parts.Length != 3)
                return Result.Failure<User, string>($"Invalid line: {line}");

            if (!int.TryParse(parts[0], out var id))
                return Result.Failure<User, string>($"Invalid ID: {parts[0]}");

            return Result.Success<User, string>(new User
            {
                Id = id,
                Name = parts[1],
                Email = parts[2]
            });
        });

    return new ImportResult
    {
        ImportedUsers = validUsers.ToList(),
        FailedLines = errors.ToList()
    };
}
```

### Example 4: Optional Field Extraction

```csharp
public class DataExtractor
{
    public ExtractedData ExtractFromJson(JObject json)
    {
        var fields = new[] { "id", "name", "email", "age" };

        var (values, missingCount) = fields.PartitionWith(field =>
        {
            var token = json[field];
            return token != null
                ? Option<string>.From(token.ToString())
                : Option<string>.None;
        });

        if (missingCount > 0)
        {
            _logger.LogWarning($"{missingCount} required fields missing");
        }

        var valueList = values.ToList();
        return new ExtractedData
        {
            Id = int.Parse(valueList[0]),
            Name = valueList[1],
            Email = valueList[2],
            Age = int.Parse(valueList[3])
        };
    }
}
```

### Example 5: Parallel Processing with Results

```csharp
public async Task<ProcessingSummary> ProcessFiles(IEnumerable<string> filePaths)
{
    var results = await Task.WhenAll(
        filePaths.Select(async path =>
        {
            try
            {
                var content = await File.ReadAllTextAsync(path);
                var processed = ProcessContent(content);
                return Result.Success<ProcessedFile, string>(processed);
            }
            catch (Exception ex)
            {
                return Result.Failure<ProcessedFile, string>(
                    $"{path}: {ex.Message}");
            }
        }));

    var partition = results.PartitionToLists();

    return new ProcessingSummary
    {
        SuccessfulFiles = partition.Successes.ToList(),
        FailedFiles = partition.Failures.ToList(),
        SuccessRate = (double)partition.SuccessCount / partition.TotalCount
    };
}
```

### Example 6: Stream Processing

```csharp
public void ProcessLargeDataset(IEnumerable<DataItem> items)
{
    // Only process successes, log failures
    foreach (var validItem in items
        .Select(ValidateItem)
        .ChooseSuccesses())
    {
        ProcessValidItem(validItem);
    }

    // Or collect errors for reporting
    var errors = items
        .Select(ValidateItem)
        .ChooseFailures()
        .ToList();

    if (errors.Any())
    {
        ReportErrors(errors);
    }
}
```

---

## Best Practices

### 1. Choose the Right Return Format

```csharp
// Use tuple for simple cases
var (successes, failures) = results.Partition();

// Use object for rich information
var partition = results.PartitionToLists();
if (partition.AllSucceeded)
{
    ProcessAllSuccesses(partition.Successes);
}

// Use helpers for lazy evaluation
var successes = results.ChooseSuccesses(); // Deferred execution
```

### 2. Use PartitionWith for Transformation

```csharp
// ✅ Good - Transform and partition in one pass
var (valid, invalid) = inputs.PartitionWith(ValidateAndTransform);

// ❌ Less efficient - Two passes
var results = inputs.Select(ValidateAndTransform);
var (valid, invalid) = results.Partition();
```

### 3. Lazy Evaluation with Choose Methods

```csharp
// ✅ Good - Only enumerate once
var processedSuccesses = results
    .ChooseSuccesses()
    .Select(Transform)
    .Where(Filter)
    .ToList();

// ❌ Less efficient - Materializes unnecessarily
var (successes, _) = results.Partition();
var processed = successes
    .Select(Transform)
    .Where(Filter)
    .ToList();
```

### 4. Handle Empty Collections

```csharp
var (successes, failures) = results.Partition();

if (!successes.Any() && !failures.Any())
{
    Console.WriteLine("No results to process");
    return;
}

// Or use PartitionToLists
var partition = results.PartitionToLists();
if (partition.TotalCount == 0)
{
    Console.WriteLine("No results to process");
    return;
}
```

### 5. Combine with Other Operations

```csharp
// Chain with other functional operations
return GetResults()
    .PartitionWith(Validate)
    .Then((successes, failures) => new
    {
        ProcessedData = successes.Select(Transform).ToList(),
        ErrorReport = GenerateErrorReport(failures)
    });
```

### 6. Async Processing Patterns

```csharp
// Sequential processing
var (successes, failures) = await items.PartitionWithAsync(ValidateAsync);

// Parallel processing
var tasks = items.Select(ValidateAsync);
var results = await Task.WhenAll(tasks);
var (successes, failures) = results.Partition();
```

---

## Performance Considerations

### Time Complexity
- **Partition**: O(n) - Single pass through collection
- **PartitionWith**: O(n) - Single pass with selector invocation
- **Choose methods**: O(n) - Lazy enumeration

### Space Complexity
- **Partition**: O(n) - Materializes both collections
- **PartitionToLists**: O(n) - Materializes both collections as lists
- **Choose methods**: O(1) - Deferred execution, no immediate allocation

### When to Use Each

```csharp
// Need both successes and failures immediately
var (successes, failures) = results.Partition();
ProcessSuccesses(successes);
LogFailures(failures);

// Only need successes, lazy evaluation
var processedItems = results
    .ChooseSuccesses()
    .Select(Transform)
    .ToList();

// Need statistics about results
var partition = results.PartitionToLists();
Console.WriteLine($"Success rate: {partition.SuccessCount}/{partition.TotalCount}");
```

---

## Comparison with Alternatives

### vs Manual Filtering

```csharp
// ❌ Manual - Verbose, two passes
var successes = results.Where(r => r.IsSuccess).Select(r => r.Value).ToList();
var failures = results.Where(r => r.IsFailure).Select(r => r.Error).ToList();

// ✅ Partition - Concise, single pass
var (successes, failures) = results.Partition();
```

### vs LINQ GroupBy

```csharp
// ❌ GroupBy - Awkward for binary split
var groups = results.GroupBy(r => r.IsSuccess);
var successes = groups.First(g => g.Key).Select(r => r.Value);
var failures = groups.First(g => !g.Key).Select(r => r.Error);

// ✅ Partition - Natural for binary split
var (successes, failures) = results.Partition();
```

---

## Summary

**Partition** operations provide efficient, type-safe ways to split collections of Results and Options:

- ✅ **Single pass** through collection
- ✅ **Multiple formats** (tuple, object, helpers)
- ✅ **Lazy evaluation** available with Choose methods
- ✅ **Full async support** with Task and ValueTask
- ✅ **Flexible** - Partition existing or with selectors
- ✅ **Rich API** - Statistics and convenience properties

Use Partition whenever you need to separate successes from failures or values from nones in functional-style code.

