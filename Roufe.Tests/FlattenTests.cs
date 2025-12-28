using System;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class FlattenTests
{
    #region Basic Flatten Tests

    [Fact]
    public void Flatten_SuccessOfSuccess_ReturnsInnerSuccess()
    {
        // Arrange
        var innerResult = Result.Success<int, string>(42);
        var outerResult = Result.Success<Result<int, string>, string>(innerResult);

        // Act
        var flattened = outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    [Fact]
    public void Flatten_SuccessOfFailure_ReturnsInnerFailure()
    {
        // Arrange
        var innerResult = Result.Failure<int, string>("inner error");
        var outerResult = Result.Success<Result<int, string>, string>(innerResult);

        // Act
        var flattened = outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("inner error", flattened.Error);
    }

    [Fact]
    public void Flatten_FailureOfResult_ReturnsOuterFailure()
    {
        // Arrange
        var outerResult = Result.Failure<Result<int, string>, string>("outer error");

        // Act
        var flattened = outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("outer error", flattened.Error);
    }

    [Fact]
    public void Flatten_NestedSuccess_UnwrapsOneLevel()
    {
        // Arrange
        var value = "test value";
        var innerResult = Result.Success<string, string>(value);
        var outerResult = Result.Success<Result<string, string>, string>(innerResult);

        // Act
        var flattened = outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(value, flattened.Value);
    }

    #endregion

    #region Task Flatten Tests

    [Fact]
    public async Task Flatten_TaskOfNestedResult_Success_ReturnsSuccess()
    {
        // Arrange
        var innerResult = Result.Success<int, string>(42);
        var outerResult = Result.Success<Result<int, string>, string>(innerResult);
        var taskResult = Task.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    [Fact]
    public async Task Flatten_TaskOfNestedResult_OuterFailure_ReturnsFailure()
    {
        // Arrange
        var outerResult = Result.Failure<Result<int, string>, string>("outer error");
        var taskResult = Task.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("outer error", flattened.Error);
    }

    [Fact]
    public async Task Flatten_TaskOfNestedResult_InnerFailure_ReturnsInnerFailure()
    {
        // Arrange
        var innerResult = Result.Failure<int, string>("inner error");
        var outerResult = Result.Success<Result<int, string>, string>(innerResult);
        var taskResult = Task.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("inner error", flattened.Error);
    }

    [Fact]
    public async Task Flatten_ResultContainingTask_Success_ReturnsSuccess()
    {
        // Arrange
        var innerTaskResult = Task.FromResult(Result.Success<int, string>(42));
        var outerResult = Result.Success<Task<Result<int, string>>, string>(innerTaskResult);

        // Act
        var flattened = await outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    [Fact]
    public async Task Flatten_ResultContainingTask_OuterFailure_ReturnsFailure()
    {
        // Arrange
        var outerResult = Result.Failure<Task<Result<int, string>>, string>("outer error");

        // Act
        var flattened = await outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("outer error", flattened.Error);
    }

    [Fact]
    public async Task Flatten_ResultContainingTask_InnerFailure_ReturnsInnerFailure()
    {
        // Arrange
        var innerTaskResult = Task.FromResult(Result.Failure<int, string>("inner error"));
        var outerResult = Result.Success<Task<Result<int, string>>, string>(innerTaskResult);

        // Act
        var flattened = await outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("inner error", flattened.Error);
    }

    [Fact]
    public async Task Flatten_TaskOfResultContainingTask_Success_ReturnsSuccess()
    {
        // Arrange
        var innerTaskResult = Task.FromResult(Result.Success<int, string>(42));
        var outerResult = Result.Success<Task<Result<int, string>>, string>(innerTaskResult);
        var taskResult = Task.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    #endregion

    #region ValueTask Flatten Tests

    [Fact]
    public async Task Flatten_ValueTaskOfNestedResult_Success_ReturnsSuccess()
    {
        // Arrange
        var innerResult = Result.Success<int, string>(42);
        var outerResult = Result.Success<Result<int, string>, string>(innerResult);
        var taskResult = ValueTask.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    [Fact]
    public async Task Flatten_ValueTaskOfNestedResult_OuterFailure_ReturnsFailure()
    {
        // Arrange
        var outerResult = Result.Failure<Result<int, string>, string>("outer error");
        var taskResult = ValueTask.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("outer error", flattened.Error);
    }

    [Fact]
    public async Task Flatten_ResultContainingValueTask_Success_ReturnsSuccess()
    {
        // Arrange
        var innerTaskResult = ValueTask.FromResult(Result.Success<int, string>(42));
        var outerResult = Result.Success<ValueTask<Result<int, string>>, string>(innerTaskResult);

        // Act
        var flattened = await outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    [Fact]
    public async Task Flatten_ResultContainingValueTask_OuterFailure_ReturnsFailure()
    {
        // Arrange
        var outerResult = Result.Failure<ValueTask<Result<int, string>>, string>("outer error");

        // Act
        var flattened = await outerResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("outer error", flattened.Error);
    }

    [Fact]
    public async Task Flatten_ValueTaskOfResultContainingValueTask_Success_ReturnsSuccess()
    {
        // Arrange
        var innerTaskResult = ValueTask.FromResult(Result.Success<int, string>(42));
        var outerResult = Result.Success<ValueTask<Result<int, string>>, string>(innerTaskResult);
        var taskResult = ValueTask.FromResult(outerResult);

        // Act
        var flattened = await taskResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal(42, flattened.Value);
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_MapReturningResult_ThenFlatten()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Alice" };
        var userResult = Result.Success<User, string>(user);

        // Act - Map returns a Result, creating nested Result
        var nestedResult = userResult.Map(u => ValidateUser(u));
        var flattened = nestedResult.Flatten();

        // Assert
        Assert.True(flattened.IsSuccess);
        Assert.Equal("Alice", flattened.Value.Name);
    }

    [Fact]
    public void Scenario_MapReturningResult_ValidationFails_ReturnsFailure()
    {
        // Arrange
        var user = new User { Id = 1, Name = "" }; // Invalid name
        var userResult = Result.Success<User, string>(user);

        // Act
        var nestedResult = userResult.Map(u => ValidateUser(u));
        var flattened = nestedResult.Flatten();

        // Assert
        Assert.True(flattened.IsFailure);
        Assert.Equal("Name cannot be empty", flattened.Error);
    }

    [Fact]
    public async Task Scenario_ChainedAsyncOperations_WithFlatten()
    {
        // Arrange
        var userId = 1;

        // Act
        var result = await GetUserAsync(userId)
            .ContinueWith(t => t.Result.Map(u => ValidateUserAsync(u)))
            .Unwrap()
            .ContinueWith(async t => (await t).Flatten())
            .Unwrap();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public void Scenario_BindVsFlatten_Comparison()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Bob" };
        var userResult = Result.Success<User, string>(user);

        // Using Bind (correct for chaining)
        var bindResult = userResult.Bind(u => ValidateUser(u));

        // Using Map + Flatten (equivalent but more explicit)
        var mapResult = userResult.Map(u => ValidateUser(u)).Flatten();

        // Assert - Both produce the same result
        Assert.True(bindResult.IsSuccess);
        Assert.True(mapResult.IsSuccess);
        Assert.Equal(bindResult.Value.Name, mapResult.Value.Name);
    }

    [Fact]
    public void Scenario_MultipleNestedLevels_RequiresMultipleFlatten()
    {
        // Arrange - Triple nested
        var innermost = Result.Success<int, string>(42);
        var middle = Result.Success<Result<int, string>, string>(innermost);
        var outer = Result.Success<Result<Result<int, string>, string>, string>(middle);

        // Act - Need to flatten twice
        var flattened1 = outer.Flatten(); // Result<Result<int, string>, string>
        var flattened2 = flattened1.Flatten(); // Result<int, string>

        // Assert
        Assert.True(flattened2.IsSuccess);
        Assert.Equal(42, flattened2.Value);
    }

    #endregion

    #region Helper Methods

    private Result<User, string> ValidateUser(User user)
    {
        if (string.IsNullOrEmpty(user.Name))
            return Result.Failure<User, string>("Name cannot be empty");

        return Result.Success<User, string>(user);
    }

    private Task<Result<User, string>> GetUserAsync(int id)
    {
        return Task.FromResult(Result.Success<User, string>(new User { Id = id, Name = "Test" }));
    }

    private Result<User, string> ValidateUserAsync(User user)
    {
        return ValidateUser(user);
    }

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    #endregion
}

