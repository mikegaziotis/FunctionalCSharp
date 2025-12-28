using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Roufe.Tests;

public class PartitionExtensionsTests
{
    #region Result Partition Tests

    [Fact]
    public void Partition_Results_MixedSuccessAndFailure_SeparatesCorrectly()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error1"),
            Result.Success<int, string>(2),
            Result.Failure<int, string>("error2"),
            Result.Success<int, string>(3)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, successes);
        Assert.Equal(new[] { "error1", "error2" }, failures);
    }

    [Fact]
    public void Partition_Results_AllSuccesses_ReturnsAllInSuccesses()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2),
            Result.Success<int, string>(3)
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, successes);
        Assert.Empty(failures);
    }

    [Fact]
    public void Partition_Results_AllFailures_ReturnsAllInFailures()
    {
        // Arrange
        var results = new[]
        {
            Result.Failure<int, string>("error1"),
            Result.Failure<int, string>("error2"),
            Result.Failure<int, string>("error3")
        };

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        Assert.Empty(successes);
        Assert.Equal(new[] { "error1", "error2", "error3" }, failures);
    }

    [Fact]
    public void Partition_Results_EmptyCollection_ReturnsBothEmpty()
    {
        // Arrange
        var results = Array.Empty<Result<int, string>>();

        // Act
        var (successes, failures) = results.Partition();

        // Assert
        Assert.Empty(successes);
        Assert.Empty(failures);
    }

    [Fact]
    public void PartitionToLists_Results_ReturnsPartitionResultObject()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error"),
            Result.Success<int, string>(2)
        };

        // Act
        var partition = results.PartitionToLists();

        // Assert
        Assert.Equal(2, partition.SuccessCount);
        Assert.Equal(1, partition.FailureCount);
        Assert.Equal(3, partition.TotalCount);
        Assert.True(partition.HasSuccesses);
        Assert.True(partition.HasFailures);
        Assert.False(partition.AllSucceeded);
        Assert.False(partition.AllFailed);
    }

    [Fact]
    public void PartitionToLists_AllSuccesses_AllSucceededIsTrue()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2)
        };

        // Act
        var partition = results.PartitionToLists();

        // Assert
        Assert.True(partition.AllSucceeded);
        Assert.False(partition.AllFailed);
        Assert.False(partition.HasFailures);
    }

    [Fact]
    public void PartitionToLists_AllFailures_AllFailedIsTrue()
    {
        // Arrange
        var results = new[]
        {
            Result.Failure<int, string>("error1"),
            Result.Failure<int, string>("error2")
        };

        // Act
        var partition = results.PartitionToLists();

        // Assert
        Assert.False(partition.AllSucceeded);
        Assert.True(partition.AllFailed);
        Assert.False(partition.HasSuccesses);
    }

    [Fact]
    public void PartitionResult_Deconstruct_Works()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error")
        };

        // Act
        var (successes, failures) = results.PartitionToLists();

        // Assert
        Assert.Single(successes);
        Assert.Single(failures);
    }

    [Fact]
    public async Task Partition_TaskResults_Works()
    {
        // Arrange
        var tasks = new[]
        {
            Task.FromResult(Result.Success<int, string>(1)),
            Task.FromResult(Result.Failure<int, string>("error")),
            Task.FromResult(Result.Success<int, string>(2))
        };

        // Act
        var (successes, failures) = await tasks.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2 }, successes);
        Assert.Equal(new[] { "error" }, failures);
    }

    [Fact]
    public async Task Partition_TaskOfResults_Works()
    {
        // Arrange
        var taskOfResults = Task.FromResult<IEnumerable<Result<int, string>>>(new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error")
        });

        // Act
        var (successes, failures) = await taskOfResults.Partition();

        // Assert
        Assert.Single(successes);
        Assert.Single(failures);
    }

    [Fact]
    public async Task Partition_ValueTaskResults_Works()
    {
        // Arrange
        var tasks = new[]
        {
            ValueTask.FromResult(Result.Success<int, string>(1)),
            ValueTask.FromResult(Result.Failure<int, string>("error")),
            ValueTask.FromResult(Result.Success<int, string>(2))
        };

        // Act
        var (successes, failures) = await tasks.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2 }, successes);
        Assert.Equal(new[] { "error" }, failures);
    }

    #endregion

    #region Option Partition Tests

    [Fact]
    public void Partition_Options_MixedSomeAndNone_SeparatesCorrectly()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.None,
            Option<int>.From(2),
            Option<int>.None,
            Option<int>.From(3)
        };

        // Act
        var (values, noneCount) = options.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, values);
        Assert.Equal(2, noneCount);
    }

    [Fact]
    public void Partition_Options_AllSome_ReturnsAllValues()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.From(2),
            Option<int>.From(3)
        };

        // Act
        var (values, noneCount) = options.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, values);
        Assert.Equal(0, noneCount);
    }

    [Fact]
    public void Partition_Options_AllNone_ReturnsEmptyValues()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.None,
            Option<int>.None,
            Option<int>.None
        };

        // Act
        var (values, noneCount) = options.Partition();

        // Assert
        Assert.Empty(values);
        Assert.Equal(3, noneCount);
    }

    [Fact]
    public void Partition_Options_Empty_ReturnsBothEmpty()
    {
        // Arrange
        var options = Array.Empty<Option<int>>();

        // Act
        var (values, noneCount) = options.Partition();

        // Assert
        Assert.Empty(values);
        Assert.Equal(0, noneCount);
    }

    [Fact]
    public void PartitionToResult_Options_ReturnsPartitionOptionObject()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.None,
            Option<int>.From(2)
        };

        // Act
        var partition = options.PartitionToResult();

        // Assert
        Assert.Equal(2, partition.ValueCount);
        Assert.Equal(1, partition.NoneCount);
        Assert.Equal(3, partition.TotalCount);
        Assert.True(partition.HasValues);
        Assert.True(partition.HasNones);
        Assert.False(partition.AllHaveValues);
        Assert.False(partition.AllNone);
    }

    [Fact]
    public void PartitionToResult_AllSome_AllHaveValuesIsTrue()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.From(2)
        };

        // Act
        var partition = options.PartitionToResult();

        // Assert
        Assert.True(partition.AllHaveValues);
        Assert.False(partition.AllNone);
        Assert.False(partition.HasNones);
    }

    [Fact]
    public void PartitionToResult_AllNone_AllNoneIsTrue()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.None,
            Option<int>.None
        };

        // Act
        var partition = options.PartitionToResult();

        // Assert
        Assert.False(partition.AllHaveValues);
        Assert.True(partition.AllNone);
        Assert.False(partition.HasValues);
    }

    [Fact]
    public void PartitionOption_Deconstruct_Works()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.None
        };

        // Act
        var (values, noneCount) = options.PartitionToResult();

        // Assert
        Assert.Single(values);
        Assert.Equal(1, noneCount);
    }

    [Fact]
    public async Task Partition_TaskOptions_Works()
    {
        // Arrange
        var tasks = new[]
        {
            Task.FromResult(Option<int>.From(1)),
            Task.FromResult(Option<int>.None),
            Task.FromResult(Option<int>.From(2))
        };

        // Act
        var (values, noneCount) = await tasks.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2 }, values);
        Assert.Equal(1, noneCount);
    }

    [Fact]
    public async Task Partition_TaskOfOptions_Works()
    {
        // Arrange
        var taskOfOptions = Task.FromResult<IEnumerable<Option<int>>>(new[]
        {
            Option<int>.From(1),
            Option<int>.None
        });

        // Act
        var (values, noneCount) = await taskOfOptions.Partition();

        // Assert
        Assert.Single(values);
        Assert.Equal(1, noneCount);
    }

    [Fact]
    public async Task Partition_ValueTaskOptions_Works()
    {
        // Arrange
        var tasks = new[]
        {
            ValueTask.FromResult(Option<int>.From(1)),
            ValueTask.FromResult(Option<int>.None),
            ValueTask.FromResult(Option<int>.From(2))
        };

        // Act
        var (values, noneCount) = await tasks.Partition();

        // Assert
        Assert.Equal(new[] { 1, 2 }, values);
        Assert.Equal(1, noneCount);
    }

    #endregion

    #region PartitionWith Result Tests

    [Fact]
    public void PartitionWith_Result_SeparatesBasedOnPredicate()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4, 5, 6 };

        // Act
        var (evens, odds) = numbers.PartitionWith(n =>
            n % 2 == 0
                ? Result.Success<int, int>(n)
                : Result.Failure<int, int>(n));

        // Assert
        Assert.Equal(new[] { 2, 4, 6 }, evens);
        Assert.Equal(new[] { 1, 3, 5 }, odds);
    }

    [Fact]
    public void PartitionWith_Result_WithValidation()
    {
        // Arrange
        var strings = new[] { "1", "2", "invalid", "3", "bad" };

        // Act
        var (numbers, errors) = strings.PartitionWith(s =>
            int.TryParse(s, out var n)
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>(s));

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, numbers);
        Assert.Equal(new[] { "invalid", "bad" }, errors);
    }

    [Fact]
    public async Task PartitionWithAsync_Result_Task_Works()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var (evens, odds) = await numbers.PartitionWithAsync(async n =>
        {
            await Task.Delay(1);
            return n % 2 == 0
                ? Result.Success<int, int>(n)
                : Result.Failure<int, int>(n);
        });

        // Assert
        Assert.Equal(new[] { 2, 4 }, evens);
        Assert.Equal(new[] { 1, 3 }, odds);
    }

    [Fact]
    public async Task PartitionWithAsync_Result_ValueTask_Works()
    {
        // Arrange
        var numbers = new[] { 1, 2, 3, 4 };

        // Act
        var (evens, odds) = await numbers.PartitionWithAsync(async n =>
        {
            await Task.Delay(1);
            return n % 2 == 0
                ? Result.Success<int, int>(n)
                : Result.Failure<int, int>(n);
        });

        // Assert
        Assert.Equal(new[] { 2, 4 }, evens);
        Assert.Equal(new[] { 1, 3 }, odds);
    }

    #endregion

    #region PartitionWith Option Tests

    [Fact]
    public void PartitionWith_Option_SeparatesBasedOnPredicate()
    {
        // Arrange
        var strings = new[] { "1", "2", "invalid", "3", "bad" };

        // Act
        var (numbers, invalidCount) = strings.PartitionWith(s =>
            int.TryParse(s, out var n)
                ? Option<int>.From(n)
                : Option<int>.None);

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, numbers);
        Assert.Equal(2, invalidCount);
    }

    [Fact]
    public void PartitionWith_Option_AllValid()
    {
        // Arrange
        var strings = new[] { "1", "2", "3" };

        // Act
        var (numbers, invalidCount) = strings.PartitionWith(s =>
            int.TryParse(s, out var n)
                ? Option<int>.From(n)
                : Option<int>.None);

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, numbers);
        Assert.Equal(0, invalidCount);
    }

    [Fact]
    public async Task PartitionWithAsync_Option_Task_Works()
    {
        // Arrange
        var strings = new[] { "1", "invalid", "2" };

        // Act
        var (numbers, invalidCount) = await strings.PartitionWithAsync(async s =>
        {
            await Task.Delay(1);
            return int.TryParse(s, out var n)
                ? Option<int>.From(n)
                : Option<int>.None;
        });

        // Assert
        Assert.Equal(new[] { 1, 2 }, numbers);
        Assert.Equal(1, invalidCount);
    }

    [Fact]
    public async Task PartitionWithAsync_Option_ValueTask_Works()
    {
        // Arrange
        var strings = new[] { "1", "invalid", "2" };

        // Act
        var (numbers, invalidCount) = await strings.PartitionWithAsync(async s =>
        {
            await Task.Delay(1);
            return int.TryParse(s, out var n)
                ? Option<int>.From(n)
                : Option<int>.None;
        });

        // Assert
        Assert.Equal(new[] { 1, 2 }, numbers);
        Assert.Equal(1, invalidCount);
    }

    #endregion

    #region Helper Methods Tests

    [Fact]
    public void ChooseSuccesses_ReturnsOnlySuccesses()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error"),
            Result.Success<int, string>(2),
            Result.Failure<int, string>("error2"),
            Result.Success<int, string>(3)
        };

        // Act
        var successes = results.ChooseSuccesses().ToArray();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, successes);
    }

    [Fact]
    public void ChooseFailures_ReturnsOnlyFailures()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Failure<int, string>("error1"),
            Result.Success<int, string>(2),
            Result.Failure<int, string>("error2")
        };

        // Act
        var failures = results.ChooseFailures().ToArray();

        // Assert
        Assert.Equal(new[] { "error1", "error2" }, failures);
    }

    [Fact]
    public void ChooseValues_ReturnsOnlyValues()
    {
        // Arrange
        var options = new[]
        {
            Option<int>.From(1),
            Option<int>.None,
            Option<int>.From(2),
            Option<int>.None,
            Option<int>.From(3)
        };

        // Act
        var values = options.ChooseValues().ToArray();

        // Assert
        Assert.Equal(new[] { 1, 2, 3 }, values);
    }

    [Fact]
    public void ChooseSuccesses_LazyEvaluation()
    {
        // Arrange
        var callCount = 0;
        var results = Enumerable.Range(1, 10).Select(n =>
        {
            callCount++;
            return n % 2 == 0
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>("odd");
        });

        // Act - just create the enumerable
        var successes = results.ChooseSuccesses();

        // Assert - not evaluated yet
        Assert.Equal(0, callCount);

        // Now evaluate
        var list = successes.ToList();
        Assert.Equal(10, callCount);
        Assert.Equal(new[] { 2, 4, 6, 8, 10 }, list);
    }

    #endregion

    #region Null Argument Tests

    [Fact]
    public void Partition_Results_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<Result<int, string>> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Partition());
    }

    [Fact]
    public void Partition_Options_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<Option<int>> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.Partition());
    }

    [Fact]
    public void PartitionWith_Result_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullSource = null!;
        Func<int, Result<int, string>> selector = n => Result.Success<int, string>(n);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.PartitionWith(selector));
    }

    [Fact]
    public void PartitionWith_Result_NullSelector_ThrowsArgumentNullException()
    {
        // Arrange
        var source = new[] { 1, 2, 3 };
        Func<int, Result<int, string>> nullSelector = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => source.PartitionWith(nullSelector));
    }

    [Fact]
    public void ChooseSuccesses_NullSource_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<Result<int, string>> nullSource = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullSource.ChooseSuccesses().ToList());
    }

    #endregion

    #region Real-World Scenarios

    [Fact]
    public void Scenario_ParseUserInputs_SeparateValidAndInvalid()
    {
        // Arrange
        var inputs = new[] { "10", "20", "invalid", "30", "bad", "40" };

        Func<string, Result<int, string>> parseNumber = s =>
            int.TryParse(s, out var n)
                ? Result.Success<int, string>(n)
                : Result.Failure<int, string>($"'{s}' is not a valid number");

        // Act
        var (validNumbers, errors) = inputs.PartitionWith(parseNumber);

        // Assert
        Assert.Equal(new[] { 10, 20, 30, 40 }, validNumbers);
        Assert.Equal(2, errors.Count());
        Assert.Contains("'invalid' is not a valid number", errors);
        Assert.Contains("'bad' is not a valid number", errors);
    }

    [Fact]
    public async Task Scenario_ValidateUsers_SeparateValidAndInvalid()
    {
        // Arrange
        var userIds = new[] { 1, 2, 3, 4, 5 };

        Func<int, Task<Result<User, string>>> validateUser = async id =>
        {
            await Task.Delay(1);
            return id % 2 == 0
                ? Result.Success<User, string>(new User { Id = id, Name = $"User{id}" })
                : Result.Failure<User, string>($"User {id} is invalid");
        };

        // Act
        var (validUsers, errors) = await userIds.PartitionWithAsync(validateUser);

        // Assert
        Assert.Equal(2, validUsers.Count());
        Assert.Equal(3, errors.Count());
        Assert.All(validUsers, u => Assert.True(u.Id % 2 == 0));
    }

    [Fact]
    public void Scenario_ProcessBatchResults_GetStatistics()
    {
        // Arrange
        var results = new[]
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2),
            Result.Failure<int, string>("error1"),
            Result.Success<int, string>(3),
            Result.Failure<int, string>("error2"),
            Result.Failure<int, string>("error3")
        };

        // Act
        var partition = results.PartitionToLists();

        // Assert
        Assert.Equal(3, partition.SuccessCount);
        Assert.Equal(3, partition.FailureCount);
        Assert.Equal(6, partition.TotalCount);
        Assert.False(partition.AllSucceeded);
        Assert.False(partition.AllFailed);

        // Can use the results
        var sum = partition.Successes.Sum();
        Assert.Equal(6, sum);

        var errorMessages = string.Join(", ", partition.Failures);
        Assert.Contains("error1", errorMessages);
    }

    [Fact]
    public void Scenario_FilterOptionalFields_GetValidOnes()
    {
        // Arrange
        var data = new[]
        {
            new { Id = 1, Name = "Alice", Age = (int?)25 },
            new { Id = 2, Name = "Bob", Age = (int?)null },
            new { Id = 3, Name = "Charlie", Age = (int?)30 },
            new { Id = 4, Name = "David", Age = (int?)null }
        };

        // Act
        var (withAge, withoutAgeCount) = data.PartitionWith(d =>
            d.Age.HasValue
                ? Option<(int Id, string Name, int Age)>.From((d.Id, d.Name, d.Age.Value))
                : Option<(int Id, string Name, int Age)>.None);

        // Assert
        Assert.Equal(2, withAge.Count());
        Assert.Equal(2, withoutAgeCount);
        Assert.All(withAge, item => Assert.True(item.Age > 0));
    }

    [Fact]
    public void Scenario_ProcessBatch_OnlyGetSuccesses()
    {
        // Arrange
        var results = Enumerable.Range(1, 100).Select(n =>
            n % 10 == 0
                ? Result.Failure<int, string>($"Error at {n}")
                : Result.Success<int, string>(n * 2));

        // Act
        var successes = results.ChooseSuccesses().ToArray();

        // Assert
        Assert.Equal(90, successes.Length); // 100 - 10 failures
        Assert.DoesNotContain(20, successes); // 10 * 2 should be filtered
        Assert.DoesNotContain(40, successes); // 20 * 2 should be filtered
    }

    #endregion

    private class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

