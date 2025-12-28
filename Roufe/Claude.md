# Roufe - Functional Programming Library for C#

**AI Agent Context Document** - Last Updated: December 27, 2025

## Project Overview

**Roufe** is a comprehensive functional programming library for C# that provides robust implementations of core functional patterns including **Result**, **Option**, **Unit**, and extensive functional extensions. The library emphasizes railway-oriented programming, type safety, and async-first design.

### Name & Purpose
- **Name**: Roufe (formerly known as CSharpFunctionalExtensions in some legacy references)
- **Target Framework**: .NET 10.0 (latest version of .NET)
- **Language Version**: C# 14 (using latest features including extension blocks)
- **Nullable**: Enabled (full nullable reference type support)

### Key Characteristics
- **Zero external dependencies** - Pure .NET library
- **Performance-focused** - Aggressive inlining and optimization
- **Async-first** - Full Task and ValueTask support throughout
- **Type-safe** - Leverages strong typing and nullable annotations
- **Railway-oriented** - Emphasizes error handling via Result type

---

## Project Structure

```
Roufe/
├── Claude.md                          ← This file (AI agent context)
├── Configuration.cs                   ← Global configuration (ConfigureAwait, error separators)
├── GlobalUsings.cs                    ← Global using static Configuration
├── Unit.cs                            ← Unit type (functional void/null replacement)
├── ICombine.cs                        ← Interface for combinable types
│
├── Option/                            ← Option<T> monad (Maybe/Optional)
│   ├── Option.cs                      ← Core Option<T> implementation
│   ├── Option.Configuration.cs        ← Option-specific configuration
│   ├── OptionEqualityComparer.cs      ← Equality comparison support
│   └── Extensions/                    ← 47 extension method files
│       ├── Bind.cs, Bind.Task.cs, Bind.ValueTask.cs
│       ├── Map.cs, Map.Task.cs, Map.ValueTask.cs
│       ├── Match.cs, Match.Task.cs, Match.ValueTask.cs
│       ├── Execute.cs, Execute.Task.cs, Execute.ValueTask.cs
│       ├── AsOption.cs, AsNullable.cs, ToResult.cs
│       ├── Choose.cs, Flatten.cs, Where.cs
│       ├── TryFind.cs, TryFirst.cs, TryLast.cs
│       └── ... (30+ more extension files)
│
├── Result/                            ← Result<T,TE> monad (Either with success/failure semantics)
│   ├── IResult.cs                     ← Result interfaces
│   ├── ResultTE.cs                    ← Core Result<T,TE> implementation
│   ├── NullableExtensions.cs          ← Nullable integration
│   ├── Exceptions/                    ← Custom exceptions
│   │   ├── ResultFailureException.cs
│   │   └── ResultSuccessException.cs
│   ├── Internal/                      ← Internal utilities
│   │   └── ResultCommonLogic.cs
│   └── Methods/                       ← Result creation and extensions
│       ├── Success.cs, Failure.cs     ← Factory methods
│       ├── SuccessIf.cs, FailureIf.cs ← Conditional creation
│       ├── Try.cs                     ← Exception handling
│       ├── Combine.cs                 ← Multiple result combination
│       └── Extensions/                ← 100+ extension method files
│           ├── Bind.cs, Map.cs, MapError.cs
│           ├── Ensure.cs, EnsureNot.cs, Check.cs
│           ├── Tap.cs, TapError.cs, TapIf.cs
│           ├── Match.cs, Compensate.cs, Finally.cs
│           ├── BindTry.cs, MapTry.cs, TapTry.cs
│           ├── BindOptional.cs, AsOption.cs
│           ├── WithTransactionScope.cs
│           └── ... (each with .Task.cs and .ValueTask.cs variants)
│
├── FunctionalExtensions/              ← Generic functional utilities
│   ├── EnumerableExtensions.cs        ← IEnumerable<T> functional methods
│   ├── EnumerableExtensions.Task.cs   ← Async Task variants
│   ├── EnumerableExtensions.ValueTask.cs ← Async ValueTask variants
│   ├── FunctionExtensions.cs          ← Function composition, currying, etc.
│   ├── FunctionExtensions.Examples.md ← Usage examples
│   ├── FunctionExtensions.QuickReference.md
│   ├── GenericExtensions.cs           ← Generic T extensions (Pipe, IsIn)
│   ├── TaskExtensions.cs              ← Task/ValueTask utilities
│   └── NumberExtensions.cs            ← Numeric comparison extensions
│
└── Properties/
    └── AssemblyInfo.cs                ← Assembly configuration
```

---

## Core Types

### 1. Result<T, TE>

**Purpose**: Represents the result of an operation that can succeed with value `T` or fail with error `TE`.

**Key Properties**:
- `bool IsSuccess` / `bool IsFailure`
- `T Value` - Throws if failure
- `TE Error` - Throws if success
- `T GetValueOrDefault(T defaultValue)`

**Creation**:
```csharp
Result.Success<T, TE>(value)
Result.Failure<T, TE>(error)
Result.SuccessIf<T, TE>(condition, value, error)
Result.Try(() => dangerousOperation, exceptionHandler)
```

**Railway-Oriented Methods**:
- **Bind** - Chains Result-returning operations (flatMap)
- **Map** - Transforms success value
- **MapError** - Transforms error value
- **Ensure** - Adds validation, converts success to failure if predicate fails
- **Check** - Runs validation without changing result
- **Tap** - Side effects on success
- **TapError** - Side effects on failure
- **Match** - Pattern match on success/failure
- **Compensate** - Recover from failure
- **Finally** - Execute action regardless of state

**File Organization**:
- Core: `Result/ResultTE.cs`
- Factory methods: `Result/Methods/*.cs`
- Extensions: `Result/Methods/Extensions/*.cs` with `.Task.cs` and `.ValueTask.cs` variants

### 2. Option<T>

**Purpose**: Represents an optional value (Some or None), safer alternative to null.

**Key Properties**:
- `bool HasValue` / `bool HasNoValue`
- `T Value` - Throws if no value
- `T GetValueOrThrow(string? message = null)`
- `T GetValueOrDefault(T defaultValue)`
- `bool TryGetValue(out T value)`

**Creation**:
```csharp
Option<T>.From(value)
Option<T>.None
(T)value // implicit conversion
```

**Key Methods**:
- **Bind** - Chains Option-returning operations
- **Map** - Transforms inner value if present
- **Match** - Pattern match on Some/None
- **Execute** - Side effects if value present
- **Or** - Provides alternative Option if None
- **Where** - Filters to None if predicate fails
- **Choose** - Filters collection of Options
- **ToResult** - Converts to Result<T, TE>

**File Organization**:
- Core: `Option/Option.cs`
- Extensions: `Option/Extensions/*.cs` with `.Task.cs` and `.ValueTask.cs` variants

### 3. Unit

**Purpose**: Represents "no meaningful value" (functional programming's void/null replacement).

**Usage**: Return type for operations with side effects but no return value.

```csharp
public readonly struct Unit : IEquatable<Unit>
{
    public static readonly Unit Value = default;
    // Equals always returns true
    // ToString returns "()"
}
```

**Common Patterns**:
- `Result<Unit, TE>` - Operation that can fail but returns no value on success
- `Func<T, Unit>` - Function that performs side effects
- `Action<T>.ToFunc()` - Converts Action to Func<T, Unit>

---

## C# 14 Extension Block Syntax

**Critical**: This project uses C# 14's **extension block** syntax (not traditional extension methods).

### Syntax Pattern:
```csharp
public static partial class OptionExtensions
{
    extension<T>(in Option<T> option)  // ← Extension block declaration
    {
        public Option<TK> Bind<TK>(Func<T, Option<TK>> selector)
            => option.HasNoValue ? Option<TK>.None : selector(option.Value);
    }
}
```

### Key Differences from Traditional Extensions:
- Uses `extension<TParams>(Type target)` instead of `public static Method(this Type target)`
- Methods inside block are scoped to the extension target
- Can access target via the parameter name (e.g., `option`, `result`)
- Multiple type parameters in extension declaration (e.g., `extension<T, TK, TE>`)
- Often use `in` modifier for readonly structs to avoid copies

### File Naming Convention:
- Base method: `Bind.cs`
- Task variant: `Bind.Task.cs` (depends on base)
- ValueTask variant: `Bind.ValueTask.cs` (depends on base)

---

## Async Patterns

### Three Variants for Most Operations:

1. **Synchronous** (`Method.cs`):
   ```csharp
   Result<TK, TE> Map<TK>(Func<T, TK> func)
   ```

2. **Task Variant** (`Method.Task.cs`):
   ```csharp
   Task<Result<TK, TE>> Map<TK>(Func<T, Task<TK>> func)
   Task<Result<TK, TE>> Map<TK>(this Task<Result<T, TE>> task, Func<T, TK> func)
   ```

3. **ValueTask Variant** (`Method.ValueTask.cs`):
   ```csharp
   ValueTask<Result<TK, TE>> Map<TK>(Func<T, ValueTask<TK>> func)
   ValueTask<Result<TK, TE>> Map<TK>(this ValueTask<Result<T, TE>> task, Func<T, TK> func)
   ```

### ConfigureAwait Pattern:
All async methods use `ConfigureAwait(DefaultConfigureAwait)` where `DefaultConfigureAwait` is set in `Configuration.cs` (currently `false`).

---

## Functional Extensions

### EnumerableExtensions (IEnumerable<T>)

**Methods**:
- `Map<TK>(Func<T, TK>)` - Alias for Select
- `Filter(Func<T, bool>)` - Alias for Where
- `Fold<TK>(Func<TK, T, TK>, TK seed)` - Alias for Aggregate
- `Tap/ForEach/Iter(Action<T>)` - Side effects, returns original collection
- `ToSafeArray()` - Converts to array, avoids multiple enumeration

**Async Variants**: `MapAsync`, `FilterAsync`, `FoldAsync`, `TapAsync` for Task and ValueTask

### FunctionExtensions (NEW - Dec 2025)

**Function Composition** (29 methods):
- `Compose<T,TK,TR>` - Math composition f(g(x))
- `AndThen<T,TK,TR>` - Forward chaining g(f(x))
- `Curry` / `Uncurry` - Convert multi-param ↔ nested single-param
- `Partial` - Fix some arguments, create specialized functions
- `Flip` - Reverse argument order
- `Identity<T>` / `Constant<T>` - Basic combinators
- `Apply<T,TR>` - Reverse function application for fluent chains
- `Negate` / `And` / `Or` / `Xor` - Predicate combinators
- `Action.ToFunc()` - Convert Action to Func<Unit>

### MemoizationExtensions (NEW - Dec 2025)

**Function Result Caching** (13+ methods):
- `Memoize<T,TResult>` - Cache function results indefinitely (1-4 params)
- `Memoize<T,TResult>(IEqualityComparer)` - With custom comparer
- `MemoizeWithTimeout` - Time-based cache expiration
- `MemoizeWithCapacity` - LRU-like eviction with max size
- `MemoizeAsync` - Async memoization for Task/ValueTask
- `MemoizeAsyncWithTimeout` - Async with expiration
- `MemoizeWithCache` - Returns cache control interface
- **Thread-safe** using ConcurrentDictionary
- **Cache control** - Clear, Remove, Contains, TryGetValue operations

### TraverseSequenceExtensions (NEW - Dec 2025)

**Collection → Monad Transformations** (16+ methods):
- `Sequence<T, TE>` - Flip IEnumerable&lt;Result&lt;T, TE&gt;&gt; to Result&lt;IEnumerable&lt;T&gt;, TE&gt;
- `Sequence<T>` - Flip IEnumerable&lt;Option&lt;T&gt;&gt; to Option&lt;IEnumerable&lt;T&gt;&gt;
- `Traverse<T, TR, TE>` - Map collection through Result-returning function
- `Traverse<T, TR>` - Map collection through Option-returning function
- `TraverseAsync` - Async variants for Task and ValueTask
- `TraverseParallel` - Parallel execution with order preservation
- **Short-circuits** on first failure/None
- **Full async support** with ConfigureAwait
- **Railway-oriented** - Perfect for validation pipelines

### PartitionExtensions (NEW - Dec 2025)

**Split Result/Option Collections** (14+ methods):
- `Partition<T, TE>` - Split IEnumerable&lt;Result&lt;T, TE&gt;&gt; into (successes, failures)
- `Partition<T>` - Split IEnumerable&lt;Option&lt;T&gt;&gt; into (values, noneCount)
- `PartitionToLists` - Returns rich objects with statistics
- `PartitionWith` - Partition with selector function
- `PartitionWithAsync` - Async variants for Task and ValueTask
- `ChooseSuccesses` / `ChooseFailures` - Lazy extraction helpers
- `ChooseValues` - Lazy extraction for Options
- **Single pass** through collection
- **Rich API** - Statistics and convenience properties
- **Lazy helpers** - Deferred execution for performance

### ScanExtensions (NEW - Dec 2025)

**Fold with Intermediate Results** (15+ methods):
- `Scan<T, TAccumulator>` - Returns all intermediate fold states
- `Scan<T>` - Without seed (uses first element)
- `ScanRight` - Right-associative scan
- `ScanAsync` - Async variants for Task and ValueTask
- `ScanResult` - Short-circuits on failure, returns Result
- `ScanResultAsync` - Async Result variants
- `ScanOption` - Short-circuits on None, returns Option
- `ScanOptionAsync` - Async Option variants
- **Helper methods** - CumulativeSum, CumulativeProduct, RunningMax, RunningMin
- **Lazy evaluation** - Deferred execution like LINQ
- **Progress tracking** - Shows value evolution over time

### GenericExtensions

**Methods**:
- `Pipe<TResult>(Func<T, TResult>)` - Forward pipe (same as Apply)
- `PipeAsync<TResult>(Func<T, Task<TResult>>)` - Async pipe
- `IsIn<T>(params T[])` - Check if value is in collection

### TaskExtensions

**Methods**:
- `AsCompletedTask<T>()` - Wraps value in Task.FromResult
- `AsCompletedValueTask<T>()` - Wraps value in ValueTask.FromResult

### NumberExtensions

**Methods** (for IConvertible types):
- `GreaterThan<TU>(TU other)` / `GreaterThanOrEqualTo` / `LessThanOrEqualTo`
- `IsBetween<TU, TV>(TU min, TV max, InclusionType)` - Range checking with configurable bounds

---

## Configuration

### Global Settings (Configuration.cs)

```csharp
public static class Configuration
{
    public static string ErrorMessagesSeparator = ", ";
    public static bool DefaultConfigureAwait = false;
    public static Func<Exception, string> DefaultTryErrorHandler = exc => exc.Message;
}
```

### GlobalUsings.cs

```csharp
global using static Roufe.Configuration;
```

This makes `DefaultConfigureAwait` and other config available throughout the library without qualification.

---

## Common Patterns & Idioms

### 1. Railway-Oriented Programming

Chain operations that can fail, short-circuiting on first failure:

```csharp
return input
    .Pipe(ParseUserId)
    .Bind(userId => GetUser(userId))
    .Ensure(user => user.IsActive, "User is not active")
    .Map(user => user.Email)
    .Tap(email => SendEmail(email));
```

### 2. Option to Result Conversion

```csharp
Option<User> userOption = FindUser(id);
Result<User, string> userResult = userOption.ToResult("User not found");
```

### 3. Multiple Result Combination

```csharp
var results = new[] { result1, result2, result3 };
Result<Unit, string> combined = results.Combine();  // Fails if any fail
```

### 4. Exception Handling with Try

```csharp
Result<Data, string> result = Result.Try(
    () => DangerousOperation(),
    ex => $"Operation failed: {ex.Message}"
);
```

### 5. Conditional Success/Failure

```csharp
Result<User, string> result = Result.SuccessIf(
    user.Age >= 18,
    user,
    "User must be 18 or older"
);
```

### 6. Compensate (Error Recovery)

```csharp
Result<Data, Error> result = GetDataFromPrimary()
    .Compensate(error => GetDataFromFallback());
```

### 7. Tap for Logging/Side Effects

```csharp
return result
    .Tap(value => _logger.LogInfo($"Success: {value}"))
    .TapError(error => _logger.LogError($"Failed: {error}"));
```

### 8. Memoization for Performance

```csharp
// Cache expensive computations
Func<int, int> fibonacci = null!;
fibonacci = n => n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);
var memoizedFib = fibonacci.Memoize();

// Cache database lookups with timeout
Func<int, Task<User>> getUser = id => _db.Users.FindAsync(id);
var cachedGetUser = getUser.MemoizeAsyncWithTimeout(TimeSpan.FromMinutes(5));

// Cache API calls with capacity limit
Func<string, ApiResponse> callApi = endpoint => FetchFromApi(endpoint);
var cachedApi = callApi.MemoizeWithCapacity(100);
```

### 9. Traverse & Sequence for Collections

```csharp
// Parse all strings, fail if any invalid
var strings = new[] { "1", "2", "3" };
Result<IEnumerable<int>, string> parsed = strings.Traverse(s =>
    int.TryParse(s, out var n)
        ? Result.Success<int, string>(n)
        : Result.Failure<int, string>($"Invalid: {s}"));

// Validate all users in parallel
Result<IEnumerable<User>, string> validated =
    await userIds.TraverseParallel(id => ValidateUserAsync(id));

// Sequence existing results
var results = new[] { result1, result2, result3 };
Result<IEnumerable<Data>, string> combined = results.Sequence();

// Optional field extraction
var keys = new[] { "name", "email", "age" };
Option<IEnumerable<string>> values = keys.Traverse(key =>
    dictionary.TryGetValue(key, out var val)
        ? Option<string>.From(val)
        : Option<string>.None);
```

### 10. Partition for Separating Successes and Failures

```csharp
// Separate batch processing results
var results = await ProcessBatchAsync(items);
var (successes, failures) = results.Partition();
LogSuccesses(successes);
ReportErrors(failures);

// Get rich statistics
var partition = results.PartitionToLists();
Console.WriteLine($"Success rate: {partition.SuccessCount}/{partition.TotalCount}");
if (partition.AllSucceeded)
    ProceedToNextStep(partition.Successes);

// Partition with transformation
var (valid, invalid) = inputs.PartitionWith(s =>
    int.TryParse(s, out var n)
        ? Result.Success<int, string>(n)
        : Result.Failure<int, string>($"Invalid: {s}"));

// Lazy extraction of successes only
var processedSuccesses = results
    .ChooseSuccesses()
    .Select(Transform)
    .ToList();

// Partition Options
var (values, noneCount) = options.Partition();
Console.WriteLine($"{noneCount} items were None");
```

### 11. Scan for Tracking Progression

```csharp
// Track cumulative values
var numbers = new[] { 1, 2, 3, 4, 5 };
var cumulativeSums = numbers.CumulativeSum();
// [0, 1, 3, 6, 10, 15]

// Account balance over time
var transactions = new[] { 100, -50, 200, -30 };
var balances = transactions.Scan(0, (balance, txn) => balance + txn);
// [0, 100, 50, 250, 220]

// Validation with history
var result = increments.ScanResult(0, (acc, n) =>
    acc + n <= 100
        ? Result.Success<int, string>(acc + n)
        : Result.Failure<int, string>($"Limit exceeded at {acc + n}"));

// Running statistics
var numbers = new[] { 1, 5, 3, 9, 2, 7 };
var runningMax = numbers.RunningMax();
// [1, 5, 5, 9, 9, 9]

// Factorials using cumulative product
var factorials = Enumerable.Range(1, 5).CumulativeProduct().Skip(1);
// [1, 2, 6, 24, 120]
```

---

## Code Style & Conventions

### Naming
- **Generic Type Parameters**: Start with `T` (e.g., `T`, `TK`, `TE`, `TR`, `TContext`)
  - `T` - Primary type
  - `TK` - Key or secondary type
  - `TE` - Error type
  - `TR` - Result type
  - `TU`, `TV`, `TW` - Additional types

### Variable Naming
- Use `var` wherever type is obvious from right side
- Prefer descriptive names: `result`, `option`, `value`, `error`, `func`, `predicate`

### Method Attributes
Most methods use:
```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
```

### Null Safety
- All public methods validate arguments with `ArgumentNullException.ThrowIfNull(param)`
- Nullable annotations enabled project-wide
- Use `[NotNullWhen(true)]` for TryGet patterns

### File-Scoped Namespaces
**All files use file-scoped namespaces**:
```csharp
namespace Roufe;

public class MyClass { }
```

NOT:
```csharp
namespace Roufe
{
    public class MyClass { }
}
```

### Partial Classes
- Extension classes are `partial` to allow splitting across files
- `public static partial class ResultExtensions`
- `public static partial class OptionExtensions`

---

## Testing

### Test Project
- **Location**: `Roufe.Tests/`
- **Framework**: xUnit
- **Namespace**: `Roufe.Tests`

### Test Organization
```
Roufe.Tests/
├── OptionTests/
│   ├── Extensions/
│   │   ├── BindTests.cs
│   │   ├── BindTests.Task.cs
│   │   ├── BindTests.ValueTask.cs
│   │   └── ...
│   └── Example.cs
├── ResultTests/
│   └── ... (similar structure)
├── FunctionExtensionsTests.cs
├── EnumerableExtensionsTest.cs
├── UnitTests.cs
└── TestBase.cs
```

### Test Patterns
- Use descriptive test names: `Method_Scenario_ExpectedBehavior`
- Group related tests in regions or classes
- Test both success and failure paths
- Test null argument handling
- Test async variants separately

---


## Gaps & Future Work (as of Dec 2025)

### 🟡 Not Yet Implemented (Medium Priority)
1. **Either<TLeft, TRight>** - Neutral choice type (Result is biased)
2. **Validation<T, TE>** - Non-short-circuiting error accumulation
3. **Lazy<T> helpers** - Lazy evaluation utilities
4. **Tuple extensions** - BiMap, Swap, Fold for tuples
5. **HeadOption/TailOption** - Safe head/tail for collections
6. **ZipWith** - Zip with function

### 🟢 Nice to Have
1. **Retry/Resilience** - Retry logic for Result operations
2. **String parsing extensions** - TryParseInt, TryParseEnum to Result/Option
3. **Dictionary extensions** - TryGetValue as Option
4. **Async timeout** - WithTimeout for async Result operations

---

## Integration Examples

### With ASP.NET Core
```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetUser(int id)
{
    return await GetUserById(id)
        .Map(user => new UserDto(user))
        .Match(
            success: dto => Ok(dto),
            failure: error => NotFound(error)
        );
}
```

### With EF Core
```csharp
Result<User, string> GetUser(int id)
{
    return Option<User>.From(
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id)
        )
        .ToResult($"User {id} not found");
}
```

### With LINQ
```csharp
var validUsers = users
    .Select(u => ValidateUser(u))  // Result<User, string>
    .Where(r => r.IsSuccess)
    .Select(r => r.Value);
```

---

## Performance Considerations

### Struct vs Class
- `Option<T>` - Readonly struct (value type)
- `Result<T, TE>` - Readonly struct (value type)
- `Unit` - Readonly struct (value type)

**Implications**:
- No heap allocations for the monads themselves
- Use `in` parameter modifier to avoid struct copies
- Aggressive inlining reduces overhead

### Async Performance
- Prefer `ValueTask` for hot paths that often complete synchronously
- Use `Task` for truly async operations
- ConfigureAwait(false) by default to avoid context capture

### Collection Operations
- `ToSafeArray()` caches array to prevent multiple enumeration
- `Fold`/`Map`/`Filter` are deferred (LINQ lazy evaluation)
- `Tap`/`ForEach` are eager (materialize immediately)

---

## Common Mistakes to Avoid

### 1. Don't Mix Null and Option
❌ Bad:
```csharp
Option<string> name = null;  // Don't use null!
```
✅ Good:
```csharp
Option<string> name = Option<string>.None;
```

### 2. Don't Access Value Without Checking
❌ Bad:
```csharp
var user = userOption.Value;  // Throws if None!
```
✅ Good:
```csharp
var user = userOption.GetValueOrDefault(defaultUser);
// or
var user = userOption.Match(u => u, () => defaultUser);
```

### 3. Don't Forget ConfigureAwait in Library Code
❌ Bad:
```csharp
var result = await task;
```
✅ Good:
```csharp
var result = await task.ConfigureAwait(DefaultConfigureAwait);
```

### 4. Use Bind for Chaining, Not Map
❌ Bad:
```csharp
result.Map(x => GetUser(x))  // Returns Result<Result<User, E>, E>
```
✅ Good:
```csharp
result.Bind(x => GetUser(x))  // Returns Result<User, E>
```

### 5. Don't Catch Exceptions Manually
❌ Bad:
```csharp
try {
    var data = DangerousOp();
    return Result.Success<Data, string>(data);
} catch (Exception ex) {
    return Result.Failure<Data, string>(ex.Message);
}
```
✅ Good:
```csharp
return Result.Try(() => DangerousOp(), ex => ex.Message);
```

---

## Glossary

- **Monad** - Type that wraps a value and provides Bind/Map operations
- **Functor** - Type that provides Map operation
- **Railway-Oriented Programming** - Error handling pattern using Result chaining
- **Bind** - flatMap, chains monadic operations (prevents nesting)
- **Map** - Transforms inner value without changing monad structure
- **Tap** - Side effect that returns original value (like "tee" in Unix)
- **Ensure** - Validation that converts success to failure if predicate fails
- **Compensate** - Error recovery (catch-like behavior)
- **Match** - Pattern matching on monad state
- **Applicative** - Functor that can apply wrapped functions
- **Pure Function** - Function with no side effects, same output for same input

---

## Key Decision Records

### Why Result<T, TE> instead of Either<L, R>?
- **Semantics**: Result has clear success/failure semantics vs Either's neutral left/right
- **C# Idioms**: IsSuccess/IsFailure more intuitive for C# developers
- **Railway-Oriented**: Aligns with ROP pattern (bias toward success)

### Why Struct Instead of Class?
- **Performance**: No heap allocations, better for hot paths
- **Immutability**: Readonly structs enforce immutability
- **Equality**: Value semantics more appropriate for monads

### Why Extension Blocks (C# 14)?
- **Cleaner Syntax**: More concise than traditional extensions
- **Better Scoping**: Methods scoped to the type being extended
- **Modern C#**: Leveraging latest language features

### Why Three Files Per Method (Base, Task, ValueTask)?
- **Maintainability**: Clear separation of concerns
- **Discoverability**: IDE shows related files grouped
- **Consistency**: Uniform pattern across all async operations

---

## Tips for AI Agents Working on Roufe

1. **Always use file-scoped namespaces** (`namespace Roufe;` not `namespace Roufe { }`)

2. **Use extension block syntax** for new extension methods:
   ```csharp
   extension<T>(Result<T, TE> result) { }
   ```

3. **Create three files for async methods**: `Method.cs`, `Method.Task.cs`, `Method.ValueTask.cs`

4. **Follow generic naming**: `T`, `TK`, `TE`, `TR` (not `T`, `K`, `E`, `R`)

5. **Add ConfigureAwait**: All `await` calls should use `ConfigureAwait(DefaultConfigureAwait)`

6. **Validate arguments**: Use `ArgumentNullException.ThrowIfNull(param)` at method start

7. **Use aggressive optimization**: Add `[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]`

8. **Use `in` for structs**: `extension<T>(in Option<T> option)` to avoid copies

9. **Prefer `var`** where type is obvious from right-hand side

10. **XML docs**: Add clear XML documentation with `<summary>` and `<example>` tags

11. **Test coverage**: Create corresponding test file in `Roufe.Tests/`

12. **Partial classes**: Extension classes should be `public static partial class`

---

## References & Inspiration

- **Language-Ext** (C# functional library)
- **CSharpFunctionalExtensions** (former name inspiration)
- **F#** (functional programming patterns)
- **Railway-Oriented Programming** (Scott Wlaschin)
- **Category Theory** (mathematical foundations)

---

## Recent Additions Summary

### December 2025 - Major Functional Extensions Release
The library received a significant expansion with 5 major functional extension implementations:

1. **FunctionExtensions** (29 methods)
   - Complete function composition toolkit
   - Currying and uncurrying up to 4 parameters
   - Partial application for creating specialized functions
   - Predicate combinators (And, Or, Xor, Negate)
   - Action to Func<Unit> conversions

2. **MemoizationExtensions** (13+ methods)
   - Function result caching with multiple strategies
   - Time-based expiration support
   - LRU-like capacity limiting
   - Thread-safe ConcurrentDictionary implementation
   - Full async support with Task and ValueTask
   - Manual cache control interface

3. **TraverseSequenceExtensions** (18 methods)
   - Sequence: Flip IEnumerable<Monad<T>> to Monad<IEnumerable<T>>
   - Traverse: Map + Sequence in one operation
   - Short-circuits on first failure/None
   - Parallel execution variant for performance
   - Full Result and Option support

4. **PartitionExtensions** (19 methods + 2 classes)
   - Split Result/Option collections into constituents
   - Rich result objects with statistics
   - Lazy extraction helpers (ChooseSuccesses, ChooseFailures, ChooseValues)
   - PartitionWith for conditional partitioning
   - Single-pass efficiency

5. **ScanExtensions** (19 methods)
   - Fold with all intermediate results
   - Left and right associativity (Scan, ScanRight)
   - Result and Option variants with short-circuiting
   - Helper methods: CumulativeSum, CumulativeProduct, RunningMax, RunningMin
   - Perfect for progress tracking and state evolution

**Total Addition**: ~100 new methods with comprehensive documentation and tests

---

**Document Version**: 2.0
**Last Updated**: December 27, 2025
**Major Update**: Added comprehensive functional extensions and library statistics
**Maintained By**: AI Agent (Claude) & Development Team

