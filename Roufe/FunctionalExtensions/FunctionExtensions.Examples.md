# Function Composition & Transformation Extensions - Usage Guide

This document demonstrates the usage of the function composition and transformation extensions added to Roufe.

## Table of Contents
1. [Function Composition](#function-composition)
2. [Currying](#currying)
3. [Partial Application](#partial-application)
4. [Flip](#flip)
5. [Identity and Constant](#identity-and-constant)
6. [Apply and Invoke](#apply-and-invoke)
7. [Predicate Operations](#predicate-operations)
8. [Action Composition](#action-composition)
9. [Real-World Examples](#real-world-examples)

---

## Function Composition

### Compose (f ∘ g)
Applies functions from right to left (mathematical composition).

```csharp
Func<int, int> addOne = x => x + 1;
Func<int, int> multiplyByTwo = x => x * 2;

var composed = addOne.Compose(multiplyByTwo);
var result = composed(5); // returns 11 ((5 * 2) + 1)
```

### AndThen
Applies functions from left to right (more intuitive for chaining).

```csharp
Func<int, int> addOne = x => x + 1;
Func<int, int> multiplyByTwo = x => x * 2;

var chained = addOne.AndThen(multiplyByTwo);
var result = chained(5); // returns 12 ((5 + 1) * 2)
```

### Complex Pipeline Example
```csharp
Func<string, string> trim = s => s.Trim();
Func<string, string> toLower = s => s.ToLower();
Func<string, string> removeSpaces = s => s.Replace(" ", "");

var normalize = trim
    .AndThen(toLower)
    .AndThen(removeSpaces);

var result = normalize("  Hello World  "); // returns "helloworld"
```

---

## Currying

### Two Parameters
```csharp
Func<int, int, int> add = (a, b) => a + b;
var curriedAdd = add.Curry();

var add5 = curriedAdd(5);
var result = add5(3); // returns 8
```

### Three Parameters
```csharp
Func<string, int, bool, string> format = (name, age, active) =>
    $"{name} is {age} years old and is {(active ? "active" : "inactive")}";

var curriedFormat = format.Curry();
var formatPerson = curriedFormat("John");
var formatJohnAge30 = formatPerson(30);
var result = formatJohnAge30(true); // "John is 30 years old and is active"
```

### Uncurrying
```csharp
Func<int, Func<int, int>> curriedAdd = x => y => x + y;
var uncurriedAdd = curriedAdd.Uncurry();
var result = uncurriedAdd(5, 3); // returns 8
```

### Practical Use Case: Configuration Builder
```csharp
Func<string, string, int, Configuration> buildConfig =
    (host, username, port) => new Configuration(host, username, port);

var curriedBuilder = buildConfig.Curry();
var prodBuilder = curriedBuilder("prod.example.com");
var userBuilder = prodBuilder("admin");

var devConfig = userBuilder(8080);
var stagingConfig = userBuilder(8081);
```

---

## Partial Application

### Two Parameters
```csharp
Func<int, int, int> subtract = (a, b) => a - b;
var subtract10 = subtract.Partial(10);

var result = subtract10(3); // returns 7 (10 - 3)
```

### Three Parameters - Partial One Argument
```csharp
Func<string, string, string, string> buildUrl =
    (protocol, domain, path) => $"{protocol}://{domain}/{path}";

var httpsUrl = buildUrl.Partial("https");
var result = httpsUrl("example.com", "api/users"); // "https://example.com/api/users"
```

### Three Parameters - Partial Two Arguments
```csharp
Func<string, string, string, string> buildUrl =
    (protocol, domain, path) => $"{protocol}://{domain}/{path}";

var myApiUrl = buildUrl.Partial("https", "api.example.com");
var usersEndpoint = myApiUrl("users");     // "https://api.example.com/users"
var postsEndpoint = myApiUrl("posts");     // "https://api.example.com/posts"
```

### Practical Use Case: Logger
```csharp
Func<string, string, string, string> log =
    (level, module, message) => $"[{level}] {module}: {message}";

var errorLog = log.Partial("ERROR");
var errorInAuth = errorLog.Partial("Authentication");

errorInAuth("User not found");        // "[ERROR] Authentication: User not found"
errorInAuth("Invalid credentials");   // "[ERROR] Authentication: Invalid credentials"
```

---

## Flip

### Two Parameters
```csharp
Func<int, int, int> divide = (a, b) => a / b;
var flippedDivide = divide.Flip();

divide(10, 2);         // returns 5 (10 / 2)
flippedDivide(10, 2);  // returns 0.2 (2 / 10)
```

### Practical Use Case: String Operations
```csharp
Func<string, string, string> replace = (oldValue, newValue) =>
    text => text.Replace(oldValue, newValue);

var removeSpaces = replace(" ", "");
var replaceSpacesWithDash = replace.Flip()(" ", "-");
```

### Three Parameters (Flips First Two)
```csharp
Func<string, int, bool, string> format = (name, age, active) =>
    $"{name}-{age}-{active}";

var flipped = format.Flip();
flipped(30, "John", true); // "John-30-True"
```

---

## Identity and Constant

### Identity Function
```csharp
var identity = FunctionExtensions.Identity<int>();
var result = identity(42); // returns 42

// Useful in higher-order functions
var numbers = new[] { 1, 2, 3 };
var unchanged = numbers.Select(FunctionExtensions.Identity<int>()); // [1, 2, 3]
```

### Constant Function with Input
```csharp
var alwaysTrue = FunctionExtensions.Constant<string, bool>(true);
alwaysTrue("anything"); // returns true
alwaysTrue("xyz");      // returns true

// Useful for default behaviors
var messages = new[] { "error1", "error2" };
var allCritical = messages.Select(FunctionExtensions.Constant<string, string>("CRITICAL"));
// ["CRITICAL", "CRITICAL"]
```

### Constant Function without Input
```csharp
var getDefaultTimeout = FunctionExtensions.Constant(30);
getDefaultTimeout(); // returns 30
```

---

## Apply and Invoke

### Apply (Reverse Function Application)
```csharp
Func<int, int> square = x => x * x;
var result = 5.Apply(square); // returns 25

// Fluent chaining
var result = 5
    .Apply(x => x + 1)
    .Apply(x => x * 2)
    .Apply(x => x * x); // ((5 + 1) * 2)^2 = 144
```

### Apply with Real Data
```csharp
var user = new User("John", "Doe");

var fullName = user
    .Apply(u => $"{u.FirstName} {u.LastName}")
    .Apply(name => name.ToUpper())
    .Apply(name => $"Mr. {name}");
// "Mr. JOHN DOE"
```

### Invoke
```csharp
Func<int> getNumber = () => 42;
var result = getNumber.Invoke(); // returns 42

// Useful for delayed execution
var lazyValue = () => ExpensiveComputation();
// ... later
var value = lazyValue.Invoke();
```

---

## Predicate Operations

### Negate
```csharp
Func<int, bool> isEven = x => x % 2 == 0;
var isOdd = isEven.Negate();

isOdd(5); // returns true
isOdd(4); // returns false
```

### And
```csharp
Func<int, bool> isPositive = x => x > 0;
Func<int, bool> isEven = x => x % 2 == 0;

var isPositiveAndEven = isPositive.And(isEven);

isPositiveAndEven(4);  // true
isPositiveAndEven(3);  // false
isPositiveAndEven(-4); // false
```

### Or
```csharp
Func<int, bool> isNegative = x => x < 0;
Func<int, bool> isZero = x => x == 0;

var isNonPositive = isNegative.Or(isZero);

isNonPositive(-5); // true
isNonPositive(0);  // true
isNonPositive(5);  // false
```

### Xor
```csharp
Func<int, bool> isEven = x => x % 2 == 0;
Func<int, bool> isPositive = x => x > 0;

var xorPredicate = isEven.Xor(isPositive);

xorPredicate(-2);  // true (even but not positive)
xorPredicate(3);   // true (positive but not even)
xorPredicate(4);   // false (both even and positive)
xorPredicate(-3);  // false (neither)
```

### Complex Predicate Composition
```csharp
Func<string, bool> isNotEmpty = s => !string.IsNullOrEmpty(s);
Func<string, bool> isValidEmail = s => s.Contains("@");
Func<string, bool> isGmailAddress = s => s.EndsWith("@gmail.com");

var isValidGmail = isNotEmpty
    .And(isValidEmail)
    .And(isGmailAddress);

isValidGmail("user@gmail.com");  // true
isValidGmail("user@yahoo.com");  // false
isValidGmail("");                // false
```

---

## Action Composition

### AndThen for Actions
```csharp
var log = new List<string>();

Action<string> logInfo = msg => log.Add($"INFO: {msg}");
Action<string> logToConsole = msg => Console.WriteLine(msg);

var combinedLog = logInfo.AndThen(logToConsole);
combinedLog("User logged in"); // logs to both list and console
```

### ToFunc - Convert Action to Func
```csharp
var sideEffectOccurred = false;
Action<int> performSideEffect = x => sideEffectOccurred = true;

Func<int, Unit> func = performSideEffect.ToFunc();
var result = func(42); // result is Unit.Value, sideEffectOccurred is true
```

### Parameterless Action to Func
```csharp
Action initialize = () => Console.WriteLine("Initialized");
Func<Unit> initFunc = initialize.ToFunc();

var result = initFunc(); // returns Unit.Value after executing
```

---

## Real-World Examples

### Example 1: Data Validation Pipeline
```csharp
// Define validators
Func<string, bool> isNotEmpty = s => !string.IsNullOrWhiteSpace(s);
Func<string, bool> hasMinLength = s => s.Length >= 3;
Func<string, bool> hasMaxLength = s => s.Length <= 50;
Func<string, bool> isAlphanumeric = s => s.All(char.IsLetterOrDigit);

// Compose validation
var isValidUsername = isNotEmpty
    .And(hasMinLength)
    .And(hasMaxLength)
    .And(isAlphanumeric);

// Use validator
var usernames = new[] { "john123", "ab", "user@name", "valid_user" };
var validUsernames = usernames.Where(isValidUsername);
```

### Example 2: Price Calculator with Discounts
```csharp
Func<decimal, decimal, decimal> applyDiscount = (price, discountPercent) =>
    price * (1 - discountPercent / 100);

var apply10PercentDiscount = applyDiscount.Partial(10);
var apply20PercentDiscount = applyDiscount.Flip().Partial(20);

decimal originalPrice = 100m;
var discountedPrice = originalPrice.Apply(apply10PercentDiscount); // 90
```

### Example 3: String Processing Pipeline
```csharp
Func<string, string> trim = s => s.Trim();
Func<string, string> toLower = s => s.ToLower();
Func<string, string> removeSpecialChars = s =>
    new string(s.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());
Func<string, string> collapseSpaces = s =>
    System.Text.RegularExpressions.Regex.Replace(s, @"\s+", " ");

var normalizeText = trim
    .AndThen(toLower)
    .AndThen(removeSpecialChars)
    .AndThen(collapseSpaces);

var result = normalizeText("  Hello,  World!!!  ");
// "hello world"
```

### Example 4: HTTP Request Builder
```csharp
Func<string, string, string, string, string> buildUrl =
    (protocol, host, port, path) => $"{protocol}://{host}:{port}/{path}";

var buildHttpsUrl = buildUrl.Partial("https");
var buildApiUrl = buildHttpsUrl.Partial("api.example.com");
var buildApiUrlOnPort443 = buildApiUrl.Partial("443");

var usersEndpoint = buildApiUrlOnPort443("users");
var postsEndpoint = buildApiUrlOnPort443("posts");
// "https://api.example.com:443/users"
// "https://api.example.com:443/posts"
```

### Example 5: Filtering and Transformation
```csharp
var numbers = Enumerable.Range(1, 100);

Func<int, bool> isDivisibleBy3 = x => x % 3 == 0;
Func<int, bool> isDivisibleBy5 = x => x % 5 == 0;
Func<int, bool> isFizzBuzz = isDivisibleBy3.And(isDivisibleBy5);

Func<int, string> toFizzBuzz = n =>
    isFizzBuzz(n) ? "FizzBuzz" :
    isDivisibleBy3(n) ? "Fizz" :
    isDivisibleBy5(n) ? "Buzz" :
    n.ToString();

var result = numbers.Select(toFizzBuzz);
```

### Example 6: Function Memoization Setup
```csharp
// While we don't have memoization yet, we can prepare functions for it
Func<int, int> fibonacci = null!;
fibonacci = n => n <= 1 ? n : fibonacci(n - 1) + fibonacci(n - 2);

// When memoization is available:
// var memoizedFib = fibonacci.Memoize();
```

### Example 7: Combining with Result/Option
```csharp
Func<string, Result<int, string>> parseAge = str =>
    int.TryParse(str, out var age) && age >= 0 && age <= 120
        ? Result.Success<int, string>(age)
        : Result.Failure<int, string>("Invalid age");

Func<int, Result<string, string>> formatAge = age =>
    Result.Success<string, string>($"Age: {age} years");

var processAge = parseAge.AndThen(result =>
    result.Bind(age => formatAge(age)));

var result1 = processAge("25");    // Success: "Age: 25 years"
var result2 = processAge("invalid"); // Failure: "Invalid age"
```

---

## Best Practices

1. **Use AndThen for Readability**: When building pipelines, `AndThen` is more intuitive than `Compose`.

2. **Partial Application for Configuration**: Use partial application to create configured versions of functions.

3. **Predicate Composition**: Build complex validations by composing simple predicates.

4. **Type Inference**: Let the compiler infer types when possible for cleaner code.

5. **Pure Functions**: These extensions work best with pure functions (no side effects).

6. **Combine with LINQ**: These extensions work seamlessly with LINQ for powerful data transformations.

---

## Performance Considerations

- All functions are marked with `AggressiveInlining` and `AggressiveOptimization` for best performance.
- Function composition creates small wrapper functions - the JIT compiler will optimize these away in most cases.
- For hot paths, consider using direct function calls instead of heavily composed functions.

