using System;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class BiMapTests
{
    #region Basic BiMap Tests

    [Fact]
    public void BiMap_Success_MapsSuccessValue()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act
        var mapped = result.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public void BiMap_Failure_MapsErrorValue()
    {
        // Arrange
        var result = Result.Failure<int, string>("error message");

        // Act
        var mapped = result.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error); // "error message".Length
    }

    [Fact]
    public void BiMap_Success_TransformsTypes()
    {
        // Arrange
        var result = Result.Success<int, string>(100);

        // Act
        var mapped = result.BiMap(
            value => value > 50,
            error => error.StartsWith("E"));

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.True(mapped.Value);
    }

    [Fact]
    public void BiMap_Failure_TransformsTypes()
    {
        // Arrange
        var result = Result.Failure<int, string>("Error");

        // Act
        var mapped = result.BiMap(
            value => value > 50,
            error => error.StartsWith("E"));

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.True(mapped.Error);
    }

    [Fact]
    public void BiMap_BothFunctionsProvided_OnlySuccessExecutes()
    {
        // Arrange
        var result = Result.Success<int, string>(42);
        var successCalled = false;
        var errorCalled = false;

        // Act
        var mapped = result.BiMap(
            value => { successCalled = true; return value.ToString(); },
            error => { errorCalled = true; return error.Length; });

        // Assert
        Assert.True(successCalled);
        Assert.False(errorCalled);
    }

    [Fact]
    public void BiMap_BothFunctionsProvided_OnlyErrorExecutes()
    {
        // Arrange
        var result = Result.Failure<int, string>("error");
        var successCalled = false;
        var errorCalled = false;

        // Act
        var mapped = result.BiMap(
            value => { successCalled = true; return value.ToString(); },
            error => { errorCalled = true; return error.Length; });

        // Assert
        Assert.False(successCalled);
        Assert.True(errorCalled);
    }

    #endregion

    #region Task BiMap Tests

    [Fact]
    public async Task BiMap_TaskResult_Success_MapsSuccessValue()
    {
        // Arrange
        var taskResult = Task.FromResult(Result.Success<int, string>(42));

        // Act
        var mapped = await taskResult.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task BiMap_TaskResult_Failure_MapsErrorValue()
    {
        // Arrange
        var taskResult = Task.FromResult(Result.Failure<int, string>("error message"));

        // Act
        var mapped = await taskResult.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error);
    }

    [Fact]
    public async Task BiMapAsync_TaskResult_Success_MapsWithAsyncFunctions()
    {
        // Arrange
        var taskResult = Task.FromResult(Result.Success<int, string>(42));

        // Act
        var mapped = await taskResult.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task BiMapAsync_TaskResult_Failure_MapsWithAsyncFunctions()
    {
        // Arrange
        var taskResult = Task.FromResult(Result.Failure<int, string>("error message"));

        // Act
        var mapped = await taskResult.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error);
    }

    [Fact]
    public async Task BiMapAsync_SyncResult_Success_MapsWithAsyncFunctions()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act
        var mapped = await result.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task BiMapAsync_SyncResult_Failure_MapsWithAsyncFunctions()
    {
        // Arrange
        var result = Result.Failure<int, string>("error message");

        // Act
        var mapped = await result.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error);
    }

    #endregion

    #region ValueTask BiMap Tests

    [Fact]
    public async Task BiMap_ValueTaskResult_Success_MapsSuccessValue()
    {
        // Arrange
        var taskResult = ValueTask.FromResult(Result.Success<int, string>(42));

        // Act
        var mapped = await taskResult.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task BiMap_ValueTaskResult_Failure_MapsErrorValue()
    {
        // Arrange
        var taskResult = ValueTask.FromResult(Result.Failure<int, string>("error message"));

        // Act
        var mapped = await taskResult.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error);
    }

    [Fact]
    public async Task BiMapAsync_ValueTaskResult_Success_MapsWithAsyncFunctions()
    {
        // Arrange
        var taskResult = ValueTask.FromResult(Result.Success<int, string>(42));

        // Act
        var mapped = await taskResult.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task BiMapAsync_ValueTaskResult_Failure_MapsWithAsyncFunctions()
    {
        // Arrange
        var taskResult = ValueTask.FromResult(Result.Failure<int, string>("error message"));

        // Act
        var mapped = await taskResult.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error);
    }

    [Fact]
    public async Task BiMapAsync_ValueTask_SyncResult_Success_MapsWithAsyncFunctions()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act
        var mapped = await result.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }

    [Fact]
    public async Task BiMapAsync_ValueTask_SyncResult_Failure_MapsWithAsyncFunctions()
    {
        // Arrange
        var result = Result.Failure<int, string>("error message");

        // Act
        var mapped = await result.BiMapAsync(
            async value => { await Task.Delay(1); return value.ToString(); },
            async error => { await Task.Delay(1); return error.Length; });

        // Assert
        Assert.True(mapped.IsFailure);
        Assert.Equal(13, mapped.Error);
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void BiMap_NullSuccessFunction_ThrowsArgumentNullException()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            result.BiMap(null!, error => error.Length));
    }

    [Fact]
    public void BiMap_NullFailureFunction_ThrowsArgumentNullException()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            result.BiMap(value => value.ToString(), null!));
    }

    [Fact]
    public async Task BiMap_TaskResult_NullSuccessFunction_ThrowsArgumentNullException()
    {
        // Arrange
        var taskResult = Task.FromResult(Result.Success<int, string>(42));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await taskResult.BiMap(null!, error => error.Length));
    }

    [Fact]
    public async Task BiMapAsync_NullSuccessFunction_ThrowsArgumentNullException()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await result.BiMapAsync(null!, async error => error.Length));
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_ConvertResultTypes_ForApiResponse()
    {
        // Arrange
        var internalResult = Result.Success<User, DatabaseError>(
            new User { Id = 1, Name = "Alice" });

        // Act - Convert internal types to API types
        var apiResult = internalResult.BiMap(
            user => new UserDto { Id = user.Id, DisplayName = user.Name },
            dbError => new ApiError { Code = dbError.Code, Message = dbError.Message });

        // Assert
        Assert.True(apiResult.IsSuccess);
        Assert.Equal(1, apiResult.Value.Id);
        Assert.Equal("Alice", apiResult.Value.DisplayName);
    }

    [Fact]
    public void Scenario_TransformBothChannels_ValidationResult()
    {
        // Arrange
        var validationResult = Result.Failure<string, ValidationErrors>(
            new ValidationErrors { Errors = new[] { "Invalid email" } });

        // Act - Transform to user-friendly messages
        var userResult = validationResult.BiMap(
            value => $"Validated: {value}",
            errors => string.Join(", ", errors.Errors));

        // Assert
        Assert.True(userResult.IsFailure);
        Assert.Equal("Invalid email", userResult.Error);
    }

    [Fact]
    public void Scenario_SwapSuccessAndFailure_InvertLogic()
    {
        // Arrange
        var result = Result.Success<bool, bool>(true);

        // Act - Swap the meaning of success and failure
        var inverted = result.BiMap(
            success => !success,
            failure => !failure);

        // Assert
        Assert.True(inverted.IsSuccess);
        Assert.False(inverted.Value);
    }

    [Fact]
    public async Task Scenario_EnrichWithAsyncData_BothChannels()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Act - Enrich both channels with async data
        var enriched = await result.BiMapAsync(
            async id => { await Task.Delay(1); return new User { Id = id, Name = "User" + id }; },
            async error => { await Task.Delay(1); return new ErrorDetails { Message = error, Timestamp = DateTime.Now }; });

        // Assert
        Assert.True(enriched.IsSuccess);
        Assert.Equal(42, enriched.Value.Id);
        Assert.Equal("User42", enriched.Value.Name);
    }

    [Fact]
    public void Scenario_NormalizeErrorTypes_ToCommonFormat()
    {
        // Arrange
        var differentErrors = new[]
        {
            Result.Failure<int, object>("string error"),
            Result.Failure<int, object>(404),
            Result.Failure<int, object>(new Exception("exception error"))
        };

        // Act - Normalize all errors to string
        var normalized = differentErrors.Select(r => r.BiMap(
            value => value.ToString(),
            error => error switch
            {
                string s => s,
                int code => $"Error code: {code}",
                Exception ex => ex.Message,
                _ => "Unknown error"
            })).ToArray();

        // Assert
        Assert.All(normalized, r => Assert.True(r.IsFailure));
        Assert.Equal("string error", normalized[0].Error);
        Assert.Equal("Error code: 404", normalized[1].Error);
        Assert.Equal("exception error", normalized[2].Error);
    }

    [Fact]
    public void Scenario_LoggingTransformation_AddContextToBothChannels()
    {
        // Arrange
        var result = Result.Success<int, string>(100);

        // Act - Add logging context
        var logged = result.BiMap(
            value => new LoggedValue<int> { Value = value, Context = "Success processing" },
            error => new LoggedError { Error = error, Context = "Failure processing" });

        // Assert
        Assert.True(logged.IsSuccess);
        Assert.Equal(100, logged.Value.Value);
        Assert.Equal("Success processing", logged.Value.Context);
    }

    #endregion

    #region Comparison with Map and MapError

    [Fact]
    public void BiMap_EquivalentToMapAndMapError_WhenAppliedSeparately()
    {
        // Arrange
        var result = Result.Success<int, string>(42);

        // Using BiMap
        var biMapped = result.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Using Map + MapError
        var mapped = result
            .Map(value => value.ToString())
            .MapError(error => error.Length);

        // Assert - Both produce same result
        Assert.Equal(biMapped.IsSuccess, mapped.IsSuccess);
        Assert.Equal(biMapped.Value, mapped.Value);
    }

    [Fact]
    public void BiMap_MoreEfficientThan_SeparateMapAndMapError()
    {
        // Arrange
        var result = Result.Failure<int, string>("error");

        // BiMap evaluates once
        var biMapped = result.BiMap(
            value => value.ToString(),
            error => error.Length);

        // Map + MapError evaluates twice (though both are efficient)
        var mapped = result
            .Map(value => value.ToString())
            .MapError(error => error.Length);

        // Assert - Same result
        Assert.Equal(biMapped.IsFailure, mapped.IsFailure);
        Assert.Equal(biMapped.Error, mapped.Error);
    }

    #endregion

    #region Helper Classes

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    private class UserDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }

    private class DatabaseError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    private class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    private class ValidationErrors
    {
        public string[] Errors { get; set; } = Array.Empty<string>();
    }

    private class ErrorDetails
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    private class LoggedValue<T>
    {
        public T Value { get; set; } = default!;
        public string Context { get; set; } = string.Empty;
    }

    private class LoggedError
    {
        public string Error { get; set; } = string.Empty;
        public string Context { get; set; } = string.Empty;
    }

    #endregion
}

