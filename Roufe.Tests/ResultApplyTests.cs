using System;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class ResultApplyTests
{
    #region Basic Apply Tests

    [Fact]
    public void Apply_BothSuccess_AppliesFunctionToValue()
    {
        // Arrange
        var funcResult = Result.Success<Func<int, string>, string>(x => x.ToString());
        var valueResult = Result.Success<int, string>(42);

        // Act
        var applied = valueResult.Apply(funcResult);

        // Assert
        Assert.True(applied.IsSuccess);
        Assert.Equal("42", applied.Value);
    }

    [Fact]
    public void Apply_FunctionFailure_ReturnsFailure()
    {
        // Arrange
        var funcResult = Result.Failure<Func<int, string>, string>("function error");
        var valueResult = Result.Success<int, string>(42);

        // Act
        var applied = valueResult.Apply(funcResult);

        // Assert
        Assert.True(applied.IsFailure);
        Assert.Equal("function error", applied.Error);
    }

    [Fact]
    public void Apply_ValueFailure_ReturnsFailure()
    {
        // Arrange
        var funcResult = Result.Success<Func<int, string>, string>(x => x.ToString());
        var valueResult = Result.Failure<int, string>("value error");

        // Act
        var applied = valueResult.Apply(funcResult);

        // Assert
        Assert.True(applied.IsFailure);
        Assert.Equal("value error", applied.Error);
    }

    [Fact]
    public void Apply_BothFailure_ReturnsFirstFailure()
    {
        // Arrange
        var funcResult = Result.Failure<Func<int, string>, string>("function error");
        var valueResult = Result.Failure<int, string>("value error");

        // Act
        var applied = valueResult.Apply(funcResult);

        // Assert
        Assert.True(applied.IsFailure);
        Assert.Equal("function error", applied.Error); // Function failure takes precedence
    }

    [Fact]
    public void Apply_ReversedCall_WorksCorrectly()
    {
        // Arrange
        var funcResult = Result.Success<Func<int, string>, string>(x => $"Value: {x}");
        var valueResult = Result.Success<int, string>(42);

        // Act
        var applied = funcResult.Apply(valueResult);

        // Assert
        Assert.True(applied.IsSuccess);
        Assert.Equal("Value: 42", applied.Value);
    }

    #endregion

    #region Pure Tests

    [Fact]
    public void Pure_CreatesSuccessResultWithFunction()
    {
        // Arrange
        Func<int, string> func = x => x.ToString();

        // Act
        var result = ResultExtensions.Pure<int, string, string>(func);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("42", result.Value(42));
    }

    #endregion

    #region Multi-Parameter Apply Tests

    [Fact]
    public void Apply_TwoParameters_BothSuccess_CombinesValues()
    {
        // Arrange
        var result1 = Result.Success<int, string>(10);
        var result2 = Result.Success<int, string>(32);

        // Act
        var combined = result1.Apply(result2, (a, b) => a + b);

        // Assert
        Assert.True(combined.IsSuccess);
        Assert.Equal(42, combined.Value);
    }

    [Fact]
    public void Apply_TwoParameters_FirstFailure_ReturnsFirstFailure()
    {
        // Arrange
        var result1 = Result.Failure<int, string>("error1");
        var result2 = Result.Success<int, string>(32);

        // Act
        var combined = result1.Apply(result2, (a, b) => a + b);

        // Assert
        Assert.True(combined.IsFailure);
        Assert.Equal("error1", combined.Error);
    }

    [Fact]
    public void Apply_TwoParameters_SecondFailure_ReturnsSecondFailure()
    {
        // Arrange
        var result1 = Result.Success<int, string>(10);
        var result2 = Result.Failure<int, string>("error2");

        // Act
        var combined = result1.Apply(result2, (a, b) => a + b);

        // Assert
        Assert.True(combined.IsFailure);
        Assert.Equal("error2", combined.Error);
    }

    [Fact]
    public void Apply_ThreeParameters_AllSuccess_CombinesValues()
    {
        // Arrange
        var result1 = Result.Success<int, string>(10);
        var result2 = Result.Success<int, string>(20);
        var result3 = Result.Success<int, string>(12);

        // Act
        var combined = result1.Apply(result2, result3, (a, b, c) => a + b + c);

        // Assert
        Assert.True(combined.IsSuccess);
        Assert.Equal(42, combined.Value);
    }

    [Fact]
    public void Apply_FourParameters_AllSuccess_CombinesValues()
    {
        // Arrange
        var result1 = Result.Success<int, string>(10);
        var result2 = Result.Success<int, string>(10);
        var result3 = Result.Success<int, string>(10);
        var result4 = Result.Success<int, string>(12);

        // Act
        var combined = result1.Apply(result2, result3, result4, (a, b, c, d) => a + b + c + d);

        // Assert
        Assert.True(combined.IsSuccess);
        Assert.Equal(42, combined.Value);
    }

    #endregion

    #region Task Apply Tests

    [Fact]
    public async Task Apply_TaskResults_BothSuccess_AppliesFunction()
    {
        // Arrange
        var funcTask = Task.FromResult(Result.Success<Func<int, string>, string>(x => x.ToString()));
        var valueTask = Task.FromResult(Result.Success<int, string>(42));

        // Act
        var applied = await valueTask.Apply(funcTask);

        // Assert
        Assert.True(applied.IsSuccess);
        Assert.Equal("42", applied.Value);
    }

    [Fact]
    public async Task Apply_TaskFunc_SyncValue_Works()
    {
        // Arrange
        var funcTask = Task.FromResult(Result.Success<Func<int, string>, string>(x => x.ToString()));
        var value = Result.Success<int, string>(42);

        // Act
        var applied = await value.Apply(funcTask);

        // Assert
        Assert.True(applied.IsSuccess);
        Assert.Equal("42", applied.Value);
    }

    [Fact]
    public async Task Apply_TwoTaskParameters_AllSuccess_CombinesValues()
    {
        // Arrange
        var task1 = Task.FromResult(Result.Success<int, string>(10));
        var task2 = Task.FromResult(Result.Success<int, string>(32));

        // Act
        var combined = await task1.Apply(task2, (a, b) => a + b);

        // Assert
        Assert.True(combined.IsSuccess);
        Assert.Equal(42, combined.Value);
    }

    #endregion

    #region ValueTask Apply Tests

    [Fact]
    public async Task Apply_ValueTaskResults_BothSuccess_AppliesFunction()
    {
        // Arrange
        var funcTask = ValueTask.FromResult(Result.Success<Func<int, string>, string>(x => x.ToString()));
        var valueTask = ValueTask.FromResult(Result.Success<int, string>(42));

        // Act
        var applied = await valueTask.Apply(funcTask);

        // Assert
        Assert.True(applied.IsSuccess);
        Assert.Equal("42", applied.Value);
    }

    [Fact]
    public async Task Apply_TwoValueTaskParameters_AllSuccess_CombinesValues()
    {
        // Arrange
        var task1 = ValueTask.FromResult(Result.Success<int, string>(10));
        var task2 = ValueTask.FromResult(Result.Success<int, string>(32));

        // Act
        var combined = await task1.Apply(task2, (a, b) => a + b);

        // Assert
        Assert.True(combined.IsSuccess);
        Assert.Equal(42, combined.Value);
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_ValidationOfMultipleInputs()
    {
        // Arrange
        var nameResult = ValidateName("Alice");
        var ageResult = ValidateAge(25);
        var emailResult = ValidateEmail("alice@example.com");

        // Act - Combine all validations
        var userResult = nameResult.Apply(ageResult, emailResult,
            (name, age, email) => new User { Name = name, Age = age, Email = email });

        // Assert
        Assert.True(userResult.IsSuccess);
        Assert.Equal("Alice", userResult.Value.Name);
        Assert.Equal(25, userResult.Value.Age);
        Assert.Equal("alice@example.com", userResult.Value.Email);
    }

    [Fact]
    public void Scenario_ValidationOfMultipleInputs_OneFails()
    {
        // Arrange
        var nameResult = ValidateName("Alice");
        var ageResult = ValidateAge(-5); // Invalid age
        var emailResult = ValidateEmail("alice@example.com");

        // Act
        var userResult = nameResult.Apply(ageResult, emailResult,
            (name, age, email) => new User { Name = name, Age = age, Email = email });

        // Assert
        Assert.True(userResult.IsFailure);
        Assert.Contains("Age", userResult.Error);
    }

    [Fact]
    public void Scenario_IndependentComputations_AllSucceed()
    {
        // Arrange - Three independent API calls
        var weatherResult = Result.Success<string, string>("Sunny");
        var temperatureResult = Result.Success<int, string>(72);
        var humidityResult = Result.Success<int, string>(65);

        // Act - Combine into weather report
        var reportResult = weatherResult.Apply(temperatureResult, humidityResult,
            (weather, temp, humidity) => $"{weather}, {temp}°F, {humidity}% humidity");

        // Assert
        Assert.True(reportResult.IsSuccess);
        Assert.Equal("Sunny, 72°F, 65% humidity", reportResult.Value);
    }

    [Fact]
    public async Task Scenario_ParallelValidation_Async()
    {
        // Arrange - Simulate async validations
        var emailTask = ValidateEmailAsync("test@example.com");
        var usernameTask = ValidateUsernameAsync("testuser");

        // Act - Validate both in parallel
        var result = await emailTask.Apply(usernameTask,
            (email, username) => new { Email = email, Username = username });

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("test@example.com", result.Value.Email);
        Assert.Equal("testuser", result.Value.Username);
    }

    [Fact]
    public void Scenario_ApplyWithPure_LiftFunctionIntoResult()
    {
        // Arrange
        var value = Result.Success<int, string>(42);
        var func = ResultExtensions.Pure<int, string, string>(x => $"The answer is {x}");

        // Act
        var result = value.Apply(func);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("The answer is 42", result.Value);
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void Apply_NullFuncResult_ThrowsArgumentNullException()
    {
        // Arrange
        var value = Result.Success<int, string>(42);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            value.Apply((Result<Func<int, string>, string>)null!));
    }

    [Fact]
    public void Apply_MultiParam_NullResult2_ThrowsArgumentNullException()
    {
        // Arrange
        var result1 = Result.Success<int, string>(10);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            result1.Apply(null!, (a, b) => a + b));
    }

    [Fact]
    public void Pure_NullFunction_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            ResultExtensions.Pure<int, string, string>(null!));
    }

    #endregion

    #region Helper Methods

    private Result<string, string> ValidateName(string name)
    {
        return string.IsNullOrWhiteSpace(name)
            ? Result.Failure<string, string>("Name is required")
            : Result.Success<string, string>(name);
    }

    private Result<int, string> ValidateAge(int age)
    {
        return age < 0 || age > 120
            ? Result.Failure<int, string>("Age must be between 0 and 120")
            : Result.Success<int, string>(age);
    }

    private Result<string, string> ValidateEmail(string email)
    {
        return email.Contains("@")
            ? Result.Success<string, string>(email)
            : Result.Failure<string, string>("Invalid email format");
    }

    private Task<Result<string, string>> ValidateEmailAsync(string email)
    {
        return Task.FromResult(ValidateEmail(email));
    }

    private Task<Result<string, string>> ValidateUsernameAsync(string username)
    {
        return Task.FromResult(
            username.Length >= 3
                ? Result.Success<string, string>(username)
                : Result.Failure<string, string>("Username too short"));
    }

    private class User
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    #endregion
}

