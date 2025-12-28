using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class TraverseSequenceExtensionsTests
{
    #region Result Sequence Tests

    [Fact]
    public void Sequence_AllSuccess_ReturnsSuccessWithAllValues()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2),
            Result.Success<int, string>(3)
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        Assert.True(sequenced.IsSuccess);
        Assert.Equal(new[] { 1, 2, 3 }, sequenced.Value);
    }

    [Fact]
    public void Sequence_OneFailure_ReturnsFirstFailure()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error1"),
            Result.Success<int, string>(3),
            Result.Failure<int, string>("error2")
        };

        // Act
        var sequenced = results.Sequence();

        // Assert
        Assert.True(sequenced.IsFailure);
        Assert.Equal("error1", sequenced.Error);
    }

    [Fact]
    public void Sequence_EmptyCollection_ReturnsSuccessWithEmptyCollection()
    {
        // Arrange
        var results = Array.Empty<Result<int, string>>();

        // Act
        var sequenced = results.Sequence();

        // Assert
        Assert.True(sequenced.IsSuccess);
        Assert.Empty(sequenced.Value);
    }

    [Fact]
    public async Task Sequence_TaskResults_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        var tasks = new[]
        {
            Task.FromResult(Result.Success<int, string>(1)),
            Task.FromResult(Result.Success<int, string>(2)),
            Task.FromResult(Result.Success<int, string>(3))
        };

        // Act
        var sequenced = await tasks.Sequence();

        // Assert
        Assert.True(sequenced.IsSuccess);
        Assert.Equal(new[] { 1, 2, 3 }, sequenced.Value);
    }

    [Fact]
    public async Task Sequence_TaskResults_OneFailure_ReturnsFailure()
    {
        // Arrange
        var tasks = new[]
        {
            Task.FromResult(Result.Success<int, string>(1)),
            Task.FromResult(Result.Failure<int, string>("error")),
            Task.FromResult(Result.Success<int, string>(3))
        };

        // Act
        var sequenced = await tasks.Sequence();

        // Assert
        Assert.True(sequenced.IsFailure);
        Assert.Equal("error", sequenced.Error);
    }

    [Fact]
    public async Task Sequence_TaskOfResults_ReturnsCorrectResult()
    {
        // Arrange
        var taskOfResults = Task.FromResult<IEnumerable<Result<int, string>>>(new[]
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2)
        });

        // Act
        var sequenced = await taskOfResults.Sequence();

        // Assert
        Assert.True(sequenced.IsSuccess);
        Assert.Equal(new[] { 1, 2 }, sequenced.Value);
    }

    [Fact]
    public async Task Sequence_ValueTaskResults_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        var tasks = new[]
        {
            ValueTask.FromResult(Result.Success<int, string>(1)),
            ValueTask.FromResult(Result.Success<int, string>(2)),
            ValueTask.FromResult(Result.Success<int, string>(3))
        };

        // Act
        var sequenced = await tasks.Sequence();

        // Assert
        Assert.True(sequenced.IsSuccess);
        Assert.Equal(new[] { 1, 2, 3 }, sequenced.Value);
    }

    #endregion

    #region Result Traverse Tests

    [Fact]
    public void Traverse_AllSuccess_ReturnsSuccessWithTransformedValues()
    {
        // Arrange
        var strings = new[] { "1", "2", "3" };
        Func<string, Result<int, string>> tryParse = s =>
            int.TryParse(s, out var n)
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>($"Cannot parse {s}");

        // Act
        var traversed = strings.Traverse(tryParse);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Equal(new[] { 1, 2, 3 }, traversed.Value);
    }

    [Fact]
    public void Traverse_OneFailure_ReturnsFirstFailure()
    {
        // Arrange
        var strings = new[] { "1", "invalid", "3" };
        Func<string, Result<int, string>> tryParse = s =>
            int.TryParse(s, out var n)
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>($"Cannot parse {s}");

        // Act
        var traversed = strings.Traverse(tryParse);

        // Assert
        Assert.True(traversed.IsFailure);
        Assert.Equal("Cannot parse invalid", traversed.Error);
    }

    [Fact]
    public void Traverse_EmptyCollection_ReturnsSuccessWithEmpty()
    {
        // Arrange
        var strings = Array.Empty<string>();
        Func<string, Result<int, string>> tryParse = s => Result.Success<int, string>(0);

        // Act
        var traversed = strings.Traverse(tryParse);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Empty(traversed.Value);
    }

    [Fact]
    public async Task TraverseAsync_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, Task<Result<int, string>>> square = async n =>
        {
            await Task.Delay(1);
            return Result.Success<int, string>(n * n);
        };

        // Act
        var traversed = await numbers.TraverseAsync(square);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.Value);
    }

    [Fact]
    public async Task TraverseAsync_OneFailure_ReturnsFailure()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, Task<Result<int, string>>> process = async n =>
        {
            await Task.Delay(1);
            return n == 2
                ? Result.Failure<int, string>("error at 2")
                : Result.Success<int, string>(n);
        };

        // Act
        var traversed = await numbers.TraverseAsync(process);

        // Assert
        Assert.True(traversed.IsFailure);
        Assert.Equal("error at 2", traversed.Error);
    }

    [Fact]
    public async Task Traverse_TaskCollection_WithSyncFunc_ReturnsCorrectResult()
    {
        // Arrange
        var taskCollection = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });
        Func<int, Result<int, string>> square = n => Result.Success<int, string>(n * n);

        // Act
        var traversed = await taskCollection.Traverse(square);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.Value);
    }

    [Fact]
    public async Task TraverseAsync_TaskCollection_WithAsyncFunc_ReturnsCorrectResult()
    {
        // Arrange
        var taskCollection = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });
        Func<int, Task<Result<int, string>>> square = async n =>
        {
            await Task.Delay(1);
            return Result.Success<int, string>(n * n);
        };

        // Act
        var traversed = await taskCollection.TraverseAsync(square);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.Value);
    }

    [Fact]
    public async Task TraverseAsync_ValueTask_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, ValueTask<Result<int, string>>> square = async n =>
        {
            await Task.Delay(1);
            return Result.Success<int, string>(n * n);
        };

        // Act
        var traversed = await numbers.TraverseAsync(square);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.Value);
    }

    #endregion

    #region Option Sequence Tests

    [Fact]
    public void Sequence_Options_AllSome_ReturnsSomeWithAllValues()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.From(2),
            Option<int>.From(3)
        };

        // Act
        var sequenced = options.Sequence();

        // Assert
        Assert.True(sequenced.HasValue);
        Assert.Equal(new[] { 1, 2, 3 }, sequenced.GetValueOrThrow());
    }

    [Fact]
    public void Sequence_Options_OneNone_ReturnsNone()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.None,
            Option<int>.From(3)
        };

        // Act
        var sequenced = options.Sequence();

        // Assert
        Assert.True(sequenced.HasNoValue);
    }

    [Fact]
    public void Sequence_Options_Empty_ReturnsSomeWithEmpty()
    {
        // Arrange
        var options = Array.Empty<Option<int>>();

        // Act
        var sequenced = options.Sequence();

        // Assert
        Assert.True(sequenced.HasValue);
        Assert.Empty(sequenced.GetValueOrThrow());
    }

    [Fact]
    public async Task Sequence_TaskOptions_AllSome_ReturnsSome()
    {
        // Arrange
        var tasks = new[]
        {
            Task.FromResult(Option<int>.From(1)),
            Task.FromResult(Option<int>.From(2)),
            Task.FromResult(Option<int>.From(3))
        };

        // Act
        var sequenced = await tasks.Sequence();

        // Assert
        Assert.True(sequenced.HasValue);
        Assert.Equal(new[] { 1, 2, 3 }, sequenced.GetValueOrThrow());
    }

    [Fact]
    public async Task Sequence_TaskOptions_OneNone_ReturnsNone()
    {
        // Arrange
        var tasks = new[]
        {
            Task.FromResult(Option<int>.From(1)),
            Task.FromResult(Option<int>.None),
            Task.FromResult(Option<int>.From(3))
        };

        // Act
        var sequenced = await tasks.Sequence();

        // Assert
        Assert.True(sequenced.HasNoValue);
    }

    [Fact]
    public async Task Sequence_TaskOfOptions_ReturnsCorrectResult()
    {
        // Arrange
        var taskOfOptions = Task.FromResult<IEnumerable<Option<int>>>(new[]
        {
            Option<int>.From(1),
            Option<int>.From(2)
        });

        // Act
        var sequenced = await taskOfOptions.Sequence();

        // Assert
        Assert.True(sequenced.HasValue);
        Assert.Equal(new[] { 1, 2 }, sequenced.GetValueOrThrow());
    }

    [Fact]
    public async Task Sequence_ValueTaskOptions_AllSome_ReturnsSome()
    {
        // Arrange
        var tasks = new[]
        {
            ValueTask.FromResult(Option<int>.From(1)),
            ValueTask.FromResult(Option<int>.From(2)),
            ValueTask.FromResult(Option<int>.From(3))
        };

        // Act
        var sequenced = await tasks.Sequence();

        // Assert
        Assert.True(sequenced.HasValue);
        Assert.Equal(new[] { 1, 2, 3 }, sequenced.GetValueOrThrow());
    }

    #endregion

    #region Option Traverse Tests

    [Fact]
    public void Traverse_Options_AllSome_ReturnsSomeWithTransformed()
    {
        // Arrange
        var strings = new[] { "1", "2", "3" };
        Func<string, Option<int>> tryParse = s =>
            int.TryParse(s, out var n) ? Option<int>.From(n) : Option<int>.None;

        // Act
        var traversed = strings.Traverse(tryParse);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Equal(new[] { 1, 2, 3 }, traversed.GetValueOrThrow());
    }

    [Fact]
    public void Traverse_Options_OneNone_ReturnsNone()
    {
        // Arrange
        var strings = new[] { "1", "invalid", "3" };
        Func<string, Option<int>> tryParse = s =>
            int.TryParse(s, out var n) ? Option<int>.From(n) : Option<int>.None;

        // Act
        var traversed = strings.Traverse(tryParse);

        // Assert
        Assert.True(traversed.HasNoValue);
    }

    [Fact]
    public void Traverse_Options_Empty_ReturnsSomeWithEmpty()
    {
        // Arrange
        var strings = Array.Empty<string>();
        Func<string, Option<int>> tryParse = s => Option<int>.From(0);

        // Act
        var traversed = strings.Traverse(tryParse);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Empty(traversed.GetValueOrThrow());
    }

    [Fact]
    public async Task TraverseAsync_Options_AllSome_ReturnsSome()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, Task<Option<int>>> square = async n =>
        {
            await Task.Delay(1);
            return Option<int>.From(n * n);
        };

        // Act
        var traversed = await numbers.TraverseAsync(square);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.GetValueOrThrow());
    }

    [Fact]
    public async Task TraverseAsync_Options_OneNone_ReturnsNone()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, Task<Option<int>>> process = async n =>
        {
            await Task.Delay(1);
            return n == 2 ? Option<int>.None : Option<int>.From(n);
        };

        // Act
        var traversed = await numbers.TraverseAsync(process);

        // Assert
        Assert.True(traversed.HasNoValue);
    }

    [Fact]
    public async Task Traverse_Options_TaskCollection_WithSyncFunc_ReturnsCorrectResult()
    {
        // Arrange
        var taskCollection = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });
        Func<int, Option<int>> square = n => Option<int>.From(n * n);

        // Act
        var traversed = await taskCollection.Traverse(square);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.GetValueOrThrow());
    }

    [Fact]
    public async Task TraverseAsync_Options_TaskCollection_WithAsyncFunc_ReturnsCorrectResult()
    {
        // Arrange
        var taskCollection = Task.FromResult<IEnumerable<int>>(new[] { 1, 2, 3 });
        Func<int, Task<Option<int>>> square = async n =>
        {
            await Task.Delay(1);
            return Option<int>.From(n * n);
        };

        // Act
        var traversed = await taskCollection.TraverseAsync(square);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.GetValueOrThrow());
    }

    [Fact]
    public async Task TraverseAsync_Options_ValueTask_AllSome_ReturnsSome()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3 };
        Func<int, ValueTask<Option<int>>> square = async n =>
        {
            await Task.Delay(1);
            return Option<int>.From(n * n);
        };

        // Act
        var traversed = await numbers.TraverseAsync(square);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Equal(new[] { 1, 4, 9 }, traversed.GetValueOrThrow());
    }

    #endregion

    #region Parallel Traverse Tests

    [Fact]
    public async Task TraverseParallel_Results_AllSuccess_ReturnsSuccess()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 10).ToArray();
        var callCount = 0;

        Func<int, Task<Result<int, string>>> square = async n =>
        {
            System.Threading.Interlocked.Increment(ref callCount);
            await Task.Delay(10);
            return Result.Success<int, string>(n * n);
        };

        // Act
        var traversed = await numbers.TraverseParallel(square);

        // Assert
        Assert.True(traversed.IsSuccess);
        Assert.Equal(numbers.Select(n => n * n), traversed.Value);
        Assert.Equal(10, callCount);
    }

    [Fact]
    public async Task TraverseParallel_Results_OneFailure_ReturnsFailure()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };
        Func<int, Task<Result<int, string>>> process = async n =>
        {
            await Task.Delay(10);
            return n == 3
                ? Result.Failure<int, string>("error at 3")
                : Result.Success<int, string>(n);
        };

        // Act
        var traversed = await numbers.TraverseParallel(process);

        // Assert
        Assert.True(traversed.IsFailure);
        Assert.Equal("error at 3", traversed.Error);
    }

    [Fact]
    public async Task TraverseParallel_Options_AllSome_ReturnsSome()
    {
        // Arrange
        var numbers = Enumerable.Range(1, 10).ToArray();

        Func<int, Task<Option<int>>> square = async n =>
        {
            await Task.Delay(10);
            return Option<int>.From(n * n);
        };

        // Act
        var traversed = await numbers.TraverseParallel(square);

        // Assert
        Assert.True(traversed.HasValue);
        Assert.Equal(numbers.Select(n => n * n), traversed.GetValueOrThrow());
    }

    [Fact]
    public async Task TraverseParallel_Options_OneNone_ReturnsNone()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5 };
        Func<int, Task<Option<int>>> process = async n =>
        {
            await Task.Delay(10);
            return n == 3 ? Option<int>.None : Option<int>.From(n);
        };

        // Act
        var traversed = await numbers.TraverseParallel(process);

        // Assert
        Assert.True(traversed.HasNoValue);
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void Sequence_Results_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<Result<int, string>> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Sequence());
    }

    [Fact]
    public void Traverse_Results_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullSource = null!;
        Func<int, Result<int, string>> func = n => Result.Success<int, string>(n);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Traverse(func));
    }

    [Fact]
    public void Traverse_Results_NullFunc_ThrowsArgumentNullException()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        Func<int, Result<int, string>> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.Traverse(nullFunc));
    }

    [Fact]
    public void Sequence_Options_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<Option<int>> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Sequence());
    }

    [Fact]
    public void Traverse_Options_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullSource = null!;
        Func<int, Option<int>> func = n => Option<int>.From(n);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Traverse(func));
    }

    [Fact]
    public void Traverse_Options_NullFunc_ThrowsArgumentNullException()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        Func<int, Option<int>> nullFunc = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.Traverse(nullFunc));
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_ParseAllStrings_AllValid_ReturnsSuccess()
    {
        // Arrange
        var strings = new[] { "10", "20", "30", "40" };

        Func<string, Result<int, string>> parseNumber = s =>
            int.TryParse(s, out var n)
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>($"'{s}' is not a valid number");

        // Act
        var result = strings.Traverse(parseNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 10, 20, 30, 40 }, result.Value);
    }

    [Fact]
    public void Scenario_ParseAllStrings_OneInvalid_ReturnsFailure()
    {
        // Arrange
        var strings = new[] { "10", "20", "invalid", "40" };

        Func<string, Result<int, string>> parseNumber = s =>
            int.TryParse(s, out var n)
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>($"'{s}' is not a valid number");

        // Act
        var result = strings.Traverse(parseNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("'invalid' is not a valid number", result.Error);
    }

    [Fact]
    public async Task Scenario_ValidateUsers_AllValid_ReturnsSuccess()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3 };

        Func<int, Task<Result<User, string>>> validateUser = async id =>
        {
            await Task.Delay(10);
            return id > 0
                ? Result.Success<User, string>(new User { Id = id, Name = $"User{id}" })
                : Result.Failure<User, string>($"Invalid user ID: {id}");
        };

        // Act
        var result = await userIds.TraverseAsync(validateUser);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(3, result.Value.Count());
        Assert.All(result.Value, u => Assert.True(u.Id > 0));
    }

    [Fact]
    public void Scenario_LookupOptionalValues_AllPresent_ReturnsSome()
    {
        // Arrange
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

        // Act
        var result = keys.Traverse(lookup);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(new[] { 1, 2, 3 }, result.GetValueOrThrow());
    }

    [Fact]
    public void Scenario_LookupOptionalValues_OneMissing_ReturnsNone()
    {
        // Arrange
        var keys = new[] { "a", "b", "missing", "c" };
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

        // Act
        var result = keys.Traverse(lookup);

        // Assert
        Assert.True(result.HasNoValue);
    }

    #endregion

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

