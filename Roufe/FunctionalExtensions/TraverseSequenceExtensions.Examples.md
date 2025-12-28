# Traverse & Sequence Extensions - Usage Guide

## Table of Contents
1. [Overview](#overview)
2. [Understanding Traverse & Sequence](#understanding-traverse--sequence)
3. [Result Operations](#result-operations)
4. [Option Operations](#option-operations)
5. [Async Variants](#async-variants)
6. [Parallel Traverse](#parallel-traverse)
7. [Real-World Examples](#real-world-examples)
8. [Common Patterns](#common-patterns)
9. [Best Practices](#best-practices)

---

## Overview

**Traverse** and **Sequence** are fundamental functional programming operations that transform collections through monadic contexts. They solve the common problem of "flipping" structures:

- **Sequence**: `IEnumerable<Monad<T>>` → `Monad<IEnumerable<T>>`
- **Traverse**: `IEnumerable<T>` + `(T → Monad<R>)` → `Monad<IEnumerable<R>>`

### Key Concepts
- ✅ **Short-circuiting**: Stops on first failure/None
- ✅ **Type safety**: Converts collections of monads to monads of collections
- ✅ **Railway-oriented**: Aligns with Result-based error handling
- ✅ **Async support**: Full Task and ValueTask variants
- ✅ **Parallel execution**: Optional parallel traverse for performance

---

## Understanding Traverse & Sequence

### The Problem They Solve

Imagine you have multiple operations that can fail, and you want to:
1. Execute all of them
2. Collect all the results
3. Return Success if all succeed, or Failure if any fail

Without Traverse/Sequence:
```csharp
❌ Verbose and error-prone
var results = new List<int>();
foreach (var str in strings)
{
    var result = TryParse(str);
    if (result.IsFailure)
        return Result.Failure<IEnumerable<int>, string>(result.Error);
    results.Add(result.Value);
}
return Result.Success<IEnumerable<int>, string>(results);
```

With Traverse:
```csharp
✅ Concise and elegant
return strings.Traverse(TryParse);
```

### Sequence vs Traverse

**Sequence**: When you already have a collection of monads
```csharp
IEnumerable<Result<int, string>> results = GetResults();
Result<IEnumerable<int>, string> sequenced = results.Sequence();
```

**Traverse**: When you need to transform and collect in one step
```csharp
IEnumerable<string> strings = GetStrings();
Result<IEnumerable<int>, string> parsed = strings.Traverse(TryParse);
```

**Relationship**: `Traverse = Map + Sequence`
```csharp
// These are equivalent:
strings.Traverse(TryParse)
strings.Select(TryParse).Sequence()
```

---

## Result Operations

### Sequence - All or Nothing

```csharp
// All successful
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Success<int, string>(2),
    Result.Success<int, string>(3)
};

var sequenced = results.Sequence();
// sequenced.IsSuccess = true
// sequenced.Value = [1, 2, 3]
```

```csharp
// One failure stops everything
var results = new[]
{
    Result.Success<int, string>(1),
    Result.Failure<int, string>("error"),
    Result.Success<int, string>(3)
};

var sequenced = results.Sequence();
// sequenced.IsFailure = true
// sequenced.Error = "error"
```

### Traverse - Transform and Collect

#### Basic Traverse
```csharp
var strings = new[] { "1", "2", "3" };

Func<string, Result<int, string>> tryParse = s =>
    int.TryParse(s, out var n)
        ? Result.Success<int, string>(n)
        : Result.Failure<int, string>($"Cannot parse '{s}'");

var result = strings.Traverse(tryParse);
// result.IsSuccess = true
// result.Value = [1, 2, 3]
```

#### With Validation
```csharp
var ages = new[] { "25", "30", "150" };

Func<string, Result<int, string>> validateAge = s =>
{
    if (!int.TryParse(s, out var age))
        return Result.Failure<int, string>($"'{s}' is not a number");

    if (age < 0 || age > 120)
        return Result.Failure<int, string>($"Age {age} is out of range");

    return Result.Success<int, string>(age);
};

var result = ages.Traverse(validateAge);
// result.IsFailure = true
// result.Error = "Age 150 is out of range"
```

### Empty Collections

```csharp
var empty = Array.Empty<Result<int, string>>();
var sequenced = empty.Sequence();
// sequenced.IsSuccess = true
// sequenced.Value = [] (empty collection)
```

---

## Option Operations

### Sequence - All or None

```csharp
// All have values
var options = new[]
{
    Option<int>.From(1),
    Option<int>.From(2),
    Option<int>.From(3)
};

var sequenced = options.Sequence();
// sequenced.HasValue = true
// sequenced.Value = [1, 2, 3]
```

```csharp
// One None stops everything
var options = new[]
{
    Option<int>.From(1),
    Option<int>.None,
    Option<int>.From(3)
};

var sequenced = options.Sequence();
// sequenced.HasNoValue = true
```

### Traverse - Transform and Collect

#### Safe Dictionary Lookup
```csharp
var keys = new[] { "a", "b", "c" };
var dictionary = new Dictionary<string, int>
{
    ["a"] = 1,
    ["b"] = 2,
    ["c"] = 3
};

Func<string, Option<int>> lookup = key =>
    dictionary.TryGetValue(key, out var value)
        ? Option<int>.From(value)
        : Option<int>.None;

var result = keys.Traverse(lookup);
// result.HasValue = true
// result.Value = [1, 2, 3]
```

#### With Missing Key
```csharp
var keys = new[] { "a", "missing", "c" };

var result = keys.Traverse(lookup);
// result.HasNoValue = true
```

---

## Async Variants

### Task-Based Traverse

```csharp
var userIds = new[] { 1, 2, 3 };

Func<int, Task<Result<User, string>>> getUser = async id =>
{
    var user = await _dbContext.Users.FindAsync(id);
    return user != null
        ? Result.Success<User, string>(user)
        : Result.Failure<User, string>($"User {id} not found");
};

var result = await userIds.TraverseAsync(getUser);
// result.IsSuccess = true if all users found
// result.Value = [user1, user2, user3]
```

### Task-Based Sequence

```csharp
var tasks = new[]
{
    FetchDataAsync(1),
    FetchDataAsync(2),
    FetchDataAsync(3)
};

var result = await tasks.Sequence();
// Waits for all tasks, returns Success if all succeed
```

### ValueTask Support

```csharp
var numbers = new[] { 1, 2, 3 };

Func<int, ValueTask<Result<int, string>>> processAsync = async n =>
{
    if (n < 100)
        return Result.Success<int, string>(n * 2); // Synchronous path

    await Task.Delay(10);
    return Result.Success<int, string>(n * 2);
};

var result = await numbers.TraverseAsync(processAsync);
```

### Task Collection with Async Function

```csharp
Task<IEnumerable<string>> stringsTask = GetStringsAsync();

var result = await stringsTask.TraverseAsync(async s =>
{
    await ValidateAsync(s);
    return Result.Success<string, string>(s.ToUpper());
});
```

---

## Parallel Traverse

For performance-critical scenarios where order preservation is needed but operations can run concurrently.

### Parallel Result Traverse

```csharp
var urls = new[]
{
    "https://api.example.com/1",
    "https://api.example.com/2",
    "https://api.example.com/3"
};

Func<string, Task<Result<ApiResponse, string>>> fetchApi = async url =>
{
    try
    {
        var response = await _httpClient.GetAsync(url);
        var data = await response.Content.ReadAsStringAsync();
        return Result.Success<ApiResponse, string>(ParseResponse(data));
    }
    catch (Exception ex)
    {
        return Result.Failure<ApiResponse, string>(ex.Message);
    }
};

// All requests execute in parallel
var result = await urls.TraverseParallel(fetchApi);
// result.IsSuccess = true if all succeed
// result.Value preserves original order
```

### Parallel Option Traverse

```csharp
var ids = Enumerable.Range(1, 100).ToArray();

Func<int, Task<Option<Data>>> fetchData = async id =>
{
    var data = await _cache.GetAsync(id);
    return data != null ? Option<Data>.From(data) : Option<Data>.None;
};

// Check all caches in parallel
var result = await ids.TraverseParallel(fetchData);
// result.HasValue = true if all IDs found in cache
```

### Performance Considerations

```csharp
// Sequential (safe for rate-limited APIs)
var sequential = await urls.TraverseAsync(fetchApi);

// Parallel (faster for independent operations)
var parallel = await urls.TraverseParallel(fetchApi);

// Parallel execution but sequential failures
// (first failure is returned, but all tasks complete)
```

---

## Real-World Examples

### Example 1: Form Validation

```csharp
public class UserRegistration
{
    public string Email { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
}

public Result<UserRegistration, string> ValidateRegistration(
    string email,
    string password,
    string ageStr)
{
    var validations = new[]
    {
        ValidateEmail(email),
        ValidatePassword(password),
        ValidateAge(ageStr)
    };

    // All validations must pass
    return validations.Sequence()
        .Map(_ => new UserRegistration
        {
            Email = email,
            Password = password,
            Age = int.Parse(ageStr)
        });
}
```

### Example 2: Batch Database Operations

```csharp
public async Task<Result<IEnumerable<Order>, string>> ProcessOrders(
    IEnumerable<OrderDto> orderDtos)
{
    Func<OrderDto, Task<Result<Order, string>>> processOrder = async dto =>
    {
        // Validate
        var validation = ValidateOrder(dto);
        if (validation.IsFailure)
            return Result.Failure<Order, string>(validation.Error);

        // Save to database
        var order = MapToOrder(dto);
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();

        return Result.Success<Order, string>(order);
    };

    // Process all orders, stop on first failure
    return await orderDtos.TraverseAsync(processOrder);
}
```

### Example 3: Configuration Loading

```csharp
public Result<AppConfig, string> LoadConfiguration(
    IEnumerable<string> configKeys)
{
    Func<string, Result<(string key, string value), string>> loadSetting = key =>
    {
        var value = _configProvider.GetValue(key);
        return value != null
            ? Result.Success<(string, string), string>((key, value))
            : Result.Failure<(string, string), string>($"Missing config: {key}");
    };

    return configKeys.Traverse(loadSetting)
        .Map(settings => new AppConfig(settings.ToDictionary(s => s.key, s => s.value)));
}
```

### Example 4: File Processing Pipeline

```csharp
public async Task<Result<IEnumerable<ProcessedFile>, string>> ProcessFiles(
    IEnumerable<string> filePaths)
{
    Func<string, Task<Result<ProcessedFile, string>>> processFile = async path =>
    {
        if (!File.Exists(path))
            return Result.Failure<ProcessedFile, string>($"File not found: {path}");

        try
        {
            var content = await File.ReadAllTextAsync(path);
            var processed = await ProcessContent(content);
            return Result.Success<ProcessedFile, string>(processed);
        }
        catch (Exception ex)
        {
            return Result.Failure<ProcessedFile, string>($"Error processing {path}: {ex.Message}");
        }
    };

    // Process files in parallel for performance
    return await filePaths.TraverseParallel(processFile);
}
```

### Example 5: API Request Batching

```csharp
public async Task<Result<IEnumerable<UserProfile>, string>> FetchUserProfiles(
    IEnumerable<int> userIds)
{
    Func<int, Task<Result<UserProfile, string>>> fetchProfile = async id =>
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/users/{id}");
            if (!response.IsSuccessStatusCode)
                return Result.Failure<UserProfile, string>($"User {id} not found");

            var profile = await response.Content.ReadFromJsonAsync<UserProfile>();
            return Result.Success<UserProfile, string>(profile!);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserProfile, string>(ex.Message);
        }
    };

    // Fetch all profiles in parallel
    return await userIds.TraverseParallel(fetchProfile);
}
```

### Example 6: Optional Field Extraction

```csharp
public Option<UserData> ExtractUserData(JObject json)
{
    var fields = new[] { "id", "name", "email" };

    Func<string, Option<string>> extractField = field =>
    {
        var token = json[field];
        return token != null
            ? Option<string>.From(token.ToString())
            : Option<string>.None;
    };

    // All fields must be present
    return fields.Traverse(extractField)
        .Map(values =>
        {
            var list = values.ToArray();
            return new UserData
            {
                Id = int.Parse(list[0]),
                Name = list[1],
                Email = list[2]
            };
        });
}
```

---

## Common Patterns

### Pattern 1: Parse and Validate

```csharp
var inputs = new[] { "10", "20", "30" };

var result = inputs.Traverse(s =>
    int.TryParse(s, out var n) && n > 0
        ? Result.Success<int, string>(n)
        : Result.Failure<int, string>($"Invalid: {s}"));
```

### Pattern 2: Map Then Sequence

```csharp
// These are equivalent:
var result1 = items.Traverse(func);
var result2 = items.Select(func).Sequence();

// Use Traverse when composing
// Use Map + Sequence when you already have the mapped collection
```

### Pattern 3: Combining with Other Operations

```csharp
return GetUserIds()
    .Filter(id => id > 0)
    .Traverse(FetchUser)
    .Map(users => users.OrderBy(u => u.Name));
```

### Pattern 4: Early Exit on Failure

```csharp
// Traverse short-circuits on first failure
var result = largeCollection.Traverse(expensiveValidation);
// Stops immediately when first validation fails
```

### Pattern 5: Async Pipeline

```csharp
return await GetItemsAsync()
    .TraverseAsync(async item => await ValidateAsync(item))
    .Bind(validItems => ProcessAsync(validItems));
```

---

## Best Practices

### 1. Choose Sequential vs Parallel Appropriately

```csharp
// ✅ Sequential for rate-limited APIs
await apiKeys.TraverseAsync(FetchFromRateLimitedApi);

// ✅ Parallel for independent operations
await imageUrls.TraverseParallel(DownloadImage);
```

### 2. Use Traverse for Transformations

```csharp
// ❌ Verbose
var results = new List<Result<int, string>>();
foreach (var s in strings)
    results.Add(TryParse(s));
return results.Sequence();

// ✅ Concise
return strings.Traverse(TryParse);
```

### 3. Combine with Railway-Oriented Programming

```csharp
return GetUserInput()
    .Traverse(ValidateField)
    .Bind(fields => CreateEntity(fields))
    .Tap(entity => LogCreation(entity))
    .Map(entity => MapToDto(entity));
```

### 4. Handle Empty Collections

```csharp
// Empty collections return Success/Some with empty collection
var empty = Array.Empty<string>();
var result = empty.Traverse(Parse);
// result.IsSuccess = true, Value = []
```

### 5. Error Messages Matter

```csharp
// ✅ Descriptive error messages
var result = items.Traverse(item =>
    Validate(item)
        ? Result.Success<Item, string>(item)
        : Result.Failure<Item, string>($"Item {item.Id} failed validation: {GetReason(item)}"));
```

### 6. Consider Memory with Large Collections

```csharp
// For very large collections, consider streaming
public async IAsyncEnumerable<Result<T, TE>> TraverseStream<T, TE>(
    IAsyncEnumerable<T> source,
    Func<T, Task<Result<T, TE>>> func)
{
    await foreach (var item in source)
    {
        yield return await func(item);
    }
}
```

---

## Performance Characteristics

### Time Complexity
- **Sequence**: O(n) - Single pass through collection
- **Traverse**: O(n) - Single pass with transformation
- **TraverseParallel**: O(n/p) where p is parallelism degree (for CPU-bound operations)

### Space Complexity
- **Sequence**: O(n) - Builds result collection
- **Traverse**: O(n) - Builds result collection
- **Early termination**: Stops building on first failure

### Short-Circuiting Behavior

```csharp
var items = Enumerable.Range(1, 1000000);

// Stops at first failure, doesn't process remaining items
var result = items.Traverse(i =>
    i == 100
        ? Result.Failure<int, string>("error")
        : Result.Success<int, string>(i));
// Only processes first 100 items
```

---

## Comparison with Alternatives

### vs Manual Loops
| Feature | Traverse | Manual Loop |
|---------|----------|-------------|
| Conciseness | ✅ One-liner | ❌ 10+ lines |
| Readability | ✅ Declarative | ❌ Imperative |
| Error-prone | ✅ Low | ❌ High |
| Early exit | ✅ Automatic | ❌ Manual |

### vs LINQ Select + Sequence
| Feature | Traverse | Select + Sequence |
|---------|----------|-------------------|
| Performance | ✅ Single pass | ⚠️ Two passes |
| Memory | ✅ Single allocation | ⚠️ Two allocations |
| Readability | ✅ Clear intent | ⚠️ Less obvious |

### vs Aggregate
```csharp
// ❌ Aggregate - complex
var result = items.Aggregate(
    Result.Success<List<int>, string>(new List<int>()),
    (acc, item) => acc.Bind(list =>
        Validate(item).Map(validated => { list.Add(validated); return list; })));

// ✅ Traverse - simple
var result = items.Traverse(Validate);
```

---

## Summary

**Traverse** and **Sequence** are essential operations for functional programming with collections of monads. They provide:

- ✅ **Concise syntax** for common patterns
- ✅ **Type-safe** transformations
- ✅ **Short-circuiting** on failure/None
- ✅ **Async support** with proper ConfigureAwait
- ✅ **Parallel execution** when needed
- ✅ **Railway-oriented** compatibility

Use them whenever you need to transform a collection through monadic operations and collect the results.

